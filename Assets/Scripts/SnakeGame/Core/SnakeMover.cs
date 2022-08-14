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

            Board boardSnapshot = null;
            
            if(CheckCouldEatTimePowerup())
                boardSnapshot = match.board.GetSnapshot();

            foreach (var snake in movementQueue)
            {
                ExecuteMovementOrEat(snake);
            }

            bool anyHit = false;
            foreach (var snake in movementQueue)
            {
                if(CheckHit(snake))
                {
                    anyHit = true;
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

            if(pieces.Any(p=>p != head))
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
            Vector2Int targetPos = snake.head.position + snake.moveDirection.ToVector();
            targetPos = match.board.WrapPosition(targetPos);

            Piece target = match.board.GetTopPiece(targetPos);
            if (target is FoodPiece)
            {
                ExecuteEat(snake, target as FoodPiece);
            } else
            {
                targetPos = ExecuteMovement(snake, targetPos);

            }
        }
        private Piece GetTargetPieceFor(SnakeActor snake)
        {
            Vector2Int targetPos = snake.head.position + snake.moveDirection.ToVector();
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