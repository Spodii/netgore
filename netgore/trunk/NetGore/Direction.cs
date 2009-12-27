using Microsoft.Xna.Framework;

namespace NetGore
{
    /// <summary>
    /// A direction.
    /// </summary>
    public enum Direction
    {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest
    }

    public static class DirectionHelper
    {
        public static Direction? FromVector(Vector2 v)
        {
            if (v.Y < 0)
            {
                if (v.X < 0)
                    return Direction.NorthWest;
                else if (v.X == 0)
                    return Direction.North;
                else
                    return Direction.NorthEast;
            }
            else if (v.Y == 0)
            {
                if (v.X < 0)
                    return Direction.West;
                else if (v.X == 0)
                    return null;
                else
                    return Direction.East;
            }
            else
            {
                if (v.X < 0)
                    return Direction.SouthWest;
                else if (v.X == 0)
                    return Direction.South;
                else
                    return Direction.SouthEast;
            }
        }
    }
}