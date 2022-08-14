using System;

namespace SnakeGame
{
    public class FoodSpawnerActor : Actor<FoodSpawnerActor.Setup>
    {
        [Serializable]
        public class Setup
        {

        }

        private FoodPiece spawnedFood;

        protected override void OnInitialize()
        {
            match.FoodEaten += OnFoodEaten;
        }

        public override void OnMatchStart()
        {
            SpawnNextFood();
        }

        private void OnFoodEaten(FoodPiece foodPiece)
        {
            if(foodPiece == this.spawnedFood)
            {
                SpawnNextFood();
            }
        }

        private void SpawnNextFood()
        {
            throw new NotImplementedException();
        } 
    }
}
