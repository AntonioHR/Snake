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
            public float speed = 1.5f;
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

        public event Action Hit;
        public event Action Moved;
        public event Action PiecesChanged;
        public event Action<int> SinglePieceChanged;

        private RefreshTimer moveTimer;

        private List<SnakeSegmentPiece> _pieces;

        public SnakeSegmentPiece head => _pieces[0];
        public SnakeSegmentPiece tail => _pieces[Length - 1];

        public SnakeSegmentPiece this[int i] => _pieces[i];
        public int Length => _pieces.Count;

        public Color Color => setup.color;

        public IEnumerable<SnakeSegmentPiece> GetPieces() => _pieces.AsReadOnly();

        

        public Direction moveDirection { get; private set; }
        public Vector2Int lookVector => head.position - head.next.position;

        public bool WasHit { get; private set; }

        public override void Tick()
        {
            if(moveTimer.Check())
            {
                match.snakeMover.EnqueueForMovement(this);
            }
        }

        public void OnHit()
        {
            WasHit = true;
            Hit?.Invoke();
        }

        public override void OnMatchStart()
        {
            //throw new System.NotImplementedException();
            moveTimer = RefreshTimer.CreateAndStart(1 / setup.speed);
        }

        public void OnMoved()
        {
            Moved?.Invoke();
        }

        public void CreateNewHead(BlockAsset type, Vector2Int position)
        {
            var newPiece = new SnakeSegmentPiece();
            newPiece.block = type;
            newPiece.snake = this;
            newPiece.snakeIndex = 0;
            _pieces.Insert(0, newPiece);
            match.board.AttachPiece(position, newPiece);
            RefreshPieceIndices();
            PiecesChanged?.Invoke();
        }
        internal void ReplacePiece(int index, BlockAsset consumedVersion)
        {
            _pieces[index].block = consumedVersion;
            SinglePieceChanged?.Invoke(index);
        }

        private void RefreshPieceIndices()
        {
            for (int i = 0; i < _pieces.Count; i++)
            {
                _pieces[i].snakeIndex = i;
            }
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
            var target = moveDirection.SpinCounterclockwise();
            if(target.ToVector() != -lookVector)
                moveDirection = target;
        }

        public void FlipRight()
        {
            var target = moveDirection.SpinClockwise();
            if (target.ToVector() != -lookVector)
                moveDirection = target;
        }
    }

}
