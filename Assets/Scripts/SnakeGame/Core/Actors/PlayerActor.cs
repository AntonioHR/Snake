using System;
using UnityEngine;

namespace SnakeGame
{
    public class PlayerActor : Actor<PlayerActor.Setup>
    {
        [Serializable]
        public class Setup
        {
            public SnakeActor.Setup snakeSetup;
            public KeyCode leftKey;
            public KeyCode rightKey;
        }

        private SnakeActor snake;

        public override void OnMatchStart()
        {
        }

        protected override void OnInitialize()
        {
            snake = match.SpawnActor<SnakeActor, SnakeActor.Setup>(setup.snakeSetup);
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

    }

}
