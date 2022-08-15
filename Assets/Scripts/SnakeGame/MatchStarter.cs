using System;
using System.Collections;
using UnityEngine;

namespace SnakeGame
{
    public class MatchStarter : MonoBehaviour
    {
        public MatchBuilder matchBuilder;
        private SnakeGameMatch match;

        public SnakeGameMatch.Setup setup;
        public PlayerActor.Setup[] players;
        public AISnakeSpawnerActor.Setup[] aiSnakes;
        public bool autoStart;


        private void Start()
        {
            if(autoStart)
            {
                setup.playersSetup = players;
                setup.aiSnakes = aiSnakes;
                match = matchBuilder.Build(setup);
                match.Begin();
            }
            
        }
    }
}