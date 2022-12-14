using UnityEngine;

namespace SnakeGame
{
    public class GenericPieceVisual : PieceVisual<Piece>
    {
        public Color borderColor = Color.white;
        public Color fillColor = Color.white;

        protected override Color GetBorderColor() => borderColor;
        protected override Color GetFillColor() => fillColor;
    }
}