using UnityEngine;

namespace SnakeGame
{
    public class FoodSpawnerState : ActorStateObject
    {
        public FoodSpawnerState(Actor owner) : base(owner)
        {
        }

        public Vector2Int? foodPosition;
        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}