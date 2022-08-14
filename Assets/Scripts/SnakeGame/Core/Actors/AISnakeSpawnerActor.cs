using System;

namespace SnakeGame
{
    public class AISnakeSpawnerActor : Actor<AISnakeSpawnerActor.Setup>
    {
        [Serializable]
        public class Setup
        {
            public SnakeActor.Setup snakeSetup;
        }

        public override void OnMatchStart()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnInitialize()
        {
            throw new System.NotImplementedException();
        }

    }

}
