using System;

class LabyrinthCreator
{

  private int width;
  private int height;

  private Random random = new Random();

  private Action<int, int, Direction, bool> wallSetter;

  public LabyrinthCreator(int width, int height, Action<int, int, Direction, bool> wallSetter)
  {
    this.width = width;
    this.height = height;
    this.wallSetter = wallSetter;
  }

  public void createLabyrinth()
  {
    int numRooms = width * height;
    int[] rooms = new int[numRooms];
    for (int i = 0; i < rooms.Length; ++i) { rooms[i] = -1; }
    // A bit too much, but so what
    int numWalls = numRooms * 2;

    int[] wallOrder = new int[numWalls];
    for (int i = 0; i < numWalls; ++i) { wallOrder[i] = i; }
    shuffle(wallOrder);

    for (int i = 0; i < numWalls; ++i)
    {
      punctureWall(wallOrder[i], rooms);
    }
  }

  private void punctureWall(int i, int[] rooms)
  {
    CoordinatesWithDirection d = CoordinatesWithDirection.fromWall(i, width);
    Coordinates r1 = d.coords;
    Direction dir = d.direction;
    Coordinates r2 = r1.towards(dir);

    if (inRange(r1) && inRange(r2))
    {
      // Find rooms that the wall connects
      int r1i = r1.index(width);
      int r2i = r2.index(width);

      // Find roots of each room tree
      int r1Parent = findRoot(r1i, rooms);
      int r2Parent = findRoot(r2i, rooms);

      // If rooms are in different trees, then break the wall & connect room trees
      if (r1Parent != r2Parent)
      {
        wallSetter(r1.x, r1.y, dir, false);
        rooms[r2Parent] = r1Parent;
      }

      // Shorten root paths (moves all room tree leafs on search path directly under the root)
      shortenPath(r1i, r1Parent, rooms);
      shortenPath(r2i, r1Parent, rooms);
    }
  }

  private void shortenPath(int c, int parent, int[] rooms)
  {
    int next = rooms[c];
    if (next != -1)
    {
      rooms[c] = parent;
      shortenPath(next, parent, rooms);
    }
  }

  private bool inRange(Coordinates c)
  {
    return c.x >= 0 && c.y >= 0 && c.x < width && c.y < height;
  }

  private int findRoot(int c, int[] rooms)
  {
    if (rooms[c] == -1) { return c; }
    else { return findRoot(rooms[c], rooms); }
  }


  private int[] shuffle(int[] arr)
  {
    // Fisher-Yates shuffle
    for (int i = arr.Length - 1; i >= 1; i--)
    {
      swap(arr, i, random.Next(i + 1));
    }
    return arr;
  }

  private void swap(int[] a, int i, int j)
  {
    int s = a[i];
    a[i] = a[j];
    a[j] = s;
  }

}
