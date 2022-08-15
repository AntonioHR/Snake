using System;
using UnityEngine;

namespace SnakeGame
{
    public abstract class Piece : ICloneable
    {
        public Vector2Int position;

        public virtual bool IsHazard => true;

        public abstract object Clone();
    }

}
