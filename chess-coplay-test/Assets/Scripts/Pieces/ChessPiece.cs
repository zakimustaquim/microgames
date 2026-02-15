using System.Collections.Generic;
using UnityEngine;

namespace ChessGame
{
    public enum PieceColor
    {
        White,
        Black
    }

    public enum PieceType
    {
        Pawn,
        Rook,
        Knight,
        Bishop,
        Queen,
        King
    }

    public abstract class ChessPiece : MonoBehaviour
    {
        public PieceColor Color { get; private set; }
        public PieceType PieceType { get; protected set; }
        public int BoardX { get; private set; }
        public int BoardY { get; private set; }
        public bool HasMoved { get; set; }
        protected GameManager Manager { get; private set; }

        public void Initialize(GameManager manager, PieceColor color, int boardX, int boardY)
        {
            Manager = manager;
            Color = color;
            BoardX = boardX;
            BoardY = boardY;
            HasMoved = false;
        }

        public void SetBoardPosition(int x, int y)
        {
            BoardX = x;
            BoardY = y;
        }

        public abstract List<Vector2Int> GetLegalMoves(ChessPiece[,] board);

        protected static bool IsInsideBoard(int x, int y)
        {
            return x >= 0 && x < 8 && y >= 0 && y < 8;
        }

        protected void AddDirectionalMoves(ChessPiece[,] board, List<Vector2Int> moves, int dirX, int dirY)
        {
            int x = BoardX + dirX;
            int y = BoardY + dirY;

            while (IsInsideBoard(x, y))
            {
                ChessPiece occupant = board[x, y];
                if (occupant == null)
                {
                    moves.Add(new Vector2Int(x, y));
                }
                else
                {
                    if (occupant.Color != Color)
                    {
                        moves.Add(new Vector2Int(x, y));
                    }

                    break;
                }

                x += dirX;
                y += dirY;
            }
        }
    }
}
