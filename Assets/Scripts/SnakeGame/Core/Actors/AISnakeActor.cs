using Common;
using System;
using System.Linq;
using UnityEngine;

namespace SnakeGame
{
    public class AISnakeActor : Actor<AISnakeActor.Setup>, ISnakeOwner
    {
        public SnakeActor snake { get; private set; }

        private TimerActor respawnTimer;
        private RefreshTimer thinkTimer;
        private FoodSpawnerActor foodSpawner;

        [Serializable]
        public class Setup
        {
            public SnakeActor.Setup snakeSetup;
            public TimerActor.Setup respawnTimer;
            public int foodSpawnerIndex { get; set; }
            public float thinkInterval = .1f;
            public bool evadeEnabled = true;
        }

        public override void OnMatchStart()
        {
            thinkTimer = RefreshTimer.CreateAndStart(setup.thinkInterval);
            foodSpawner = match.GetActors<FoodSpawnerActor>()[setup.foodSpawnerIndex];
        }

        public override void OnBoardReset()
        {
            thinkTimer = RefreshTimer.CreateAndStart(setup.thinkInterval);
        }

        protected override void OnInitialize()
        {
            respawnTimer = match.SpawnActor<TimerActor, TimerActor.Setup>(setup.respawnTimer);
            snake = match.SpawnActor<SnakeActor, SnakeActor.Setup>(setup.snakeSetup);
            snake.owner = this;
        }
        public override void Tick()
        {
            if(thinkTimer.Check())
            {
                CheckPath();
            }
            if(!snake.IsAlive && respawnTimer.IsOver)
            {
                Vector2Int targetPiece = match.board.GetRandomEmptyPosition();
                snake.RespawnAt(targetPiece);
            }
        }


        public void OnSnakeDead()
        {
            respawnTimer.Restart();
        }

        private void CheckPath()
        {

            snake.SetMoveDirection(FindBestDirection());

        }
        #region AI Helpers

        private Vector2Int FindBestDirection()
        {
            Vector2Int? result = CheckIfFacingFood();
            if (result != null)
                return result.Value;
            return GetFreeRoamDirection();
        }
        private Vector2Int GetFreeRoamDirection()
        {
            Piece facedPiece = snake.GetFacingPiece();
            bool isFacingHazard = CheckHazard(facedPiece);
            if (isFacingHazard && setup.evadeEnabled)
            {
                var directionOptions = snake
                    .GetDirectionOptions()
                    .OrderBy(o => UnityEngine.Random.value);

                foreach (Vector2Int direction in directionOptions)
                {
                    Vector2Int targetPos = snake.head.position + direction;
                    targetPos = match.board.WrapPosition(targetPos);
                    Piece piece = match.board.GetTopPiece(targetPos);
                    if (!CheckHazard(piece))
                    {
                        return direction;
                    }
                }
            }

            return snake.moveDirection;
        }
        private Vector2Int? CheckIfFacingFood()
        {
            Piece targetedFood = foodSpawner.currentFood;
            if (targetedFood != null)
            {
                Vector2Int toFood = targetedFood.position - snake.head.position;

                //Aligned with food
                if (toFood.x == 0 || toFood.y == 0)
                {
                    Vector2Int foodDirectionAxis = new Vector2Int(toFood.x == 0 ? 0 : 1, toFood.y == 0 ? 0 : 1);

                    var validDirections = snake.GetDirectionOptions()
                            //Filter directions aligned with food
                            .Where(d =>
                            {
                                return Mathf.Abs(Vector2.Dot(d, foodDirectionAxis)) == 1;
                            })
                            ////Sort by which is closer + a random bump tiebreaker
                            .OrderBy(d =>
                            {
                                int distance = GetAlignedMoveDistance(snake.head.position, targetedFood.position, d);
                                float randomBump = UnityEngine.Random.Range(0, .1f);
                                return randomBump + distance;
                            });
                    foreach (var d in validDirections)
                    {
                        Piece piece = GetPiece(snake.head.position + d);
                        if (!CheckHazard(piece))
                        {
                            return d;
                        }
                    }
                }
            }
            return null;
        }
        private int GetAlignedMoveDistance(Vector2Int from, Vector2Int to, Vector2Int direction)
        {
            //Project both onto the direction axis
            int boardSize = Project(match.board.Size, direction);
            int p1 = Project(from, direction);
            int p2 = Project(to, direction);
            int d = direction.x + direction.y;

            if (d < 0)
            {
                if(p2 > p1)
                {
                    p1 = p1 + boardSize;
                }
                return p1 - p2;
            } else
            {
                if (p2 < p1)
                {
                    p2 = p2 + boardSize;
                }
                return p2 - p1;
            }
        }
        private int Project(Vector2Int v, Vector2Int direction)
        {
            return v.x * Mathf.Abs(direction.x) + v.y * Mathf.Abs(direction.y);
        }
        private Piece GetPiece(Vector2Int pos)
        {
            pos = match.board.WrapPosition(pos);
            return match.board.GetTopPiece(pos);
        }
        private bool CheckHazard(Piece piece)
        {
            return piece != null && piece.IsHazard && piece != snake.tail;
        }
        #endregion

    }

}
