using UnityEngine;

namespace SnakeGame
{
    [CreateAssetMenu(menuName = "SnakeGame/Blocks/Ram")]
    public class BatteringRamBlockAsset : BlockAsset
    {
        public BlockAsset consumedVersion;
    }
}