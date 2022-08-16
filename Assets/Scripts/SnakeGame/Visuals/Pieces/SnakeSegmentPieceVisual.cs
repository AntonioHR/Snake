using Common;
using DG.Tweening;
using System;
using UnityEngine;

namespace SnakeGame
{
    public class SnakeSegmentPieceVisual : PieceVisual<SnakeSegmentPiece>
    {

        protected override Color GetBorderColor() => piece.snake.Color.WithAlpha(GetAlpha());


        protected override Color GetFillColor() => piece.block.color.WithAlpha(GetAlpha());

        private float GetAlpha()
        {
            return piece.snake.IsGhostly ? .5f : 1.0f;
        }
    }
}