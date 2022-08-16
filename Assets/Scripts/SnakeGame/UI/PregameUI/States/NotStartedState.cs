namespace SnakeGame
{
    public partial class SnakePregameUI
    {
        private class NotStartedState : State
        {
            public override void BeginSelection()
            {
                ExitTo(new ChoosingKeysState());
            }
        }

    }
}