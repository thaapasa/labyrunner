public enum WallDirection {
  SOUTH = 0,
  WEST = 1,
  NORTH = 2,
  EAST = 3
}

public class Labyrinth
{
  public readonly int size;

  // x, y = room coordinate from 0 to size - 1
  // wall[x][y][0] = wall to south from room x, y
  // wall[x][y][1] = wall to west from room x, y
  // wall x, y indexes are from 0 to size
  private bool[][][] walls;

  public Labyrinth(int size) {
    this.size = size;
    int wSize = size + 1;
    this.walls = new bool[wSize][][];
    for (int x = 0; x < wSize; ++x) {
      this.walls[x] = new bool[wSize][];
      for (int y = 0; y < wSize; ++y) {
        this.walls[x][y] = new bool[2];
        this.walls[x][y][0] = false;
        this.walls[x][y][1] = false;        
      }
    }
  }

  public void createWallsAround() {
    for (int i = 0; i < size; ++i) {
      this.walls[i][0][0] = true;
      this.walls[0][i][1] = true;
      this.walls[i][size][0] = true;
      this.walls[size][i][1] = true;
    }
  }

  public void createWallsInside() {
    for (int x = 0; x < size; ++x) {
      for (int y = 0; y < size; ++y) {
        walls[x][y][0] = true;
        walls[x][y][1] = true;
      }
    }
  }

  public bool hasWallTo(int x, int y, WallDirection dir) {
    if (dir == WallDirection.NORTH) {
      x = x + 1;
      dir = WallDirection.SOUTH;
    } else if (dir == WallDirection.EAST) {
      y = y + 1;
      dir = WallDirection.WEST;
    }
    if (x < 0 || y < 0 || x > size || y > size) {
      return false;
    }
    return walls[x][y][dir == WallDirection.SOUTH ? 0 : 1];
  }

}
