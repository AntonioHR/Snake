using UnityEngine;

namespace SnakeGame
{
    [CreateAssetMenu(menuName ="SnakeGame/Configs/Game")]
    public class GameConfigs : ScriptableObject
    {
        public Color[] playerColors;
        public int maxPlayers = 5;
        public Vector2Int boardSize = new Vector2Int(20, 15);
        public Vector2Int enemySpacing = new Vector2Int(-1, -2);
        public Vector2Int playerSpacing = new Vector2Int(1, 2);

        public BlockAsset[] blockOptions;
        public PlayerActor.Setup basePlayerSetup;
        public SnakeTypeAsset[] snakeTypes;
        public SnakeTypeAsset aiSnakeType;
        public AISnakeActor.Setup templateAISnake;
    }
}