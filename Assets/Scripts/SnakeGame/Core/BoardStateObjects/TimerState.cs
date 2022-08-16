
namespace SnakeGame
{
    public class TimerState : BoardStateObject
    {
        public TimerActor owner;
        public bool isOver => remaining < 0;
        public float remaining;

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}