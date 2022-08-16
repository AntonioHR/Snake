using System;
using System.Collections;
using UnityEngine;

namespace SnakeGame
{
    public class MatchStarter : MonoBehaviour
    {
        public GameConfigs configs;
        public MatchBuilder matchBuilder;
        public SnakePregameUI pregameUI;
        private SnakeGameMatch match;

        public SnakeGameMatch.Setup setup;
        public AISnakeActor.Setup[] aiSnakes;
        public bool autoStart;

        private void Start()
        {
            pregameUI.BeginSelection(configs, (result) =>
            {
                setup.size = configs.boardSize;
                setup.playersSetup = result;
                setup.aiSnakes = GetSnakes(result.Length);
                setup.foodSpawners = GetFoodSpawners(result.Length);
                match = matchBuilder.Build(setup);
                match.Begin();
            });
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