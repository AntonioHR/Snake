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
                SnakeSegmentPieceVisual pieceVisual = SpawnPiece();
                pieceVisual.Initialize(board, piece);
            }

            snake.Moved += OnMoved;
            snake.PiecesChanged += OnPiecesChanged;
            snake.SinglePieceChanged += OnSinglePieceChanged;
        }

        private void OnPiecesChanged()
        {
            while(pieces.Count > snake.Length)
            {
                RemoveLastPiece();
            }

            for (int i = 0; i < pieces.Count; i++)
            {
                pieces[i].SetPiece(snake[i]);
            }

            while (pieces.Count < snake.Length)
            {
                SpawnPiece();
            }

        }

        private void OnSinglePieceChanged(int i)
        {
            pieces[i].SetPiece(snake[i]);
        }

        private SnakeSegmentPieceVisual SpawnPiece()
        {
            var pieceVisual = Instantiate(piecePrefab, transform);
            int index = pieces.Count;
            pieces.Add(pieceVisual);
            pieceVisual.Initialize(board, snake[index]);
            return pieceVisual;
        }
        private void RemoveLastPiece()
        {
            var piece = pieces[pieces.Count - 1];
            pieces.RemoveAt(pieces.Count - 1);
            Destroy(piece.gameObject);
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