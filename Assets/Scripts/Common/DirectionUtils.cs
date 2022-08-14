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

        public static Direction Invert(Direction direction) => direction switch
        {
            Direction.Up => Direction.Down,
            Direction.Right => Direction.Left,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            _ => throw new System.NotImplementedException(),
        };

        public static Direction SpinCounterclockwise(this Direction direction) => direction switch
        {
            Direction.Up => Direction.Left,
            Direction.Right => Direction.Up,
            Direction.Down => Direction.Right,
            Direction.Left => Direction.Down,
            _ => throw new System.NotImplementedException(),
        };
        public static Direction SpinClockwise(this Direction direction) => direction switch
        {
            Direction.Up => Direction.Right,
            Direction.Right => Direction.Down,
            Direction.Down => Direction.Left,
            Direction.Left => Direction.Up,
            _ => throw new System.NotImplementedException(),
        };
    }
}