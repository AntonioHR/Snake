using JammerTools.StateMachines;
using System;
using System.Collections;
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
        }

    }
}