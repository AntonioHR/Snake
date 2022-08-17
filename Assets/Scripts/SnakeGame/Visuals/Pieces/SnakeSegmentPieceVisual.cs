using Common;
using DG.Tweening;
using System;
using UnityEngine;

namespace SnakeGame
{
    public class SnakeSegmentPieceVisual : PieceVisual<SnakeSegmentPiece>
    {
        public GameObject eye;

        protected override Color GetBorderColor() => piece.snake.Color.WithAlpha(GetAlpha());


        protected override Color GetFillColor() => piece.block.color.WithAlpha(GetAlpha());
        protected override void OnRefreshVisuals()
        {
            eye.gameObject.SetActive(piece.isHead);
            float angle = Vector2.SignedAngle(Vector2.up, piece.snake.lookVector);
            eye.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
        }

        private float GetAlpha()
        {
            return piece.snake.IsGhostly ? .5f : 1.0f;
        }
    }
}