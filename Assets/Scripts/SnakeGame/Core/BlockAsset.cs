using UnityEngine;

namespace SnakeGame
{
    public abstract class BlockAsset : ScriptableObject
    {
        public Color color;
        public float speedModifier = -.1f;
    }
}