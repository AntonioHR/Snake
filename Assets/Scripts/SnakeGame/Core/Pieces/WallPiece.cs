namespace SnakeGame
{
    public class WallPiece : Piece
    {


        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
