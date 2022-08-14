using System;
using System.Collections;
using UnityEngine;

namespace SnakeGame
{
    public class Board
    {
        private Board() { }
        
        public Piece[,] pieces { get; private set; }

        public Vector2Int Size { get; private set; }

        public static Board BuildWithSize(Vector2Int size, bool closedByWalls)
        {
            var board = new Board()
            {
                Size=size,
                pieces = new Piece[size.x, size.y]
            };

            if(closedByWalls)
            {
                for (int i = 0; i < size.x; i++)
                {
                    board.AttachPiece(i, 0, new WallPiece());

                    board.AttachPiece(i, size.y - 1, new WallPiece());
                }
                for (int i = 1; i < size.y - 1; i++)
                {
                    board.AttachPiece(0, i, new WallPiece());

                    board.AttachPiece(size.x - 1, i, new WallPiece());
                }
            }

            return board;
        }

        public void AttachPiece(Vector2Int pos, Piece piece) => AttachPiece(pos.x, pos.y, piece);
        public void AttachPiece(int x, int y, Piece piece)
        {
            Debug.Assert(pieces[x, y] == null);
            pieces[x, y] = piece;
            piece.position = new Vector2Int(x, y);
        }
        public void Detatch(Vector2Int pos, Piece piece) => DetatchPiece(pos.x, pos.y);
        public void DetatchPiece(int x, int y)
        {
            Debug.Assert(pieces[x, y] != null);
            pieces[x, y] = null;

        }
    }
}
