using System.Collections.Generic;
using ChessGame;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseInputController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Camera targetCamera;
    [SerializeField] private PieceColor humanColor = PieceColor.White;
    [SerializeField] private bool debugLogging = true;
    [SerializeField] private Material selectedPieceMaterial;
    [SerializeField] private Material validMoveMaterial;

    private ChessPiece selectedPiece;
    private Material previousPieceMaterial;
    private Renderer selectedRenderer;
    private readonly List<Vector2Int> validMoves = new List<Vector2Int>();
    private readonly List<GameObject> moveHighlights = new List<GameObject>();

    private void Start()
    {
        if (gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
        }

        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        if (selectedPieceMaterial == null)
        {
            selectedPieceMaterial = CreateRuntimeMaterial(new Color(1f, 0.85f, 0.25f, 1f));
        }

        if (validMoveMaterial == null)
        {
            validMoveMaterial = CreateRuntimeMaterial(new Color(0.2f, 0.9f, 0.2f, 0.45f));
        }

        if (debugLogging)
        {
            Debug.Log("MouseInputController initialized. Using direct mouse input via Mouse.current.position.ReadValue().");
        }
    }

    private void Update()
    {
        if (gameManager == null || targetCamera == null || gameManager.IsGameOver)
        {
            return;
        }

        if (gameManager.CurrentTurn != humanColor)
        {
            return;
        }

        if (Mouse.current == null)
        {
            return;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandleClick();
        }
    }

    private void HandleClick()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = targetCamera.ScreenPointToRay(mousePosition);

        if (debugLogging)
        {
            Debug.Log($"Click detected at {mousePosition}.");
        }

        if (!Physics.Raycast(ray, out RaycastHit hitInfo, 500f))
        {
            if (debugLogging)
            {
                Debug.Log("Raycast did not hit any object.");
            }

            Deselect();
            return;
        }

        ChessPiece clickedPiece = hitInfo.collider.GetComponentInParent<ChessPiece>();
        if (clickedPiece != null)
        {
            if (debugLogging)
            {
                Debug.Log($"Raycast hit piece: {clickedPiece.name} at {clickedPiece.BoardX},{clickedPiece.BoardY}.");
            }

            OnPieceClicked(clickedPiece);
            return;
        }

        if (debugLogging)
        {
            Debug.Log($"Raycast hit non-piece: {hitInfo.collider.name}, tag={hitInfo.collider.tag}.");
        }

        bool hitBoardLikeSurface = hitInfo.collider.CompareTag("BoardClickPlane") ||
                                   hitInfo.collider.name.Contains("Board") ||
                                   hitInfo.collider.name.Contains("Plane");

        if (hitBoardLikeSurface)
        {
            OnBoardClicked(hitInfo.point);
            return;
        }

        Deselect();
    }

    private void OnPieceClicked(ChessPiece clickedPiece)
    {
        if (selectedPiece == clickedPiece)
        {
            Deselect();
            return;
        }

        if (selectedPiece != null && IsMoveValid(clickedPiece.BoardX, clickedPiece.BoardY))
        {
            bool moved = gameManager.TryMovePiece(selectedPiece, clickedPiece.BoardX, clickedPiece.BoardY);
            if (moved && debugLogging)
            {
                Debug.Log("Move executed by clicking opposing piece.");
            }

            Deselect();
            return;
        }

        if (clickedPiece.Color != humanColor || clickedPiece.Color != gameManager.CurrentTurn)
        {
            Deselect();
            return;
        }

        SelectPiece(clickedPiece);
    }

    private void OnBoardClicked(Vector3 hitPoint)
    {
        if (!gameManager.WorldToBoard(hitPoint, out int boardX, out int boardY))
        {
            Deselect();
            return;
        }

        if (debugLogging)
        {
            Debug.Log($"Board square clicked: {boardX},{boardY}.");
        }

        if (selectedPiece == null)
        {
            return;
        }

        if (!IsMoveValid(boardX, boardY))
        {
            Deselect();
            return;
        }

        gameManager.TryMovePiece(selectedPiece, boardX, boardY);
        Deselect();
    }

    private void SelectPiece(ChessPiece piece)
    {
        Deselect();
        selectedPiece = piece;
        validMoves.Clear();
        validMoves.AddRange(gameManager.GetLegalMovesForPiece(piece));
        DrawValidMoveHighlights();

        selectedRenderer = piece.GetComponentInChildren<Renderer>();
        if (selectedRenderer != null)
        {
            previousPieceMaterial = selectedRenderer.material;
            selectedRenderer.material = selectedPieceMaterial;
        }
    }

    private void Deselect()
    {
        if (selectedRenderer != null && previousPieceMaterial != null)
        {
            selectedRenderer.material = previousPieceMaterial;
        }

        selectedRenderer = null;
        previousPieceMaterial = null;
        selectedPiece = null;
        validMoves.Clear();
        ClearHighlights();
    }

    private bool IsMoveValid(int x, int y)
    {
        for (int i = 0; i < validMoves.Count; i++)
        {
            if (validMoves[i].x == x && validMoves[i].y == y)
            {
                return true;
            }
        }

        return false;
    }

    private void DrawValidMoveHighlights()
    {
        ClearHighlights();

        for (int i = 0; i < validMoves.Count; i++)
        {
            Vector3 pos = gameManager.BoardToWorld(validMoves[i].x, validMoves[i].y, 0.02f);
            GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Plane);
            marker.name = $"MoveHighlight_{validMoves[i].x}_{validMoves[i].y}";
            marker.transform.position = pos;
            marker.transform.localScale = new Vector3(0.58f, 1f, 0.58f);

            Collider c = marker.GetComponent<Collider>();
            if (c != null)
            {
                Destroy(c);
            }

            Renderer r = marker.GetComponent<Renderer>();
            if (r != null)
            {
                r.material = validMoveMaterial;
                r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                r.receiveShadows = false;
            }

            moveHighlights.Add(marker);
        }
    }

    private void ClearHighlights()
    {
        for (int i = 0; i < moveHighlights.Count; i++)
        {
            if (moveHighlights[i] != null)
            {
                Destroy(moveHighlights[i]);
            }
        }

        moveHighlights.Clear();
    }

    private static Material CreateRuntimeMaterial(Color color)
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null)
        {
            shader = Shader.Find("Standard");
        }

        Material material = new Material(shader);
        material.color = color;
        material.SetFloat("_Surface", 1f);
        material.SetFloat("_Blend", 0f);
        material.SetFloat("_AlphaClip", 0f);
        material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
        return material;
    }
}
