using Common;
using JammerTools.Common.Grids;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace SnakeGame
{
    public class BoardVisuals : MonoBehaviour
    {
        public SnakeGameMatch match { get; private set; }

        public WorldGrid grid;
        private Transform piecesParent;
        private Transform snakesParent;
        private Transform foodParent;
        public GenericPieceVisual wallPiecePrefab;
        public FoodPieceVisual foodPiecePrefab;

        public SnakeVisuals snakeVisualsPrefab;

        public Camera gameCamera;
        private FoodVisualSpawner[] foodVisualSpawners;
        private SnakeVisuals[] snakeVisuals;

        public void Initialize(SnakeGameMatch match)
        {
            this.match = match;
            piecesParent = transform.SpawnChild("[PIECES]");
            snakesParent = transform.SpawnChild("[SNAKES]");
            foodParent = transform.SpawnChild("[FOOD]");
            SetupGrid();
            CenterCamera();
            SpawnWalls();
            SpawnSnakes();
            SetupFoodSpawns();
        }

        private void SetupFoodSpawns()
        {
            match.GetActors<FoodSpawnerActor>()
                .Select(s => CreateFoodSpawner(s))
                .ToArray();
        }


        private void SpawnSnakes()
        {
            snakeVisuals = match.GetActors<SnakeActor>()
                .Select(s => SpawnSnake(s))
                .ToArray();
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
            for (int i = 0; i < match.board.Size.x; i++)
            {
                for (int j = 0; j < match.board.Size.y; j++)
                {
                    Piece piece = match.board.GetTopPiece(i, j);
                    if(piece is WallPiece)
                    {
                        SpawnWallPiece(piece as WallPiece);
                    }
                }
            }
        }

        private void SpawnWallPiece(Piece piece)
        {
            var result = Instantiate(wallPiecePrefab, piecesParent);
            result.Initialize(this, piece);
        }


        private SnakeVisuals SpawnSnake(SnakeActor snakeActor)
        {
            SnakeVisuals result = Instantiate(snakeVisualsPrefab, snakesParent);
            result.Initialize(this, snakeActor);
            return result;
        }
        private FoodVisualSpawner CreateFoodSpawner(FoodSpawnerActor actor)
        {
            var result = foodParent.SpawnChild<FoodVisualSpawner>("Food Spawn");
            result.Initialize(this, actor);
            return result;
        }
    }
}