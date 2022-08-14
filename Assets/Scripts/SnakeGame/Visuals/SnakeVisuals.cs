using System;
using System.Collections.Generic;
using UnityEngine;

namespace SnakeGame
{
    public class SnakeVisuals : MonoBehaviour
    {
        public SnakeSegmentPieceVisual piecePrefab;
        private List<SnakeSegmentPieceVisual> pieces = new List<SnakeSegmentPieceVisual>();
        private BoardVisuals board;
        private SnakeActor snake;

        public void Initialize(BoardVisuals board, SnakeActor snake)
        {
            this.board = board;
            this.snake = snake;

            foreach (SnakeSegmentPiece piece in snake.GetPieces())
            {
                var pieceVisual = Instantiate(piecePrefab, transform);
                pieceVisual.Initialize(board, piece);
                pieces.Add(pieceVisual);
            }

            snake.Moved += OnMoved;
        }

        private void OnMoved()
        {
            foreach (var piece in pieces)
            {
                piece.RefreshPosition();
            }
        }
    }
}