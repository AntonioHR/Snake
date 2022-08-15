using System;

namespace SnakeGame
{
    public class FoodPiece : Piece
    {
        public BlockAsset blockType;
        public override bool IsHazard => false;

        public FoodSpawnerActor spawner;

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
