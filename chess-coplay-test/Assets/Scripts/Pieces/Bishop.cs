using System.Collections.Generic;
using UnityEngine;

namespace ChessGame
{
    public class Bishop : ChessPiece
    {
        private void Awake()
        {
            PieceType = PieceType.Bishop;
        }

        public override List<Vector2Int> GetLegalMoves(ChessPiece[,] board)
        {
            List<Vector2Int> moves = new List<Vector2Int>();
            AddDirectionalMoves(board, moves, 1, 1);
            AddDirectionalMoves(board, moves, -1, 1);
            AddDirectionalMoves(board, moves, 1, -1);
            AddDirectionalMoves(board, moves, -1, -1);
            return moves;
        }
    }
}
