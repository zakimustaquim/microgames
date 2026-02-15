using System;
using System.Collections.Generic;
using ChessGame;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public const float BoardSize = 48f;
    public const float SquareSize = 6f;

    [Header("Board Prefab")]
    [SerializeField] private GameObject chessBoardPrefab;

    [Header("White Prefabs")]
    [SerializeField] private GameObject whitePawnPrefab;
    [SerializeField] private GameObject whiteRookPrefab;
    [SerializeField] private GameObject whiteKnightPrefab;
    [SerializeField] private GameObject whiteBishopPrefab;
    [SerializeField] private GameObject whiteQueenPrefab;
    [SerializeField] private GameObject whiteKingPrefab;

    [Header("Black Prefabs")]
    [SerializeField] private GameObject blackPawnPrefab;
    [SerializeField] private GameObject blackRookPrefab;
    [SerializeField] private GameObject blackKnightPrefab;
    [SerializeField] private GameObject blackBishopPrefab;
    [SerializeField] private GameObject blackQueenPrefab;
    [SerializeField] private GameObject blackKingPrefab;

    [Header("Initialization")]
    [SerializeField] private bool autoInitialize = true;

    private readonly ChessPiece[,] board = new ChessPiece[8, 8];
    private PieceColor currentTurn = PieceColor.White;
    private bool gameOver;

    public event Action<PieceColor> OnTurnChanged;
    public event Action<PieceColor> OnGameOver;

    public ChessPiece[,] BoardState => board;
    public PieceColor CurrentTurn => currentTurn;
    public bool IsGameOver => gameOver;

    public List<Vector2Int> GetLegalMovesForPiece(ChessPiece piece)
    {
        List<Vector2Int> legalMoves = new List<Vector2Int>();
        if (piece == null || !ChessPieceIsAtExpectedCell(piece))
        {
            return legalMoves;
        }

        List<Vector2Int> pseudoLegal = piece.GetLegalMoves(board);
        for (int i = 0; i < pseudoLegal.Count; i++)
        {
            Vector2Int target = pseudoLegal[i];
            ChessPiece targetPiece = board[target.x, target.y];

            if (targetPiece != null && targetPiece.PieceType == PieceType.King)
            {
                continue;
            }

            MoveSimulation simulation = ApplySimulationMove(piece, target.x, target.y);
            bool ownKingInCheck = IsKingInCheck(piece.Color);
            UndoSimulationMove(simulation);

            if (!ownKingInCheck)
            {
                legalMoves.Add(target);
            }
        }

        return legalMoves;
    }

    public bool HasAnyLegalMove(PieceColor color)
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                ChessPiece piece = board[x, y];
                if (piece == null || piece.Color != color)
                {
                    continue;
                }

                if (GetLegalMovesForPiece(piece).Count > 0)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool IsKingInCheck(PieceColor kingColor)
    {
        if (!TryFindKing(kingColor, out Vector2Int kingPos))
        {
            return true;
        }

        PieceColor enemyColor = kingColor == PieceColor.White ? PieceColor.Black : PieceColor.White;
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                ChessPiece enemyPiece = board[x, y];
                if (enemyPiece == null || enemyPiece.Color != enemyColor)
                {
                    continue;
                }

                List<Vector2Int> attacks = enemyPiece.GetLegalMoves(board);
                for (int i = 0; i < attacks.Count; i++)
                {
                    if (attacks[i].x == kingPos.x && attacks[i].y == kingPos.y)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private void Start()
    {
        if (!autoInitialize)
        {
            return;
        }

        InitializeGame();
    }

    [ContextMenu("Initialize Chess Game")]
    public void InitializeGame()
    {
        gameOver = false;
        ClearExistingPieces();
        EnsureCameraSetup();
        EnsureChessBoard();
        EnsureBoardClickPlane();
        SpawnAllPieces();
        currentTurn = PieceColor.White;
        OnTurnChanged?.Invoke(currentTurn);
    }

    public bool TryMovePiece(ChessPiece piece, int toX, int toY)
    {
        if (piece == null || gameOver)
        {
            return false;
        }

        if (piece.Color != currentTurn)
        {
            return false;
        }

        if (!ChessPieceIsAtExpectedCell(piece))
        {
            return false;
        }

        List<Vector2Int> legalMoves = GetLegalMovesForPiece(piece);
        Vector2Int target = new Vector2Int(toX, toY);
        if (!legalMoves.Contains(target))
        {
            return false;
        }

        ChessPiece captured = board[toX, toY];
        if (captured != null && captured.Color == piece.Color)
        {
            return false;
        }

        board[piece.BoardX, piece.BoardY] = null;
        board[toX, toY] = piece;

        piece.SetBoardPosition(toX, toY);
        piece.HasMoved = true;
        piece.transform.position = BoardToWorld(toX, toY, piece.PieceType == PieceType.Pawn ? 1f : 0f);

        if (captured != null)
        {
            Destroy(captured.gameObject);
        }

        PieceColor opponent = piece.Color == PieceColor.White ? PieceColor.Black : PieceColor.White;
        bool opponentHasMove = HasAnyLegalMove(opponent);
        bool opponentInCheck = IsKingInCheck(opponent);

        if (!opponentHasMove)
        {
            if (opponentInCheck)
            {
                EndGame(piece.Color);
            }
            else
            {
                EndDraw();
            }

            return true;
        }

        currentTurn = opponent;
        OnTurnChanged?.Invoke(currentTurn);
        return true;
    }

    public bool TryMoveByCoordinates(int fromX, int fromY, int toX, int toY)
    {
        if (!IsInsideBoard(fromX, fromY) || !IsInsideBoard(toX, toY))
        {
            return false;
        }

        ChessPiece piece = board[fromX, fromY];
        return TryMovePiece(piece, toX, toY);
    }

    public bool WorldToBoard(Vector3 worldPos, out int boardX, out int boardY)
    {
        float half = BoardSize * 0.5f;
        float localX = worldPos.x + half;
        float localY = worldPos.z + half;

        boardX = Mathf.FloorToInt(localX / SquareSize);
        boardY = Mathf.FloorToInt(localY / SquareSize);
        return IsInsideBoard(boardX, boardY);
    }

    public Vector3 BoardToWorld(int boardX, int boardY, float y)
    {
        float half = BoardSize * 0.5f;
        float x = -half + (boardX * SquareSize) + (SquareSize * 0.5f);
        float z = -half + (boardY * SquareSize) + (SquareSize * 0.5f);
        return new Vector3(x, y, z);
    }

    public void EndGame(PieceColor winner)
    {
        if (gameOver)
        {
            return;
        }

        gameOver = true;
        Debug.Log($"Game over. Winner: {winner}");
        OnGameOver?.Invoke(winner);
    }

    public void EndDraw()
    {
        if (gameOver)
        {
            return;
        }

        gameOver = true;
        Debug.Log("Game over. Draw by stalemate.");
    }

    private static bool IsInsideBoard(int x, int y)
    {
        return x >= 0 && x < 8 && y >= 0 && y < 8;
    }

    private bool ChessPieceIsAtExpectedCell(ChessPiece piece)
    {
        if (!IsInsideBoard(piece.BoardX, piece.BoardY))
        {
            return false;
        }

        return board[piece.BoardX, piece.BoardY] == piece;
    }

    private bool TryFindKing(PieceColor kingColor, out Vector2Int kingPos)
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                ChessPiece piece = board[x, y];
                if (piece != null && piece.Color == kingColor && piece.PieceType == PieceType.King)
                {
                    kingPos = new Vector2Int(x, y);
                    return true;
                }
            }
        }

        kingPos = default;
        return false;
    }

    private struct MoveSimulation
    {
        public ChessPiece Piece;
        public ChessPiece Captured;
        public int FromX;
        public int FromY;
        public int ToX;
        public int ToY;
        public bool PreviousHasMoved;
        public bool CapturedWasActive;
    }

    private MoveSimulation ApplySimulationMove(ChessPiece piece, int toX, int toY)
    {
        MoveSimulation simulation = new MoveSimulation
        {
            Piece = piece,
            Captured = board[toX, toY],
            FromX = piece.BoardX,
            FromY = piece.BoardY,
            ToX = toX,
            ToY = toY,
            PreviousHasMoved = piece.HasMoved,
            CapturedWasActive = board[toX, toY] != null && board[toX, toY].gameObject.activeSelf
        };

        board[simulation.FromX, simulation.FromY] = null;
        board[toX, toY] = piece;
        piece.SetBoardPosition(toX, toY);
        piece.HasMoved = true;

        if (simulation.Captured != null)
        {
            simulation.Captured.gameObject.SetActive(false);
        }

        return simulation;
    }

    private void UndoSimulationMove(MoveSimulation simulation)
    {
        board[simulation.ToX, simulation.ToY] = simulation.Captured;
        board[simulation.FromX, simulation.FromY] = simulation.Piece;

        simulation.Piece.SetBoardPosition(simulation.FromX, simulation.FromY);
        simulation.Piece.HasMoved = simulation.PreviousHasMoved;

        if (simulation.Captured != null)
        {
            simulation.Captured.gameObject.SetActive(simulation.CapturedWasActive);
        }
    }

    private void ClearExistingPieces()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                board[x, y] = null;
            }
        }

        ChessPiece[] existingPieces = FindObjectsByType<ChessPiece>(FindObjectsSortMode.None);
        for (int i = 0; i < existingPieces.Length; i++)
        {
            Destroy(existingPieces[i].gameObject);
        }
    }

    private void EnsureCameraSetup()
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            return;
        }

        cam.transform.position = new Vector3(0f, 40f, -40f);
        cam.transform.rotation = Quaternion.Euler(50f, 0f, 0f);
    }

    private void EnsureChessBoard()
    {
        GameObject existingBoard = GameObject.Find("ChessBoard");
        if (existingBoard == null)
        {
            if (chessBoardPrefab == null)
            {
                Debug.LogError("Chess board prefab is missing on GameManager.");
                return;
            }

            existingBoard = Instantiate(chessBoardPrefab, Vector3.zero, Quaternion.identity);
            existingBoard.name = "ChessBoard";
        }

        existingBoard.transform.position = Vector3.zero;
        existingBoard.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
    }

    private void EnsureBoardClickPlane()
    {
        const string clickPlaneName = "BoardClickPlane";
        GameObject clickPlane = GameObject.Find(clickPlaneName);
        if (clickPlane == null)
        {
            clickPlane = new GameObject(clickPlaneName);
        }

        clickPlane.transform.position = new Vector3(0f, 0f, 0f);
        clickPlane.transform.rotation = Quaternion.identity;
        clickPlane.transform.localScale = new Vector3(4.8f, 1f, 4.8f);

        MeshFilter meshFilter = clickPlane.GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            meshFilter = clickPlane.AddComponent<MeshFilter>();
        }

        MeshCollider meshCollider = clickPlane.GetComponent<MeshCollider>();
        if (meshCollider == null)
        {
            meshCollider = clickPlane.AddComponent<MeshCollider>();
        }

        MeshRenderer renderer = clickPlane.GetComponent<MeshRenderer>();
        if (renderer == null)
        {
            renderer = clickPlane.AddComponent<MeshRenderer>();
        }

        Mesh planeMesh = GetPlaneMesh();
        meshFilter.sharedMesh = planeMesh;
        meshCollider.sharedMesh = planeMesh;

        if (renderer.sharedMaterial == null)
        {
            Material planeMaterial = new Material(FindDefaultShader());
            planeMaterial.color = new Color(1f, 1f, 1f, 0f);
            renderer.sharedMaterial = planeMaterial;
        }

        EnsureTagExistsInEditor("BoardClickPlane");
        clickPlane.tag = "BoardClickPlane";
    }

    private Mesh GetPlaneMesh()
    {
        GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Plane);
        Mesh mesh = temp.GetComponent<MeshFilter>().sharedMesh;
        Destroy(temp);
        return mesh;
    }

    private static Shader FindDefaultShader()
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null)
        {
            shader = Shader.Find("Standard");
        }

        return shader;
    }

    private void SpawnAllPieces()
    {
        for (int file = 0; file < 8; file++)
        {
            SpawnPiece(whitePawnPrefab, PieceType.Pawn, PieceColor.White, file, 1);
            SpawnPiece(blackPawnPrefab, PieceType.Pawn, PieceColor.Black, file, 6);
        }

        SpawnBackRank(PieceColor.White, 0);
        SpawnBackRank(PieceColor.Black, 7);
    }

    private void SpawnBackRank(PieceColor color, int rank)
    {
        SpawnPiece(GetPrefabForPiece(PieceType.Rook, color), PieceType.Rook, color, 0, rank);
        SpawnPiece(GetPrefabForPiece(PieceType.Knight, color), PieceType.Knight, color, 1, rank);
        SpawnPiece(GetPrefabForPiece(PieceType.Bishop, color), PieceType.Bishop, color, 2, rank);
        SpawnPiece(GetPrefabForPiece(PieceType.Queen, color), PieceType.Queen, color, 3, rank);
        SpawnPiece(GetPrefabForPiece(PieceType.King, color), PieceType.King, color, 4, rank);
        SpawnPiece(GetPrefabForPiece(PieceType.Bishop, color), PieceType.Bishop, color, 5, rank);
        SpawnPiece(GetPrefabForPiece(PieceType.Knight, color), PieceType.Knight, color, 6, rank);
        SpawnPiece(GetPrefabForPiece(PieceType.Rook, color), PieceType.Rook, color, 7, rank);
    }

    private GameObject GetPrefabForPiece(PieceType pieceType, PieceColor color)
    {
        if (color == PieceColor.White)
        {
            return pieceType switch
            {
                PieceType.Pawn => whitePawnPrefab,
                PieceType.Rook => whiteRookPrefab,
                PieceType.Knight => whiteKnightPrefab,
                PieceType.Bishop => whiteBishopPrefab,
                PieceType.Queen => whiteQueenPrefab,
                PieceType.King => whiteKingPrefab,
                _ => null
            };
        }

        return pieceType switch
        {
            PieceType.Pawn => blackPawnPrefab,
            PieceType.Rook => blackRookPrefab,
            PieceType.Knight => blackKnightPrefab,
            PieceType.Bishop => blackBishopPrefab,
            PieceType.Queen => blackQueenPrefab,
            PieceType.King => blackKingPrefab,
            _ => null
        };
    }

    private void SpawnPiece(GameObject prefab, PieceType pieceType, PieceColor color, int x, int y)
    {
        if (prefab == null)
        {
            Debug.LogError($"Missing prefab for {color} {pieceType}.");
            return;
        }

        float worldY = pieceType == PieceType.Pawn ? 1f : 0f;
        Quaternion rotation = Quaternion.Euler(-90f, color == PieceColor.White ? -90f : 90f, 0f);
        GameObject instance = Instantiate(prefab, BoardToWorld(x, y, worldY), rotation);
        instance.name = $"{color}_{pieceType}_{x}_{y}";

        ChessPiece piece = instance.GetComponent<ChessPiece>();
        if (piece == null)
        {
            piece = AddPieceComponent(instance, pieceType);
        }

        if (piece == null)
        {
            Debug.LogError($"Failed to create script component for {color} {pieceType}.");
            Destroy(instance);
            return;
        }

        EnsurePieceCollider(instance);
        piece.Initialize(this, color, x, y);
        board[x, y] = piece;
    }

    private ChessPiece AddPieceComponent(GameObject target, PieceType pieceType)
    {
        return pieceType switch
        {
            PieceType.Pawn => target.AddComponent<Pawn>(),
            PieceType.Rook => target.AddComponent<Rook>(),
            PieceType.Knight => target.AddComponent<Knight>(),
            PieceType.Bishop => target.AddComponent<Bishop>(),
            PieceType.Queen => target.AddComponent<Queen>(),
            PieceType.King => target.AddComponent<King>(),
            _ => null
        };
    }

    private void EnsurePieceCollider(GameObject pieceObject)
    {
        BoxCollider collider = pieceObject.GetComponent<BoxCollider>();
        if (collider == null)
        {
            collider = pieceObject.AddComponent<BoxCollider>();
        }

        collider.size = new Vector3(0.035f, 0.03f, 0.05f);
    }

#if UNITY_EDITOR
    private static void EnsureTagExistsInEditor(string tag)
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");

        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            if (t.stringValue == tag)
            {
                return;
            }
        }

        tagsProp.InsertArrayElementAtIndex(tagsProp.arraySize);
        SerializedProperty newTag = tagsProp.GetArrayElementAtIndex(tagsProp.arraySize - 1);
        newTag.stringValue = tag;
        tagManager.ApplyModifiedProperties();
    }
#else
    private static void EnsureTagExistsInEditor(string tag) { }
#endif
}
