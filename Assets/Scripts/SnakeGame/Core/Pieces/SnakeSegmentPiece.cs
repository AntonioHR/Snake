namespace SnakeGame
{
    public class SnakeSegmentPiece : Piece
    {
        public BlockAsset block;

        public SnakeActor snake;
        public int snakeIndex;
        public override bool IsHazard => !snake.IsGhostly;
        public bool isHead => snakeIndex == 0;
        public bool isTail => snakeIndex == snake.Length;
        public SnakeSegmentPiece next => isTail? null : snake[snakeIndex + 1];
        public SnakeSegmentPiece previous => isHead ? null : snake[snakeIndex - 1];



        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
