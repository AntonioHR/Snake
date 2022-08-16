namespace SnakeGame
{
    public partial class SnakePregameUI
    {
        private class FinishState : State
        {
            protected override void Begin()
            {
                Context.FinishCallback(Context.players.ToArray());
                Context.FinishCallback = null;
                Context.gameObject.SetActive(false);
            }
        }

    }
}