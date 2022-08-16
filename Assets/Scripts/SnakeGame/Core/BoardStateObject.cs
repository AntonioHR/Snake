using System;

namespace SnakeGame
{
    public abstract class BoardStateObject : ICloneable
    {
        public abstract object Clone();
    }
}