using JammerTools.Common;
using JammerTools.StateMachines;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace SnakeGame
{
    public partial class SnakeGameMatch 
    {
        public class StateMachine : StateMachine<SnakeGameMatch, State>
        {
            public override State DefaultState => new PreGameState();

        }

        public abstract class State : State<SnakeGameMatch, State>
        {
            public virtual void Update() { }

            public virtual void OnGameStart()
            {
                throw new InvalidOperationException();
            }

            public virtual void OnAfterPlayerDeath()
            {
                throw new InvalidOperationException();
            }

            public virtual void OnRewindTrigger(Board targetBoard)
            {
                throw new InvalidOperationException();
            }
        }

        public class PreGameState : State
        {
            public override void OnGameStart()
            {
                ExitTo(new BoardActiveState());
            }
        }

        public class BoardActiveState : State
        {
            protected override void Begin()
            {
            }
            public override void Update()
            {
                Context.UpdateActors();
                Context.snakeMover.RunMovements();

                foreach (var foodSpawner in Context.GetActors<FoodSpawnerActor>())
                {
                    foodSpawner.CheckFoodSpawns();
                }
            }
            public override void OnAfterPlayerDeath()
            {
                int alivePlayers = Context.GetActors<PlayerActor>().Where(p => p.snake.IsAlive).Count();
                if(alivePlayers <= 0)
                {
                    ExitTo(new GameEndedState());
                }
            }
            public override void OnRewindTrigger(Board targetBoard)
            {
                ExitTo(new RewindState(targetBoard));
            }
        }

        public class RewindState : State
        {
            Board target;
            private Timer timer;

            public RewindState(Board target)
            {
                this.target = target;
            }
            protected override void Begin()
            {
                Context.RewindStarted?.Invoke();
                timer = Timer.CreateAndStart();
            }
            public override void Update()
            {
                if(timer.ElapsedSeconds > Context.setup.rewindWait)
                {
                    Context.ResetToBoard(target);
                    ExitTo(new BoardActiveState());
                }
            }
            protected override void End()
            {
                Context.RewindFinished?.Invoke();
            }
        }

        private void ResetToBoard(Board target)
        {
            this.board = target;
            foreach (var actor in actors)
            {
                actor.OnBoardReset();
            }
        }

        public class GameEndedState : State
        {
            protected override void Begin()
            {
                Context.GameOver?.Invoke();
            }
        }

    }

}