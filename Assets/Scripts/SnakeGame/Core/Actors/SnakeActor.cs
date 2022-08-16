using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SnakeGame
{
    public class SnakeActor : Actor<SnakeActor.Setup>
    {
        [Serializable]
        public class Setup
        {
            public SnakeConfigsAsset configsAsset;
            public SnakeTypeAsset snakeTypeAsset { get; set; }
            public Color color = Color.white;
            public Direction startDirection;
            public Vector2Int startPosition;

            public BlockAsset[] startBlocks => snakeTypeAsset.startingBlocks;
            public TimerActor.Setup respawnGhostTimer => configsAsset.respawnGhostTimer;
            public float baseSpeed => configsAsset.baseSpeed;
            public float minSpeed => configsAsset.minSpeed;

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

        public Piece GetFacingPiece()
        {
            Vector2Int targetPos = head.position + moveDirection;
            targetPos = match.board.WrapPosition(targetPos);

            return match.board.GetTopPiece(targetPos);
        }

        public event Action Died;
        public event Action Hit;
        public event Action Moved;
        public event Action PiecesChanged;
        public event Action<int> PieceReplaced;
        public event Action<bool> GhostlyChanged;

        private TimerActor ghostlyTimer;
        private RefreshTimer moveTimer;

        private List<SnakeSegmentPiece> _pieces;

        public SnakeSegmentPiece head => _pieces[0];
        public SnakeSegmentPiece tail => _pieces[Length - 1];

        public SnakeSegmentPiece this[int i] => _pieces[i];
        public int Length => _pieces.Count;

        public Color Color => setup.color;
        public ISnakeOwner owner { get; set; }

        public IEnumerable<SnakeSegmentPiece> GetPieces() => _pieces.AsReadOnly();

        

        public Vector2Int moveDirection { get; private set; }

        private Vector2Int startPosition;

        public Vector2Int lookVector
        {
            get
            {
                var result = head.position - head.next.position;
                //If magnitude is greater than zero, this means it's wrapped around the board
                if (result.magnitude > 1)
                {
                    //Normalized it and flip it
                    result = new Vector2Int((int)(result.x / result.magnitude), (int)(result.y / result.magnitude));
                    result = -result;
                }
                return result;
            }
        }

        public IEnumerable<Vector2Int> GetDirectionOptions()=> new[] { lookVector, lookVector.SpinClockwise(), lookVector.SpinCounterclockwise() };
        public IEnumerable<Vector2Int> GetWrappedMovementOptions()
        {
            return GetDirectionOptions().Select(dir=>
            {
                Vector2Int targetPos = head.position + dir;
                return match.board.WrapPosition(targetPos);
            });
        }

        public bool WasHit { get; private set; }
        public bool IsAlive { get; private set; }
        public bool IsGhostly { get; private set; }
        
        
        protected override void OnInitialize()
        {
            moveDirection = setup.startDirection.ToVector();
            startPosition = setup.startPosition;
            ghostlyTimer = match.SpawnActor<TimerActor, TimerActor.Setup>(setup.respawnGhostTimer);
            IsAlive = true;

            BuildSnake();

        }
        public override void OnMatchStart()
        {
            //throw new System.NotImplementedException();
            RefreshSpeed();
        }

        private void RefreshSpeed()
        {
            float speed = setup.baseSpeed + _pieces.Sum(p => p.block.speedModifier);
            speed = Mathf.Max(setup.minSpeed, speed);
            moveTimer = RefreshTimer.CreateAndStart(1 / speed);
        }

        public override void Tick()
        {
            if(IsAlive && moveTimer.Check())
            {
                match.snakeMover.EnqueueForMovement(this);
            }
            if(IsGhostly && ghostlyTimer.IsOver)
            {
                IsGhostly = false;
                GhostlyChanged?.Invoke(false);
            }
        }



        public void RespawnAt(Vector2Int startPosition)
        {
            _pieces.Clear();
            moveTimer.Restart();
            this.startPosition = startPosition;
            this.moveDirection = setup.startDirection.ToVector();
            BuildSnake();
            IsAlive = true;
            PiecesChanged?.Invoke();
            StartGhostly();
        }

        public void OnHit()
        {
            WasHit = true;
            Hit?.Invoke();
        }
        public void OnDead()
        {
            foreach (var piece in _pieces)
            {
                match.board.Detatch(piece);
            }
            IsAlive = false;
            WasHit = false;
            Died?.Invoke();
            owner.OnSnakeDead();
        }
        public void OnMoved()
        {
            Moved?.Invoke();
        }

        //Movement
        public void SetMoveDirection(Vector2Int direction)
        {
            Debug.Assert(direction.magnitude == 1);

            this.moveDirection = direction;

        }
        public void FlipLeft()
        {
            var target = moveDirection.SpinCounterclockwise();
            if (target != -lookVector)
                moveDirection = target;
        }
        public void FlipRight()
        {
            var target = moveDirection.SpinClockwise();
            if (target != -lookVector)
                moveDirection = target;
        }

        //Pieces
        public void CreateNewHead(BlockAsset type, Vector2Int position)
        {
            var newPiece = new SnakeSegmentPiece();
            newPiece.block = type;
            newPiece.snake = this;
            newPiece.snakeIndex = 0;
            _pieces.Insert(0, newPiece);
            match.board.AttachPiece(position, newPiece);
            RefreshPieceIndices();
            RefreshSpeed();
            PiecesChanged?.Invoke();
        }
        public void ReplacePiece(int index, BlockAsset consumedVersion)
        {
            _pieces[index].block = consumedVersion;
            RefreshSpeed();
            PieceReplaced?.Invoke(index);
        }

        private void RefreshPieceIndices()
        {
            for (int i = 0; i < _pieces.Count; i++)
            {
                _pieces[i].snakeIndex = i;
            }
        }


        private void StartGhostly()
        {
            ghostlyTimer.Restart();
            IsGhostly = true;
            GhostlyChanged?.Invoke(true);
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

                match.board.AttachPiece(positions[i], piece, canStack:true);
                previous = piece;
                _pieces.Add(piece);
            }
            RefreshSpeed();
        }
       
    }

}
