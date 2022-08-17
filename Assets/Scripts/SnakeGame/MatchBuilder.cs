using System;
using UnityEngine;
using Common;

namespace SnakeGame
{
    public class MatchBuilder : MonoBehaviour
    {
        public SnakeGameMatch.Setup setup;
        public Transform matchTarget;
        public BoardVisuals visuals;
        public SnakeUI ui;
        private GameConfigs configs;
        private SnakeGameMatch match;
        private Transform actorsParent;

        public SnakeGameMatch Build(GameConfigs configs, PlayerActor.Setup[] players)
        {
            this.configs = configs;
            match = matchTarget.gameObject.AddComponent<SnakeGameMatch>();

            setup.size = configs.boardSize;
            setup.playersSetup = players;
            setup.aiSnakes = GetSnakes(players.Length);
            setup.foodSpawners = GetFoodSpawners(players.Length);

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
            match.Initialize(setup);
        }

        private void InitializeActors()
        {
            foreach (PlayerActor.Setup s in setup.playersSetup)
            {
                PlayerActor actor = match.SpawnActor<PlayerActor, PlayerActor.Setup>(s);
            }
            foreach (AISnakeActor.Setup s in setup.aiSnakes)
            {
                AISnakeActor actor = match.SpawnActor<AISnakeActor, AISnakeActor.Setup>(s);
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


        private FoodSpawnerActor.Setup[] GetFoodSpawners(int length)
        {
            var result = new FoodSpawnerActor.Setup[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = new FoodSpawnerActor.Setup()
                {
                    blockAssetOptions = configs.blockOptions
                };
            }
            return result;
        }

        private AISnakeActor.Setup[] GetSnakes(int length)
        {
            AISnakeActor.Setup template = configs.templateAISnake;
            var result = new AISnakeActor.Setup[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = new AISnakeActor.Setup()
                {
                    evadeEnabled = template.evadeEnabled,
                    foodSpawnerIndex = i,
                    respawnTimer = new TimerActor.Setup()
                    {
                        defaultDuration = template.respawnTimer.defaultDuration
                    },
                    snakeSetup = new SnakeActor.Setup()
                    {
                        color = template.snakeSetup.color,
                        configsAsset = template.snakeSetup.configsAsset,
                        snakeTypeAsset = configs.aiSnakeType,
                        startDirection = template.snakeSetup.startDirection,
                        startPosition = template.snakeSetup.startPosition + configs.enemySpacing * i
                    },
                    thinkInterval = template.thinkInterval
                };
            }
            return result;
        }
    }
}