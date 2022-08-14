using System;
using UnityEngine;

namespace SnakeGame
{
    public class SnakeUI : MonoBehaviour
    {
        public Transform gameOverScreen;
        private SnakeGameMatch match;

        public void Initialize(SnakeGameMatch match)
        {
            this.match = match;
            match.GameOver += OnGameOver;
        }

        private void OnGameOver()
        {
            gameOverScreen.gameObject.SetActive(true);
        }
    }
}