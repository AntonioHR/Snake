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

        private List<SnakeSegmentPiece> _piecesCache;
        public bool WasHit { get; private set; }


        public bool IsAlive { get => state.isAlive; private set => state.isAlive = value; }
        public bool IsGhostly { get => state.isGhostly; private set => state.isGhostly = value; }

        public SnakeSegmentPiece head => _piecesCache[0];
        public SnakeSegmentPiece tail => _piecesCache[Length - 1];

        public SnakeSegmentPiece this[int i] => _piecesCache[i];
        public int Length => _piecesCache.Count;

        public Color Color => setup.color;
        public ISnakeOwner owner { get; set; }

        public IEnumerable<SnakeSegmentPiece> GetPieces() => _piecesCache.AsReadOnly();

        

        public Vector2Int moveDirection { get => state.moveDirection; private set => state.moveDirection = value; }

        private Vector2Int startPosition;

        public Board GetTopRewindBoard()
        {
            return state.timeTravelBoards.Last();
        }

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

        public void AddTimeTravelSnapshot(Board boardSnapshot)
        {
            state.timeTravelBoards.Add(boardSnapshot);
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


        private SnakeActorState state;



        protected override void OnInitialize()
        {
            state = new SnakeActorState(this);
            match.board.AddStateObject(state);

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

        public override void OnBoardReset()
        {
            state = match.board.GetStateObjectFor<SnakeActorState>(this);
            //We find our pieces on the new board state via their positions
            if (IsAlive)
            {
                _piecesCache = state.piecePositions
                    .Select(pos =>
                        match.board.GetPiecesAt(pos)
                        .OfType<SnakeSegmentPiece>()
                        .Where(p => p.snake == this).First())
                    .ToList();
                RefreshSpeed();

                PiecesChanged?.Invoke();
            }
        }

        private void RefreshSpeed()
        {
            float speed = setup.baseSpeed + _piecesCache.Sum(p => p.block.speedModifier);
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
            _piecesCache.Clear();
            state.piecePositions.Clear();
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
            foreach (var piece in _piecesCache)
            {
                match.board.Detatch(piece);
            }
            state.piecePositions.Clear();
            IsAlive = false;
            WasHit = false;
            Died?.Invoke();
            owner.OnSnakeDead();
        }
        public void OnMoved()
        {
            RefreshPiecePositions();
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
            _piecesCache.Insert(0, newPiece);
            state.piecePositions.Insert(0, position);
            match.board.AttachPiece(position, newPiece);
            RefreshPieceIndices();
            RefreshSpeed();
            PiecesChanged?.Invoke();
        }
        public void ReplacePiece(int index, BlockAsset consumedVersion)
        {
            _piecesCache[index].block = consumedVersion;
            RefreshSpeed();
            PieceReplaced?.Invoke(index);
        }

        private void RefreshPieceIndices()
        {
            for (int i = 0; i < _piecesCache.Count; i++)
            {
                _piecesCache[i].snakeIndex = i;
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

            _piecesCache = new List<SnakeSegmentPiece>();
            SnakeSegmentPiece previous = null;

            for (int i = 0; i < positions.Length; i++)
            {
                SnakeSegmentPiece piece = new SnakeSegmentPiece();
                piece.snake = this;
                piece.snakeIndex = i;
                piece.block = setup.startBlocks[i];

                match.board.AttachPiece(positions[i], piece, canStack:true);
                previous = piece;
                _piecesCache.Add(piece);
            }
            RefreshPiecePositions();
            RefreshSpeed();
        }

        private void RefreshPiecePositions()
        {
            state.piecePositions = _piecesCache.Select(p => p.position).ToList();
        }
    }

}
