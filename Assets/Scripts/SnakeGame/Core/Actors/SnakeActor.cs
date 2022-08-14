using Common;
using System;
using UnityEngine;

namespace SnakeGame
{
    public class SnakeActor : Actor<SnakeActor.Setup>
    {
        [Serializable]
        public class Setup
        {
            public Direction startDirection;
        }

        public Direction moveDirection;


        public override void OnMatchStart()
        {
            //throw new System.NotImplementedException();
        }

        protected override void OnInitialize()
        {
            moveDirection = setup.startDirection;


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
