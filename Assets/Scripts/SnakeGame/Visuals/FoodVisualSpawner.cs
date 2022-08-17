using System;
using UnityEngine;

namespace SnakeGame
{
    public class FoodVisualSpawner : MonoBehaviour
    {
        private BoardVisuals boardVisuals;
        private FoodSpawnerActor actor;

        private FoodPieceVisual foodPiece;

        public void Initialize(BoardVisuals boardVisuals, FoodSpawnerActor actor)
        {
            this.boardVisuals = boardVisuals;
            this.actor = actor;

            actor.FoodConsumed += OnFoodConsumedOnBoard;
            actor.FoodSpawned += OnFoodSpawnedOnBoard;
            actor.FoodReset += OnFoodResetOnBoard;
            if(actor.currentFood != null)
                SpawnFood();
        }

        private void OnFoodResetOnBoard()
        {
            DespawnFood();
            if(actor.currentFood != null)
            {
                SpawnFood();
            }
        }

        private void OnFoodSpawnedOnBoard()
        {
            SpawnFood();
        }
        private void OnFoodConsumedOnBoard()
        {
            DespawnFood();
        }

        private void SpawnFood()
        {
            foodPiece = Instantiate(boardVisuals.foodPiecePrefab, transform);
            foodPiece.Initialize(boardVisuals, actor.currentFood);
        }

        private void DespawnFood()
        {
            Destroy(foodPiece.gameObject);
            foodPiece = null;
        }
    }
}