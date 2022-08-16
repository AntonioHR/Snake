using UnityEngine;

namespace SnakeGame
{
    [CreateAssetMenu(menuName = "SnakeGame/SnakeType")]
    public class SnakeTypeAsset : ScriptableObject
    {
        public BlockAsset[] startingBlocks;
    }
}