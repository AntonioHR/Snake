using UnityEngine;

namespace SnakeGame
{
    [CreateAssetMenu(menuName = "SnakeGame/SnakeType")]
    public class SnakeTypeAsset : ScriptableObject
    {
        public string displayName;
        public BlockAsset[] startingBlocks;
    }
}