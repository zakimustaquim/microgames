using System.Collections.Generic;
using UnityEngine;

namespace ChessGame
{
    public class Rook : ChessPiece
    {
        private void Awake()
        {
            PieceType = PieceType.Rook;
        }

        public override List<Vector2Int> GetLegalMoves(ChessPiece[,] board)
        {
            List<Vector2Int> moves = new List<Vector2Int>();
            AddDirectionalMoves(board, moves, 1, 0);
            AddDirectionalMoves(board, moves, -1, 0);
            AddDirectionalMoves(board, moves, 0, 1);
            AddDirectionalMoves(board, moves, 0, -1);
            return moves;
        }
    }
}
