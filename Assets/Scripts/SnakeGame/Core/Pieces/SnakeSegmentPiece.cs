namespace SnakeGame
{
    public class SnakeSegmentPiece : Piece
    {
        public BlockAsset block;

        public SnakeActor snake;
        public int snakeIndex;
        public bool isHead => snakeIndex == 0;
        public bool isTail => snakeIndex == snake.Length;
        public SnakeSegmentPiece next => isTail? snake[snakeIndex + 1] : null;
        public SnakeSegmentPiece previous => isHead ? snake[snakeIndex - 1] : null;
    }
}
