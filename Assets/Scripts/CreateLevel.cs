using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLevel : MonoBehaviour
{

  public GameObject wall;
  public int labyrinthSize = 10;

  private float wallWidth = 4f;
  private float wallOffsetY = -2f;

  private float wallThickness = 0.2f;

  private Labyrinth labyrinth;

  // Wall to: SOUTH, WEST, NORTH, EAST
  // Is pointing: NORTH, EAST, SOUTH, WEST
  private Vector3[] directionVectors = new Vector3[] {
    new Vector3(0, 0, 1),
    new Vector3(1, 0, 0),
    new Vector3(0, 0, -1),
    new Vector3(-1, 0, 0)
  };

  // Start is called before the first frame update
  void Start()
  {
    labyrinth = new Labyrinth(labyrinthSize);
    labyrinth.createWallsAround();
    labyrinth.createWallsInside();

    for (int x = 0; x <= labyrinthSize; x++)
    {
      for (int y = 0; y <= labyrinthSize; y++)
      {
        float xpos = x * wallWidth;
        float ypos = y * wallWidth;
        if (labyrinth.hasWallTo(x, y, WallDirection.SOUTH)) {
          createRoomWall(x, y, WallDirection.SOUTH);
        }
        if (labyrinth.hasWallTo(x, y, WallDirection.WEST)) {
          createRoomWall(x, y, WallDirection.WEST);
        }
      }
    }
  }

  private void createRoomWall(int x, int y, WallDirection direction) {
    GameObject w = Instantiate(wall);
    float xpos = x * (wallWidth + wallThickness);
    float ypos = y * wallWidth;
    float offs = wallWidth / 2f;
    float thickOffs = wallThickness / 2f;
    switch (direction) {
      case WallDirection.SOUTH:
        w.transform.position = new Vector3(xpos, wallOffsetY, ypos - offs);
        break;
      case WallDirection.WEST:
        w.transform.position = new Vector3(xpos - offs - thickOffs, wallOffsetY, ypos);
        break;
    }
    w.transform.rotation = Quaternion.LookRotation(directionVectors[(int) direction], Vector3.up);
  }

}
