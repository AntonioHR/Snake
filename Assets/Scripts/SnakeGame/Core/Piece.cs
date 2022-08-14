using System;
using UnityEngine;

namespace SnakeGame
{
    public abstract class Piece : ICloneable
    {
        public Vector2Int position;

        public abstract object Clone();
    }

}
