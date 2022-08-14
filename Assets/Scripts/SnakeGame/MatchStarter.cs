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
        public bool autoStart;


        private void Start()
        {
            if(autoStart)
            {
                match = matchBuilder.Build(setup);
                match.Begin();
            }
            
        }
    }
}