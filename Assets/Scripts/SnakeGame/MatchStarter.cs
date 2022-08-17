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

        private void Start()
        {
            pregameUI.BeginSelection(configs, (players) =>
            {
                match = matchBuilder.Build(configs, players);
                match.Begin();
            });
        }
    }
}