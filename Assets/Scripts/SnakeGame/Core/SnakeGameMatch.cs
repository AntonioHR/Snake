using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace SnakeGame
{
    public partial class SnakeGameMatch : MonoBehaviour
    {
        #region Nested Classes
        [Serializable]
        public class Setup
        {
            public Vector2Int size = new Vector2Int(15, 10);
            public bool closedByWalls = true;

            public FoodSpawnerActor.Setup[] foodSpawners { get; set; }
            public PlayerActor.Setup[] playersSetup { get; set; }
            public AISnakeActor.Setup[] aiSnakes { get; set; }
        }
        #endregion

        //Events
        public event Action GameOver;
        public event Action<FoodPiece> FoodEaten;

        //State
        public List<Actor> actors;
        private StateMachine stateMachine;
        public SnakeMover snakeMover;
        public Board board;

        //References
        private Setup setup;
        private Transform actorsParent;


        #region Unity Messages
        private void Awake()
        {
            actorsParent = transform.SpawnChild("[ACTORS]");
            actors = new List<Actor>();
            snakeMover = new SnakeMover(this);
            stateMachine = new StateMachine();
            stateMachine.Begin(this);
        }
        public void Update()
        {
            stateMachine.CurrentState.Update();
        }

        #endregion


        public void OnHitsHappened()
        {
            bool playerDead = false;
            foreach (var snake in GetActors<SnakeActor>())
            {
                if(snake.WasHit)
                {
                    if(snake.owner is PlayerActor)
                    {
                        playerDead = true;
                    }

                    snake.OnDead();
                }
            }
            if(playerDead)
                stateMachine.CurrentState.OnAfterPlayerDeath();
        }

        public void OnPieceEaten(FoodPiece foodPiece)
        {
            FoodEaten?.Invoke(foodPiece);
        }

        public void Begin()
        {
            stateMachine.CurrentState.OnGameStart();
            foreach (var actor in actors)
            {
                actor.OnMatchStart();
            }
        }

        public T[] GetActors<T>() where T: Actor
        {
            return actors.OfType<T>().ToArray();
        }

        public TActor SpawnActor<TActor, TSetup>(TSetup s) where TActor : Actor<TSetup>
        {
            var t = transform.SpawnChild($"Actor - {typeof(TActor).Name}");
            TActor result = t.gameObject.AddComponent<TActor>();
            result.Initialize(this, s);
            actors.Add(result);
            return result;
        }

        private void UpdateActors()
        {
            foreach (var actor in actors)
            {
                actor.Tick();
            }
        }

    }
}