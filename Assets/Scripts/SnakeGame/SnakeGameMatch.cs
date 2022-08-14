using Common;
using System;
using System.Collections.Generic;
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
            public PlayerActor.Setup[] playersSetup;
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


        #region Unity Messages
        private void Awake()
        {
            actorsParent = transform.SpawnChild("[ACTORS]");
        }
        public void Update()
        {
            UpdateActors();
        }

        #endregion



        public void Begin()
        {
            throw new NotImplementedException();
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