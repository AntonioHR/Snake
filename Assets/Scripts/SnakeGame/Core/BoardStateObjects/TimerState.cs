namespace SnakeGame
{
    public class TimerState : ActorStateObject
    {
        public bool isOver => remaining < 0;
        public float remaining;

        public TimerState(Actor owner) : base(owner)
        {
        }

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}