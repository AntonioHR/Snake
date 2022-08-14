using System;
using UnityEngine;

namespace SnakeGame
{
    public class FoodPieceVisual : PieceVisual<FoodPiece>
    {
        public Color outlineColor;

        protected override Color GetBorderColor() => outlineColor;
        protected override Color GetFillColor() => piece.blockType.color;
    }
}