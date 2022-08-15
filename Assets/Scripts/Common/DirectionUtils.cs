using UnityEngine;

namespace Common
{
    public enum Direction { Up, Right, Down, Left }
    public static class DirectionUtils
    {
        public static Vector2Int ToVector(this Direction direction)=> direction switch
        {
            Direction.Up => new Vector2Int(0, 1),
            Direction.Right => new Vector2Int(1, 0),
            Direction.Down => new Vector2Int(0, -1),
            Direction.Left => new Vector2Int(-1, 0),
            _ => throw new System.NotImplementedException(),
        };

        public static Vector2Int SpinClockwise(this Vector2Int direction)
        {
            return new Vector2Int(direction.y, -direction.x);
        }
        public static Vector2Int SpinCounterclockwise(this Vector2Int direction)
        {
            return new Vector2Int(-direction.y, direction.x);
        }
    }
}