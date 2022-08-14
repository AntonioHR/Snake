using System;
using UnityEngine;

namespace SnakeGame
{
    public class SnakeSegmentPieceVisual : PieceVisual<SnakeSegmentPiece>
    {

        protected override Color GetBorderColor() => piece.snake.Color;
        protected override Color GetFillColor() => piece.block.color;
    }
}