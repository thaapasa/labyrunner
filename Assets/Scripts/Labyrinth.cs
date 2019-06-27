using System;
using System.Collections;
using System.Collections.Generic;

public class Labyrinth
{
  public readonly int width;
  public readonly int height;

  // x, y = room coordinate from 0 to size - 1
  // wall[x][y][0] = wall to south from room x, y
  // wall[x][y][1] = wall to west from room x, y
  // wall x, y indexes are from 0 to size
  private bool[][][] walls;

  private Random random = new Random();

  public Labyrinth(int size)
  {
    this.width = size;
    this.height = size;
    int wXSize = width + 1;
    int wYSize = height + 1;
    this.walls = new bool[wXSize][][];
    for (int x = 0; x < wXSize; ++x)
    {
      this.walls[x] = new bool[wYSize][];
      for (int y = 0; y < wYSize; ++y)
      {
        this.walls[x][y] = new bool[2];
        this.walls[x][y][0] = false;
        this.walls[x][y][1] = false;
      }
    }
  }

  public void createLabyrinth()
  {
    createWallsAround();
    createWallsInside();
    LabyrinthCreator l = new LabyrinthCreator(width, height, setWallTo);
    l.createLabyrinth();
  }

  private void createWallsAround()
  {
    for (int x = 0; x < width; ++x)
    {
      this.walls[x][0][0] = true;
      this.walls[x][width][0] = true;
    }
    for (int y = 0; y < height; ++y)
    {
      this.walls[0][y][1] = true;
      this.walls[height][y][1] = true;
    }
  }

  private void createWallsInside()
  {
    for (int x = 0; x < width; ++x)
    {
      for (int y = 0; y < height; ++y)
      {
        walls[x][y][0] = true;
        walls[x][y][1] = true;
      }
    }
  }

  public bool hasWallTo(int x, int y, Direction dir)
  {
    if (dir == Direction.NORTH)
    {
      x = x + 1;
      dir = Direction.SOUTH;
    }
    else if (dir == Direction.EAST)
    {
      y = y + 1;
      dir = Direction.WEST;
    }
    if (x < 0 || y < 0 || x > width || y > height)
    {
      return false;
    }
    return walls[x][y][dir == Direction.SOUTH ? 0 : 1];
  }

  public void setWallTo(int x, int y, Direction dir, bool state)
  {
    if (dir == Direction.NORTH)
    {
      x = x + 1;
      dir = Direction.SOUTH;
    }
    else if (dir == Direction.EAST)
    {
      y = y + 1;
      dir = Direction.WEST;
    }
    if (x < 0 || y < 0 || x > width || y > height)
    {
      return;
    }
    walls[x][y][dir == Direction.SOUTH ? 0 : 1] = state;
  }

}
