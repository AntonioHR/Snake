using JammerTools.Common.Grids;
using System;
using UnityEngine;

namespace SnakeGame
{
    public abstract class PieceVisual<T> : MonoBehaviour where T: Piece
    {
        public SpriteRenderer borderSprite;
        public SpriteRenderer fillSprite;

        protected SnakeGameMatch match => board.match;
        protected BoardVisuals board { get; private set; }
        protected T piece { get; private set; }

        public void Initialize(BoardVisuals board, T piece)
        {
            this.board = board;
            this.piece = piece;
            SetPosition(piece.position);

            borderSprite.color = GetBorderColor();
            fillSprite.color = GetFillColor();
            OnInitialize();
        }

        protected virtual void OnInitialize() { }

        public void SetPosition(Vector2 position)
        {
            transform.position = board.grid.GridToWorld(position, WorldGrid.PlaceMode.TileCenter);
        }


        protected abstract Color GetBorderColor();
        protected abstract Color GetFillColor();
    }

}