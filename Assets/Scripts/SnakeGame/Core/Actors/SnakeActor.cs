using Common;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SnakeGame
{
    public class SnakeActor : Actor<SnakeActor.Setup>
    {
        [Serializable]
        public class Setup
        {
            public Direction startDirection;
            public Vector2Int startPosition;
            public BlockAsset[] startBlocks;
            public Color color = Color.white;

            public Vector2Int[] GeneratePositions()
            {
                Vector2Int[] positions = new Vector2Int[startBlocks.Length];

                Vector2Int dir = startDirection.ToVector();

                for (int i = 0; i < positions.Length; i++)
                {
                    positions[i] = startPosition - dir * i;
                }

                return positions;
            }
        }

        private List<SnakeSegmentPiece> _pieces;

        public SnakeSegmentPiece this[int i] => _pieces[i];
        public int Length => _pieces.Count;

        public Color Color => setup.color;

        public IEnumerable<SnakeSegmentPiece> GetPieces() => _pieces.AsReadOnly();

        public Direction moveDirection;

        public override void OnMatchStart()
        {
            //throw new System.NotImplementedException();
        }

        protected override void OnInitialize()
        {
            moveDirection = setup.startDirection;

            BuildSnake();

        }

        private void BuildSnake()
        {
            Debug.Assert(setup.startBlocks.Length > 0);
            Vector2Int[] positions = setup.GeneratePositions();

            _pieces = new List<SnakeSegmentPiece>();
            SnakeSegmentPiece previous = null;

            for (int i = 0; i < positions.Length; i++)
            {
                SnakeSegmentPiece piece = new SnakeSegmentPiece();
                piece.snake = this;
                piece.snakeIndex = i;
                piece.block = setup.startBlocks[i];

                match.board.AttachPiece(positions[i], piece);
                previous = piece;
                _pieces.Add(piece);
            }
        }

        public void FlipLeft()
        {
            moveDirection = moveDirection.SpinCounterclockwise();
        }

        public void FlipRight()
        {
            moveDirection = moveDirection.SpinClockwise();
        }
    }

}
