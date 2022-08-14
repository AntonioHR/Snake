using System;
using UnityEngine;

namespace SnakeGame
{
    public class FoodSpawnerActor : Actor<FoodSpawnerActor.Setup>
    {
        [Serializable]
        public class Setup
        {
            public BlockAsset[] blockAssetOptions;
        }
        public event Action FoodConsumed;
        public event Action FoodSpawned;

        public FoodPiece currentFood { get; private set; }

        protected override void OnInitialize()
        {
            match.FoodEaten += OnFoodEaten;
        }

        public override void OnMatchStart()
        {
            SpawnNextFood();
        }

        public void CheckFoodSpawns()
        {
            if(currentFood == null)
                SpawnNextFood();
        }
        private void OnFoodEaten(FoodPiece foodPiece)
        {
            if(foodPiece == this.currentFood)
            {
                currentFood = null;
                FoodConsumed?.Invoke();
            }
        }

        private void SpawnNextFood()
        {
            Vector2Int pos = match.board.GetRandomEmptyPosition();
            BlockAsset block = setup.blockAssetOptions[UnityEngine.Random.Range(0, setup.blockAssetOptions.Length)];
            currentFood = new FoodPiece();
            currentFood.blockType = block;
            currentFood.spawner = this;
            match.board.AttachPiece(pos, currentFood);
            FoodSpawned?.Invoke();
        } 
    }
}
