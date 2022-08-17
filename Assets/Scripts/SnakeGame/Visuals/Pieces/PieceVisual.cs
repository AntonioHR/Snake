using DG.Tweening;
using JammerTools.Common.Grids;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SnakeGame
{
    public abstract class PieceVisual<T> : MonoBehaviour where T: Piece
    {
        public SpriteRenderer borderSprite;
        public SpriteRenderer fillSprite;


        protected SnakeGameMatch match => board.match;
        protected BoardVisuals board { get; private set; }

        protected SpriteRenderer[] allSprites;

        protected T piece { get; private set; }

        public void Initialize(BoardVisuals board, T piece)
        {
            this.board = board;
            allSprites = GetComponentsInChildren<SpriteRenderer>();
            SetPiece(piece);
            OnInitialize();
            OnPieceChanged();
        }

        public void SetPiece(T piece)
        {
            this.piece = piece;
            SetPosition(piece.position);
            RefreshVisuals();
        }

        public void RefreshVisuals()
        {
            borderSprite.color = GetBorderColor();
            fillSprite.color = GetFillColor();
            OnRefreshVisuals();
        }

        protected virtual void OnRefreshVisuals() { }

        protected virtual void OnInitialize() { }

        protected virtual void OnPieceChanged() { }

        public void SetPosition(Vector2 position)
        {
            transform.position = board.grid.GridToWorld(position, WorldGrid.PlaceMode.TileCenter);
        }


        public void RefreshPosition()
        {
            SetPosition(piece.position);
        }

        protected const float fadeTime = 1.5f;
        public void FadeOutAndDestroy()
        {
            Sequence seq = DOTween.Sequence();
            foreach (var sprite in allSprites)
            {
                seq.Join(sprite.DOFade(0, fadeTime));
            }
            seq.OnComplete(() => Destroy(gameObject));
        }

        protected abstract Color GetBorderColor();
        protected abstract Color GetFillColor();
    }

}