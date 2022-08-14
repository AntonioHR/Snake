using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SnakeGame
{
    public class Board
    {
        private Board() { }

        private List<Piece>[,] pieces;

        public Vector2Int Size { get; private set; }

        public Piece GetTopPiece(Vector2Int pos) => GetTopPiece(pos.x, pos.y);
        public Piece GetTopPiece(int x, int y) => pieces[x, y].FirstOrDefault();
        public IEnumerable<Piece> GetPiecesAt(Vector2Int pos) => pieces[pos.x, pos.y].AsReadOnly();

        public int OccupiedSlots { get; private set; }
        public int PieceSlotsTotal => Size.x * Size.y;

        public static Board BuildWithSize(Vector2Int size, bool closedByWalls)
        {
            var board = new Board()
            {
                Size=size,
                pieces = new List<Piece>[size.x, size.y]
            };
            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    board.pieces[i, j] = new List<Piece>();
                    if(closedByWalls && (i == 0 || j == 0 || i == size.x-1 || j == size.y-1))
                    {
                        board.AttachPiece(i, j, new WallPiece());
                    }
                }
            }

            return board;
        }

        public Board GetSnapshot()
        {
            return (Board) MemberwiseClone();
        }

        public void AttachPiece(Vector2Int pos, Piece piece, bool canStack = false) => AttachPiece(pos.x, pos.y, piece, canStack);
        public void AttachPiece(int x, int y, Piece piece, bool canStack = false)
        {
            bool empty = !pieces[x, y].Any();
            Debug.Assert(empty|| canStack);
            if (empty)
                OccupiedSlots++;
            pieces[x, y].Add(piece);
            piece.position = new Vector2Int(x, y);
        }

        public Vector2Int WrapPosition(Vector2Int pos)
        {
            if (pos.x < 0)
                pos.x = Size.x + (pos.x % Size.x);
            if (pos.y < 0)
                pos.y = Size.y + (pos.y % Size.y);

            return new Vector2Int(pos.x % Size.x, pos.y % Size.y);
        }
        public Vector2Int GetRandomEmptyPosition()
        {
            int availableSlots = PieceSlotsTotal - OccupiedSlots;
            Debug.Assert(availableSlots > 0);
            int targetIndex = UnityEngine.Random.Range(0, availableSlots);

            int index = 0;

            for (int i = 0; i < Size.x; i++)
            {
                for (int j = 0; j < Size.y; j++)
                {
                    if(!pieces[i, j].Any())
                    {
                        if (index == targetIndex)
                            return new Vector2Int(i, j);
                        else
                            index++;
                    }
                }
            }
            throw new IndexOutOfRangeException();
        }

        public void Detatch(Piece piece)
        {
            int x = piece.position.x;
            int y = piece.position.y;
            Debug.Assert(pieces[x, y].Contains(piece));
            pieces[x, y].Remove(piece);

            if (!pieces[x,y].Any())
                OccupiedSlots--;
        }

    }
}
