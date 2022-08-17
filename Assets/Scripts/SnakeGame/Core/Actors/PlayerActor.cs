using System;
using UnityEngine;

namespace SnakeGame
{
    public class PlayerActor : Actor<PlayerActor.Setup>, ISnakeOwner
    {
        [Serializable]
        public class Setup
        {
            public SnakeActor.Setup snakeSetup;
            public KeyCode leftKey { get; set; }
            public KeyCode rightKey { get; set; }
        }

        public SnakeActor snake { get; private set; }

        public override void OnMatchStart()
        {
        }
        public override void OnBoardReset()
        {
        }

        protected override void OnInitialize()
        {
            snake = match.SpawnActor<SnakeActor, SnakeActor.Setup>(setup.snakeSetup);
            snake.owner = this;
        }

        public override void Tick()
        {
            if(Input.GetKeyDown(setup.leftKey))
            {
                snake.FlipLeft();
            }


            if (Input.GetKeyDown(setup.rightKey))
            {
                snake.FlipRight();
            }
        }

        public void OnSnakeDead()
        {
        }

    }

}
