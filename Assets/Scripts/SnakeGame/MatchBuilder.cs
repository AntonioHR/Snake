using System;
using UnityEngine;
using Common;

namespace SnakeGame
{
    public class MatchBuilder : MonoBehaviour
    {
        public Transform matchTarget;
        public BoardVisuals visuals;
        public SnakeUI ui;


        private SnakeGameMatch match;
        private SnakeGameMatch.Setup setup;
        private Transform actorsParent;

        public SnakeGameMatch Build(SnakeGameMatch.Setup setup)
        {
            match = matchTarget.gameObject.AddComponent<SnakeGameMatch>();
            this.setup = setup;
            InitializeMatch();
            InitializeVisuals();
            IntializeUI();
            return match;
        }

        private void IntializeUI()
        {
            ui.Initialize(match);
        }

        private void InitializeVisuals()
        {
            visuals.Initialize(match);
        }
        public void InitializeMatch()
        {
            InitializeBoard();
            InitializeActors();
        }

        private void InitializeActors()
        {
            foreach (PlayerActor.Setup s in setup.playersSetup)
            {
                PlayerActor actor = match.SpawnActor<PlayerActor, PlayerActor.Setup>(s);
            }
            foreach (FoodSpawnerActor.Setup s in setup.foodSpawners)
            {
                FoodSpawnerActor actor = match.SpawnActor<FoodSpawnerActor, FoodSpawnerActor.Setup>(s);
            }
        }

        private void InitializeBoard()
        {
            match.board = Board.BuildWithSize(setup.size, setup.closedByWalls);
        }
    }
}