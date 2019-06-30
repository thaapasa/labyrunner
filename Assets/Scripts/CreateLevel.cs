using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLevel : MonoBehaviour
{

  public GameObject wall;
  public GameObject floor;
  public GameObject floorCollider;
  public int labyrinthWidth = 25;
  public int labyrinthHeight = 10;

  public float wallWidth = 4f;
  private float wallOffsetY = -1.6f;

  private int safeZone = 4;

  private Labyrinth labyrinth;

  private Vector3 levelOffset;

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
    float floorWidth = (labyrinthWidth + safeZone * 2) * wallWidth;
    float floorHeight = (labyrinthHeight + safeZone * 2) * wallWidth;
    floorCollider.transform.localScale = new Vector3(floorWidth, 1, floorHeight);
    float colliderOffsX = floorWidth / 2 - safeZone * wallWidth;
    float colliderOffsY = floorHeight / 2 - safeZone * wallWidth;
    floorCollider.transform.position = new Vector3(colliderOffsX, -0.5f, colliderOffsY);

    labyrinth = new Labyrinth(labyrinthWidth, labyrinthHeight);
    labyrinth.createLabyrinth();

    levelOffset = new Vector3(2 * wallWidth, 0, 0);

    // Starting zone
    createRoomFloor(-1, 0);
    createRoomWall(-1, 0, Direction.SOUTH);
    createRoomWall(-1, 1, Direction.SOUTH);
    createRoomFloor(-2, 0);
    createRoomWall(-2, 0, Direction.SOUTH);
    createRoomWall(-2, 1, Direction.SOUTH);
    createRoomWall(-2, 0, Direction.WEST);

    // Ending zone
    createRoomFloor(labyrinthWidth, labyrinthHeight - 1);
    createRoomWall(labyrinthWidth, labyrinthHeight - 1, Direction.SOUTH);
    createRoomWall(labyrinthWidth, labyrinthHeight, Direction.SOUTH);
    createRoomFloor(labyrinthWidth + 1, labyrinthHeight - 1);
    createRoomWall(labyrinthWidth + 1, labyrinthHeight - 1, Direction.SOUTH);
    createRoomWall(labyrinthWidth + 1, labyrinthHeight, Direction.SOUTH);
    createRoomWall(labyrinthWidth + 2, labyrinthHeight - 1, Direction.WEST);

    for (int x = 0; x <= labyrinth.width; x++)
    {
      for (int y = 0; y <= labyrinth.height; y++)
      {
        float xpos = x * wallWidth;
        float ypos = y * wallWidth;
        if (labyrinth.hasWallTo(x, y, Direction.SOUTH))
        {
          createRoomWall(x, y, Direction.SOUTH);
        }
        if (labyrinth.hasWallTo(x, y, Direction.WEST))
        {
          if ((x != 0 || y != 0) && (x != labyrinthWidth || y != labyrinthHeight - 1)) {
            createRoomWall(x, y, Direction.WEST);
          }
        }
        if (x < labyrinth.width && y < labyrinth.height)
        {
          createRoomFloor(x, y);
        }
      }
    }
  }

  private void createRoomWall(int x, int y, Direction direction)
  {
    GameObject w = Instantiate(wall);
    float xpos = x * wallWidth;
    float ypos = y * wallWidth;
    float offs = wallWidth / 2f;
    switch (direction)
    {
      case Direction.SOUTH:
        w.transform.position = new Vector3(xpos, wallOffsetY, ypos - offs) + levelOffset;
        break;
      case Direction.WEST:
        w.transform.position = new Vector3(xpos - offs, wallOffsetY, ypos) + levelOffset;
        break;
    }
    w.transform.rotation = Quaternion.LookRotation(directionVectors[(int)direction], Vector3.up);
  }

  private void createRoomFloor(int x, int y)
  {
    GameObject f = Instantiate(floor);
    float xpos = x * wallWidth;
    float ypos = y * wallWidth;
    f.transform.position = new Vector3(xpos, 0, ypos) + levelOffset;
  }
}
