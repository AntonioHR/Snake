
using System.Collections.Generic;
using UnityEngine;

namespace SnakeGame
{
    public class SnakeActorState : ActorStateObject
    {
        public List<Vector2Int> piecePositions = new List<Vector2Int>();

        public List<Board> timeTravelBoards = new List<Board>();
        public bool isAlive;
        public bool isGhostly;
        public Vector2Int moveDirection;

        public SnakeActorState(Actor owner) : base(owner)
        {
        }

        public override object Clone()
        {
            SnakeActorState result = (SnakeActorState)MemberwiseClone();
            result.piecePositions = new List<Vector2Int>(piecePositions);
            //make shallow copies of the time travel board list
            result.timeTravelBoards = new List<Board>(timeTravelBoards);
            return result;
        }
    }
}