﻿using System;
using UnityEngine;
using Common;

namespace SnakeGame
{
    public class MatchBuilder : MonoBehaviour
    {
        public Transform matchTarget;
        public BoardVisuals visuals;


        private SnakeGameMatch match;
        private SnakeGameMatch.Setup setup;
        private Transform actorsParent;

        public SnakeGameMatch Build(SnakeGameMatch.Setup setup)
        {
            match = matchTarget.gameObject.AddComponent<SnakeGameMatch>();
            this.setup = setup;
            InitializeMatch();
            InitializeVisuals();
            return match;
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
        }

        private void InitializeBoard()
        {
            match.board = Board.BuildWithSize(setup.size, setup.closedByWalls);
        }
    }
}