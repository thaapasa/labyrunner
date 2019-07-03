using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLevel : MonoBehaviour
{

  public GameObject wall;
  public GameObject floor;
  public GameObject floorCollider;
  public GameObject endingArea;
  public GameObject ghost;
  public GameObject healthPotion;
  public GameObject gem;

  public int labyrinthWidth = 25;
  public int labyrinthHeight = 10;

  public float wallWidth = 4f;
  private float wallOffsetY = -1.6f;

  private int safeZone = 4;

  public Labyrinth labyrinth;

  private Vector3 levelOffset;

  // Wall to: SOUTH, WEST, NORTH, EAST
  // Is pointing: NORTH, EAST, SOUTH, WEST
  private Vector3[] directionVectors = new Vector3[] {
    new Vector3(0, 0, 1),
    new Vector3(1, 0, 0),
    new Vector3(0, 0, -1),
    new Vector3(-1, 0, 0)
  };

  public Coordinates toLabyrinthPosition(Vector3 gamePos)
  {
    Vector3 p = (gamePos - levelOffset) / wallWidth;
    return new Coordinates((int)(p.x + 0.5), (int)(p.z + 0.5));
  }

  public Vector3 toGamePosition(int x, int y, float gameYPos = 0f)
  {
    Vector3 p = new Vector3(x, gameYPos, y);
    return p * wallWidth + levelOffset;
  }

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

    levelOffset = new Vector3(2 * wallWidth, 0, wallWidth);

    GameObject ending = Instantiate(endingArea);
    ending.transform.position = levelOffset + new Vector3(labyrinthWidth * wallWidth, 0, (labyrinthHeight - 1) * wallWidth);

    for (int x = 0; x <= labyrinth.width; x++)
    {
      for (int y = 0; y <= labyrinth.height; y++)
      {
        if (labyrinth.hasWallTo(x, y, Direction.SOUTH))
        {
          createRoomWall(x, y, Direction.SOUTH);
        }
        if (labyrinth.hasWallTo(x, y, Direction.WEST))
        {
          if ((x != 0 || y != 0) && (x != labyrinthWidth || y != labyrinthHeight - 1))
          {
            createRoomWall(x, y, Direction.WEST);
          }
        }
        if (x < labyrinth.width && y < labyrinth.height)
        {
          createRoomFloor(x, y);
        }
      }
    }

    createGhosts((int)(labyrinthWidth / 1.5));
    createHealthPotions((int)(labyrinthWidth / 2));
    createGems((int)(labyrinthWidth * 2.5));
  }

  private void createGhosts(int amount)
  {
    for (int i = 0; i < amount; ++i) {
      GameObject g = Instantiate(ghost);
      int x = Random.Range(0, labyrinth.width);
      int y = Random.Range(0, labyrinth.height);
      g.transform.position = toGamePosition(x, y, 0.3f);
    }
  }

  private void createHealthPotions(int amount)
  {
    for (int i = 0; i < amount; ++i) {
      GameObject g = Instantiate(healthPotion);
      int x = Random.Range(0, labyrinth.width);
      int y = Random.Range(0, labyrinth.height);
      g.transform.position = toGamePosition(x, y, 0.2f);
    }
  }

  private void createGems(int amount)
  {
    for (int i = 0; i < amount; ++i) {
      GameObject g = Instantiate(gem);
      int x = Random.Range(0, labyrinth.width);
      int y = Random.Range(0, labyrinth.height);
      g.transform.position = toGamePosition(x, y, 0.3f);
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
