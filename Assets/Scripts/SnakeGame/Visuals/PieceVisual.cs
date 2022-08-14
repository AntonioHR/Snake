using JammerTools.Common.Grids;
using UnityEngine;

namespace SnakeGame
{
    public abstract class PieceVisual : MonoBehaviour
    {
        public SpriteRenderer borderSprite;
        public SpriteRenderer fillSprite;


        private BoardVisuals board;
        private Piece piece;

        public void Initialize(BoardVisuals board, Piece piece)
        {
            this.board = board;
            this.piece = piece;
            SetPosition(piece.position);

            borderSprite.color = GetBorderColor();
            fillSprite.color = GetFillColor();
        }

        public void SetPosition(Vector2 position)
        {
            transform.position = board.grid.GridToWorld(position, WorldGrid.PlaceMode.TileCenter);
        }


        protected abstract Color GetBorderColor();
        protected abstract Color GetFillColor();
    }

}