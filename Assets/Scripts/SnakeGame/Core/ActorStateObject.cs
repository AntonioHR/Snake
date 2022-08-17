using System;

namespace SnakeGame
{
    public abstract class ActorStateObject : ICloneable
    {
        protected ActorStateObject(Actor owner)
        {
            this.owner = owner;
        }

        public Actor owner { get; private set; }
        public abstract object Clone();
    }
}