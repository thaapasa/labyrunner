using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLevel : MonoBehaviour
{

  public GameObject wall;
  public GameObject floor;
  public int labyrinthSize = 10;

  private float wallWidth = 4f;
  private float wallOffsetY = -1.6f;

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
    labyrinth.createLabyrinth();

    for (int x = 0; x <= labyrinth.width; x++)
    {
      for (int y = 0; y <= labyrinth.height; y++)
      {
        float xpos = x * wallWidth;
        float ypos = y * wallWidth;
        if (labyrinth.hasWallTo(x, y, Direction.SOUTH)) {
          createRoomWall(x, y, Direction.SOUTH);
        }
        if (labyrinth.hasWallTo(x, y, Direction.WEST)) {
          createRoomWall(x, y, Direction.WEST);
        }
        createRoomFloor(x, y);
      }
    }
  }

  private void createRoomWall(int x, int y, Direction direction) {
    GameObject w = Instantiate(wall);
    float xpos = x * wallWidth;
    float ypos = y * wallWidth;
    float offs = wallWidth / 2f;
    switch (direction) {
      case Direction.SOUTH:
        w.transform.position = new Vector3(xpos, wallOffsetY, ypos - offs);
        break;
      case Direction.WEST:
        w.transform.position = new Vector3(xpos - offs, wallOffsetY, ypos);
        break;
    }
    w.transform.rotation = Quaternion.LookRotation(directionVectors[(int) direction], Vector3.up);
  }

  private void createRoomFloor(int x, int y) {
    GameObject w = Instantiate(floor);
    float xpos = x * wallWidth;
    float ypos = y * wallWidth;
    w.transform.position = new Vector3(xpos, 0, ypos);
  }
}
