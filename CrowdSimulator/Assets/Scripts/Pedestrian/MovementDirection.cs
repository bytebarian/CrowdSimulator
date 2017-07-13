public enum MovementDirection
{
    None,
    NorthWest,
    North,
    NorthEast,
    East,
    SouthEast,
    South,
    SouthWest,
    West
}

public static class MovementDirections
{
    private static IntVector2[] vectors = {
        new IntVector2(0, 0),
        new IntVector2(-1, 1),
        new IntVector2(0, 1),
        new IntVector2(1, 1),
        new IntVector2(1, 0),
        new IntVector2(1, -1),
        new IntVector2(0, -1),
        new IntVector2(-1, -1),
        new IntVector2(-1, 0)
    };

    public static IntVector2 ToIntVector2(this MovementDirection direction)
    {
        return vectors[(int)direction];
    }
}
