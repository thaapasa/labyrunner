public enum Direction
{
  SOUTH = 0,
  WEST = 1,
  NORTH = 2,
  EAST = 3
}

public struct Coordinates
{
  public readonly int x;
  public readonly int y;

  public Coordinates(int p1, int p2)
  {
    x = p1;
    y = p2;
  }

  public int index(int width)
  {
    return x + y * width;
  }

  public static Coordinates fromIndex(int index, int width)
  {
    int y = index / width;
    return new Coordinates(index - y * width, y);
  }

  public Coordinates towards(Direction dir)
  {
    switch (dir)
    {
      case Direction.NORTH: return new Coordinates(x, y + 1);
      case Direction.SOUTH: return new Coordinates(x, y - 1);
      case Direction.EAST: return new Coordinates(x + 1, y);
      case Direction.WEST: return new Coordinates(x - 1, y);
    }
    return this;
  }
}

public struct CoordinatesWithDirection
{
  public Coordinates coords;
  public Direction direction;
  public CoordinatesWithDirection(Coordinates c, Direction d)
  {
    coords = c;
    direction = d;
  }
  public static CoordinatesWithDirection fromWall(int wall, int width)
  {
    return new CoordinatesWithDirection(
      Coordinates.fromIndex(wall / 2, width),
      (wall % 2 == 0) ? Direction.SOUTH : Direction.WEST
    );
  }
}
