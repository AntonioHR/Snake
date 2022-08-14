using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace SnakeGame
{
    public class SnakeGameMatch : MonoBehaviour
    {
        #region Nested Classes
        [Serializable]
        public class Setup
        {
            public Vector2Int size = new Vector2Int(15, 10);
            public bool closedByWalls = true;

            public FoodSpawnerActor.Setup[] foodSpawners;
            public PlayerActor.Setup[] playersSetup { get; set; }
            //public SnakeSpa playerSnakeSpawners;
        }
        #endregion

        //Events
        public event Action<FoodPiece> FoodEaten;

        //State
        public List<Actor> actors;
        public Board board;

        //References
        private Setup setup;
        private Transform actorsParent;
        private bool running;


        #region Unity Messages
        private void Awake()
        {
            actorsParent = transform.SpawnChild("[ACTORS]");
            actors = new List<Actor>();
        }
        public void Update()
        {
            if(running)
                UpdateActors();
        }

        #endregion



        public void Begin()
        {
            running = true;
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