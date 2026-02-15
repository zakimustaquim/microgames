using System.Collections.Generic;
using UnityEngine;

namespace ChessGame
{
    public class King : ChessPiece
    {
        private void Awake()
        {
            PieceType = PieceType.King;
        }

        public override List<Vector2Int> GetLegalMoves(ChessPiece[,] board)
        {
            List<Vector2Int> moves = new List<Vector2Int>();
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0)
                    {
                        continue;
                    }

                    int x = BoardX + dx;
                    int y = BoardY + dy;
                    if (!IsInsideBoard(x, y))
                    {
                        continue;
                    }

                    ChessPiece occupant = board[x, y];
                    if (occupant == null || occupant.Color != Color)
                    {
                        moves.Add(new Vector2Int(x, y));
                    }
                }
            }

            return moves;
        }
    }
}
