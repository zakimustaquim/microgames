using System.Collections.Generic;
using UnityEngine;

namespace ChessGame
{
    public class Knight : ChessPiece
    {
        private static readonly Vector2Int[] Offsets =
        {
            new Vector2Int(1, 2),
            new Vector2Int(2, 1),
            new Vector2Int(2, -1),
            new Vector2Int(1, -2),
            new Vector2Int(-1, -2),
            new Vector2Int(-2, -1),
            new Vector2Int(-2, 1),
            new Vector2Int(-1, 2)
        };

        private void Awake()
        {
            PieceType = PieceType.Knight;
        }

        public override List<Vector2Int> GetLegalMoves(ChessPiece[,] board)
        {
            List<Vector2Int> moves = new List<Vector2Int>();
            for (int i = 0; i < Offsets.Length; i++)
            {
                int x = BoardX + Offsets[i].x;
                int y = BoardY + Offsets[i].y;
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

            return moves;
        }
    }
}
