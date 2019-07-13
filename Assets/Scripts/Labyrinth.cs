using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Labyrinth
{
  public readonly int width;
  public readonly int height;

  // x, y = room coordinate from 0 to size - 1
  // wall[x][y][0] = wall to south from room x, y
  // wall[x][y][1] = wall to west from room x, y
  // wall x, y indexes are from 0 to size
  private bool[][][] walls;
  public bool initialized = false;

  public Labyrinth(int width, int height)
  {
    // Debug.Log("Creating labyrinth of size " + width + "x" + height);
    this.width = width;
    this.height = height;
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
    createLabyrinth();
  }

  private void createLabyrinth()
  {
    createWallsAround();
    createWallsInside();
    LabyrinthCreator l = new LabyrinthCreator(width, height, setWallTo);
    l.createLabyrinth();
    initialized = true;
    /*
    roomInfo(0, 0);
    roomInfo(0, 1);
    roomInfo(0, 2);
    roomInfo(1, 0);
    roomInfo(2, 0);
    roomInfo(1, 1);
    roomInfo(2, 2);
    */
  }

  public bool inRange(int x, int y)
  {
    return x >= 0 && x < width && y >= 0 && y < height;
  }

  public List<Corner> getCorners(int x, int y) {
    List<Corner> corners = new List<Corner>();
    bool n = hasWallTo(x, y, Direction.NORTH);
    bool e = hasWallTo(x, y, Direction.EAST);
    bool s = hasWallTo(x, y, Direction.SOUTH);
    bool w = hasWallTo(x, y, Direction.WEST);
    if (n && e) { corners.Add(Corner.NORTHEAST); }
    if (n && w) { corners.Add(Corner.NORTHWEST); }
    if (s && e) { corners.Add(Corner.SOUTHEAST); }
    if (s && w) { corners.Add(Corner.SOUTHWEST); }
    return corners;
  }

  public bool isCorridor(int x, int y) {
    bool n = hasWallTo(x, y, Direction.NORTH);
    bool e = hasWallTo(x, y, Direction.EAST);
    bool s = hasWallTo(x, y, Direction.SOUTH);
    bool w = hasWallTo(x, y, Direction.WEST);
    return (n && s && !e && !w) || (!n && !s && e && w);
  }

  public int wallCount(int x, int y) {
    int n = hasWallTo(x, y, Direction.NORTH) ? 1 : 0;
    int e = hasWallTo(x, y, Direction.EAST) ? 1 : 0;
    int s = hasWallTo(x, y, Direction.SOUTH) ? 1 : 0;
    int w = hasWallTo(x, y, Direction.WEST) ? 1 : 0;
    return n + e + s + w;
  }

  private void createWallsAround()
  {
    for (int x = 0; x < width; ++x)
    {
      this.walls[x][0][0] = true;
      this.walls[x][height][0] = true;
    }
    for (int y = 0; y < height; ++y)
    {
      this.walls[0][y][1] = true;
      this.walls[width][y][1] = true;
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
      y = y + 1;
      dir = Direction.SOUTH;
    }
    else if (dir == Direction.EAST)
    {
      x = x + 1;
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
      y = y + 1;
      dir = Direction.SOUTH;
    }
    else if (dir == Direction.EAST)
    {
      x = x + 1;
      dir = Direction.WEST;
    }
    if (x < 0 || y < 0 || x > width || y > height)
    {
      return;
    }
    walls[x][y][dir == Direction.SOUTH ? 0 : 1] = state;
  }

  public void roomInfo(int x, int y) {
    Debug.Log(
      "Room at " + x + "," + y +
      ": [N: " + hasWallTo(x, y, Direction.NORTH) +
      ", E: " + hasWallTo(x, y, Direction.EAST) +
      ", S: " + hasWallTo(x, y, Direction.SOUTH) +
      ", W: " + hasWallTo(x, y, Direction.WEST) + "]"
    );
  }

}
