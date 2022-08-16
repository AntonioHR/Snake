using UnityEngine;

namespace SnakeGame
{
    [CreateAssetMenu(menuName = "SnakeGame/Configs/Snake")]
    public class SnakeConfigsAsset : ScriptableObject
    {
        public float baseSpeed = 1.5f;
        public float minSpeed = .5f;
        public TimerActor.Setup respawnGhostTimer;
    }
}