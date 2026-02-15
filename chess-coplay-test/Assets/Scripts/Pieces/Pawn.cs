using System.Collections.Generic;
using UnityEngine;

namespace ChessGame
{
    public class Pawn : ChessPiece
    {
        private void Awake()
        {
            PieceType = PieceType.Pawn;
        }

        public override List<Vector2Int> GetLegalMoves(ChessPiece[,] board)
        {
            List<Vector2Int> moves = new List<Vector2Int>();
            int direction = Color == PieceColor.White ? 1 : -1;

            int oneStepY = BoardY + direction;
            if (IsInsideBoard(BoardX, oneStepY) && board[BoardX, oneStepY] == null)
            {
                moves.Add(new Vector2Int(BoardX, oneStepY));

                int twoStepY = BoardY + (2 * direction);
                if (!HasMoved && IsInsideBoard(BoardX, twoStepY) && board[BoardX, twoStepY] == null)
                {
                    moves.Add(new Vector2Int(BoardX, twoStepY));
                }
            }

            int[] captureOffsets = { -1, 1 };
            foreach (int offset in captureOffsets)
            {
                int captureX = BoardX + offset;
                int captureY = BoardY + direction;
                if (!IsInsideBoard(captureX, captureY))
                {
                    continue;
                }

                ChessPiece occupant = board[captureX, captureY];
                if (occupant != null && occupant.Color != Color)
                {
                    moves.Add(new Vector2Int(captureX, captureY));
                }
            }

            return moves;
        }
    }
}
