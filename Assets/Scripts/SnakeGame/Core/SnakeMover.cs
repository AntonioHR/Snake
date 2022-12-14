using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SnakeGame
{
    public class SnakeMover
    {
        private SnakeGameMatch match;
        private List<SnakeActor> movementQueue = new List<SnakeActor>();
        private Board boardSnapshot;

        private IEnumerable<SnakeSegmentPiece> tailsToMove => movementQueue.Select(q=>q.tail);

        public SnakeMover(SnakeGameMatch match)
        {
            this.match = match;
        }

        public void EnqueueForMovement(SnakeActor snakeActor)
        {
            movementQueue.Add(snakeActor);
        }
        public void RunMovements()
        {
            if(!movementQueue.Any())
                return;

            boardSnapshot = null;
            
            if(CheckCouldEatTimePowerup())
                boardSnapshot = match.board.GetSnapshot();

            foreach (var snake in movementQueue)
            {
                ExecuteMovementOrEat(snake);
            }

            bool anyHit = false;
            foreach (var snake in movementQueue)
            {
                if (!snake.IsGhostly)
                {
                    if (CheckHit(snake))
                    {
                        anyHit = true;
                    }
                }
            }



            movementQueue.Clear();
            if (anyHit)
                match.OnHitsHappened();
        }

        private bool CheckCouldEatTimePowerup()
        {
            return movementQueue.Any(snake =>
            {
                var piece = GetTargetPieceFor(snake);
                return (piece as FoodPiece)?.blockType is TimeTravelBlockAsset;
            });
        }

        private bool CheckHit(SnakeActor snake)
        {
            var head = snake.head;
            var pieces = match.board.GetPiecesAt(head.position);

            if(pieces.Any(p=>p != head && p.IsHazard))
            {
                SnakeSegmentPiece ramSegment = snake.GetPieces().FirstOrDefault(p => p.block is BatteringRamBlockAsset);
                if (ramSegment != null)
                {
                    var block = (BatteringRamBlockAsset)ramSegment.block;
                    BlockAsset consumedVersion = block.consumedVersion;
                    snake.ReplacePiece(ramSegment.snakeIndex, consumedVersion);
                    return false;
                }
                else
                {
                    snake.OnHit();
                    return true;
                }
            } else
            {
                return false;
            }
        }


        private void ExecuteMovementOrEat(SnakeActor snake)
        {
            Vector2Int targetPos = snake.head.position + snake.moveDirection;
            targetPos = match.board.WrapPosition(targetPos);

            FoodPiece food = match.board.GetTopPiece(targetPos) as FoodPiece;
            if (food != null)
            {
                if(food.blockType is TimeTravelBlockAsset)
                {
                    ClearFoodPieceFromSnapshot(food.position);
                    snake.AddTimeTravelSnapshot(boardSnapshot);
                }
                ExecuteEat(snake, food);
            } else
            {
                targetPos = ExecuteMovement(snake, targetPos);

            }
        }

        private void ClearFoodPieceFromSnapshot(Vector2Int position)
        {
            FoodPiece copyInSnapshot = boardSnapshot
                                    .GetPiecesAt(position).OfType<FoodPiece>()
                                    .First();
            boardSnapshot.Detatch(copyInSnapshot);
            var foodSpawnerStateInSnapshot = boardSnapshot.GetStateObjectFor<FoodSpawnerState>(copyInSnapshot.spawner);
            foodSpawnerStateInSnapshot.foodPosition = null;
        }

        private Piece GetTargetPieceFor(SnakeActor snake)
        {
            Vector2Int targetPos = snake.head.position + snake.moveDirection;
            targetPos = match.board.WrapPosition(targetPos);

            return match.board.GetTopPiece(targetPos);
        }

        private void ExecuteEat(SnakeActor snake, FoodPiece foodPiece)
        {
            match.board.Detatch(foodPiece);
            snake.CreateNewHead(foodPiece.blockType, foodPiece.position);
            match.OnPieceEaten(foodPiece);
        }
        private Vector2Int ExecuteMovement(SnakeActor snake, Vector2Int targetPos)
        {
            var nextPos = targetPos;
            Board board = match.board;
            foreach (var piece in snake.GetPieces())
            {
                Vector2Int currentPos = piece.position;
                board.Detatch(piece);
                board.AttachPiece(targetPos, piece, canStack: true);
                targetPos = currentPos;
            }
            snake.OnMoved();

            return targetPos;
        }
    }
}