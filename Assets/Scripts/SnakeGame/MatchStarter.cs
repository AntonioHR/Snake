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
        public bool autoStart;


        private void Start()
        {
            if(autoStart)
            {
                setup.playersSetup = players;
                match = matchBuilder.Build(setup);
                match.Begin();
            }
            
        }
    }
}