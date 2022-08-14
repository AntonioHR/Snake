using Common;
using JammerTools.Common.Grids;
using System;
using System.Collections;
using UnityEngine;

namespace SnakeGame
{
    public class BoardVisuals : MonoBehaviour
    {
        public SnakeGameMatch match { get; private set; }

        public WorldGrid grid;
        private Transform piecesParent;
        public GenericPieceVisual wallPiecePrefab;
        public Camera gameCamera;

        public void Initialize(SnakeGameMatch match)
        {
            this.match = match;
            piecesParent = transform.SpawnChild("[PIECES]");
            SetupGrid();
            SpawnWalls();
            CenterCamera();
        }

        private void SetupGrid()
        {
            Vector2Int size = match.board.Size;
            grid.topLeft = new Vector2Int(0, size.y);
            grid.bottomRight = new Vector2Int(size.x, 0);
        }

        private void CenterCamera()
        {
            float z = gameCamera.transform.position.z;
            Vector2 center = match.board.Size;
            center *= 0.5f;

            gameCamera.transform.position = new Vector3(center.x, center.y, z);
        }

        public void SpawnWalls()
        {
            for (int i = 0; i < match.board.pieces.GetLength(0); i++)
            {
                for (int j = 0; j < match.board.pieces.GetLength(1); j++)
                {
                    var piece = match.board.pieces[i, j];
                    if(piece is WallPiece)
                    {
                        SpawnWallPiece(piece);
                    }
                }
            }
        }

        private void SpawnWallPiece(Piece piece)
        {
            var result = Instantiate(wallPiecePrefab, piecesParent);
            result.Initialize(this, piece);
        }
    }
}