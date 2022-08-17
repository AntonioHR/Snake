using System.Linq;
using UnityEngine;

namespace SnakeGame
{
    public class SnakeSegmentPiece : Piece
    {
        public BlockAsset block;

        public SnakeActor snake;
        public int snakeIndex;
        public override bool IsHazard
        {
            get
            {
                Debug.Assert(snake.GetPieces().Contains(this), "This check is only valid if this piece is currently used by the actor");
                return !snake.IsGhostly;
            }
        }
        public bool isHead => snakeIndex == 0;



        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
