using System;
using UnityEngine;

namespace SnakeGame
{
    public abstract class Actor : MonoBehaviour
    {
        public abstract void OnMatchStart();
        public abstract void OnBoardReset();
        public virtual void Tick() { }


        ///Should not implement Update on Actors, use Tick() instead
        protected void Update() { }

    }

    public abstract class Actor<T> : Actor
    {
        public SnakeGameMatch match { get; private set; }
        protected T setup { get; private set; }

        public void Initialize(SnakeGameMatch match, T setup)
        {
            this.match = match;
            this.setup = setup;
            OnInitialize();
        }

        protected abstract void OnInitialize();
    }
}
