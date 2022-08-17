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
        public event Action FoodReset;
        public event Action FoodConsumed;
        public event Action FoodSpawned;

        private FoodSpawnerState state;
        private FoodPiece _currentFoodCache;

        public FoodPiece currentFood => _currentFoodCache;

        protected override void OnInitialize()
        {
            match.FoodEaten += OnFoodEaten;
            state = new FoodSpawnerState(this);
            match.board.AddStateObject(state);
        }

        public override void OnMatchStart()
        {
            SpawnNextFood();
        }
        public override void OnBoardReset()
        {
            state = match.board.GetStateObjectFor<FoodSpawnerState>(this);

            if(state.foodPosition != null)
            {
                _currentFoodCache = (FoodPiece) match.board.GetTopPiece(state.foodPosition.Value);
            } else
            {
                _currentFoodCache = null;
            }
            FoodReset?.Invoke();
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
                _currentFoodCache = null;
                FoodConsumed?.Invoke();
            }
        }

        private void SpawnNextFood()
        {
            Vector2Int pos = match.board.GetRandomEmptyPosition();
            BlockAsset block = setup.blockAssetOptions[UnityEngine.Random.Range(0, setup.blockAssetOptions.Length)];
            _currentFoodCache = new FoodPiece();
            currentFood.blockType = block;
            currentFood.spawner = this;
            match.board.AttachPiece(pos, currentFood);
            FoodSpawned?.Invoke();
        }

    }
}
