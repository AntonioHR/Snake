using System;
using UnityEngine;

namespace SnakeGame
{
    public class TimerActor : Actor<TimerActor.Setup>
    {
        private TimerState state;
        public bool IsOver => state.isOver;



        public override void Tick()
        {
            TickTimer();
        }

        public void Restart() => Restart(setup.defaultDuration);
        public void Restart(float duration)
        {
            state.remaining = duration;
        }
        private void TickTimer()
        {
            state.remaining -= Time.deltaTime;
        }
        public override void OnMatchStart()
        {
        }
        public override void OnBoardReset()
        {
            state = match.board.GetStateObjectFor<TimerState>(this);
        }

        protected override void OnInitialize()
        {
            state = new TimerState(this);
            match.board.AddStateObject(state);
        }

        [Serializable]
        public class Setup
        {
            public float defaultDuration = 1;
        }
    }
}