namespace SnakeGame
{
    public interface ISnakeOwner
    {
        public SnakeActor snake { get; }

        void OnSnakeDead();
    }

}
