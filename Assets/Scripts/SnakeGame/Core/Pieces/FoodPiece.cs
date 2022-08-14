using System;

namespace SnakeGame
{
    public class FoodPiece : Piece
    {
        public BlockAsset blockType;

        public FoodSpawnerActor spawner;

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
