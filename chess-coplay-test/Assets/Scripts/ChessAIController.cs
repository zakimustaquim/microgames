using System.Collections;
using System.Collections.Generic;
using ChessGame;
using UnityEngine;

public class ChessAIController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private PieceColor aiColor = PieceColor.Black;
    [SerializeField] private int difficultyDepth = 2;
    [SerializeField] private float thinkDelay = 0.35f;

    private bool isThinking;

    private struct SimulatedMove
    {
        public ChessPiece piece;
        public int fromX;
        public int fromY;
        public int toX;
        public int toY;
        public ChessPiece captured;
        public bool previousHasMoved;
        public bool capturedWasActive;
    }

    private void OnEnable()
    {
        if (gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
        }

        if (gameManager != null)
        {
            gameManager.OnTurnChanged += OnTurnChanged;
        }
    }

    private void OnDisable()
    {
        if (gameManager != null)
        {
            gameManager.OnTurnChanged -= OnTurnChanged;
        }
    }

    private void OnTurnChanged(PieceColor turn)
    {
        if (isThinking || gameManager == null || gameManager.IsGameOver)
        {
            return;
        }

        if (turn != aiColor)
        {
            return;
        }

        StartCoroutine(ThinkAndMove());
    }

    private IEnumerator ThinkAndMove()
    {
        isThinking = true;
        yield return new WaitForSeconds(thinkDelay);

        List<SimulatedMove> moves = GetAllMovesForColor(aiColor);
        if (moves.Count == 0)
        {
            gameManager.EndGame(aiColor == PieceColor.White ? PieceColor.Black : PieceColor.White);
            isThinking = false;
            yield break;
        }

        int alpha = int.MinValue;
        int beta = int.MaxValue;
        int bestScore = int.MinValue;
        SimulatedMove bestMove = moves[0];

        for (int i = 0; i < moves.Count; i++)
        {
            SimulatedMove applied = ApplyMove(moves[i]);

            int score;
            if (applied.captured != null && applied.captured.PieceType == PieceType.King)
            {
                score = 100000;
            }
            else
            {
                score = Minimax(difficultyDepth - 1, Opponent(aiColor), alpha, beta);
            }

            UndoMove(applied);

            if (moves[i].captured != null && HasRaycastCollisionOpportunity(moves[i]))
            {
                score += 15;
            }

            if (score > bestScore)
            {
                bestScore = score;
                bestMove = moves[i];
            }

            if (bestScore > alpha)
            {
                alpha = bestScore;
            }
        }

        if (bestMove.captured != null)
        {
            HasRaycastCollisionOpportunity(bestMove);
        }

        gameManager.TryMoveByCoordinates(bestMove.fromX, bestMove.fromY, bestMove.toX, bestMove.toY);
        isThinking = false;
    }

    private int Minimax(int depth, PieceColor turn, int alpha, int beta)
    {
        if (depth <= 0)
        {
            return EvaluateBoard();
        }

        List<SimulatedMove> moves = GetAllMovesForColor(turn);
        if (moves.Count == 0)
        {
            return EvaluateBoard();
        }

        bool maximizing = turn == aiColor;
        int best = maximizing ? int.MinValue : int.MaxValue;

        for (int i = 0; i < moves.Count; i++)
        {
            SimulatedMove applied = ApplyMove(moves[i]);

            int score;
            if (applied.captured != null && applied.captured.PieceType == PieceType.King)
            {
                score = maximizing ? 100000 : -100000;
            }
            else
            {
                score = Minimax(depth - 1, Opponent(turn), alpha, beta);
            }

            UndoMove(applied);

            if (maximizing)
            {
                if (score > best)
                {
                    best = score;
                }

                if (best > alpha)
                {
                    alpha = best;
                }
            }
            else
            {
                if (score < best)
                {
                    best = score;
                }

                if (best < beta)
                {
                    beta = best;
                }
            }

            if (beta <= alpha)
            {
                break;
            }
        }

        return best;
    }

    private List<SimulatedMove> GetAllMovesForColor(PieceColor color)
    {
        List<SimulatedMove> moves = new List<SimulatedMove>();
        ChessPiece[,] board = gameManager.BoardState;

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                ChessPiece piece = board[x, y];
                if (piece == null || piece.Color != color || !piece.gameObject.activeSelf)
                {
                    continue;
                }

                List<Vector2Int> legal = piece.GetLegalMoves(board);
                for (int i = 0; i < legal.Count; i++)
                {
                    ChessPiece target = board[legal[i].x, legal[i].y];
                    if (target != null && target.Color == color)
                    {
                        continue;
                    }

                    moves.Add(new SimulatedMove
                    {
                        piece = piece,
                        fromX = piece.BoardX,
                        fromY = piece.BoardY,
                        toX = legal[i].x,
                        toY = legal[i].y,
                        captured = target
                    });
                }
            }
        }

        moves.Sort((a, b) =>
        {
            int valueA = a.captured == null ? 0 : PieceValue(a.captured.PieceType);
            int valueB = b.captured == null ? 0 : PieceValue(b.captured.PieceType);
            return valueB.CompareTo(valueA);
        });

        return moves;
    }

    private SimulatedMove ApplyMove(SimulatedMove move)
    {
        ChessPiece[,] board = gameManager.BoardState;

        move.previousHasMoved = move.piece.HasMoved;
        move.capturedWasActive = move.captured != null && move.captured.gameObject.activeSelf;

        board[move.fromX, move.fromY] = null;
        board[move.toX, move.toY] = move.piece;

        move.piece.SetBoardPosition(move.toX, move.toY);
        move.piece.HasMoved = true;

        if (move.captured != null)
        {
            move.captured.gameObject.SetActive(false);
        }

        return move;
    }

    private void UndoMove(SimulatedMove move)
    {
        ChessPiece[,] board = gameManager.BoardState;

        board[move.toX, move.toY] = move.captured;
        board[move.fromX, move.fromY] = move.piece;

        move.piece.SetBoardPosition(move.fromX, move.fromY);
        move.piece.HasMoved = move.previousHasMoved;

        if (move.captured != null)
        {
            move.captured.gameObject.SetActive(move.capturedWasActive);
        }
    }

    private int EvaluateBoard()
    {
        int score = 0;
        ChessPiece[,] board = gameManager.BoardState;

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                ChessPiece piece = board[x, y];
                if (piece == null || !piece.gameObject.activeSelf)
                {
                    continue;
                }

                int pieceValue = PieceValue(piece.PieceType);
                int centralBonus = 3 - Mathf.Abs(3 - x) + 3 - Mathf.Abs(3 - y);
                int total = pieceValue + centralBonus;
                score += piece.Color == aiColor ? total : -total;
            }
        }

        return score;
    }

    private static int PieceValue(PieceType type)
    {
        return type switch
        {
            PieceType.Pawn => 10,
            PieceType.Knight => 30,
            PieceType.Bishop => 30,
            PieceType.Rook => 50,
            PieceType.Queen => 90,
            PieceType.King => 900,
            _ => 0
        };
    }

    private static PieceColor Opponent(PieceColor color)
    {
        return color == PieceColor.White ? PieceColor.Black : PieceColor.White;
    }

    private static bool HasRaycastCollisionOpportunity(SimulatedMove move)
    {
        if (move.piece == null || move.captured == null)
        {
            return false;
        }

        Vector3 start = move.piece.transform.position + Vector3.up * 1.5f;
        Vector3 end = move.captured.transform.position + Vector3.up * 1.5f;
        Vector3 direction = end - start;
        float distance = direction.magnitude;
        if (distance <= 0.001f)
        {
            return false;
        }

        if (Physics.Raycast(start, direction.normalized, out RaycastHit hit, distance + 0.5f))
        {
            ChessPiece hitPiece = hit.collider.GetComponentInParent<ChessPiece>();
            return hitPiece == move.captured;
        }

        return false;
    }
}
