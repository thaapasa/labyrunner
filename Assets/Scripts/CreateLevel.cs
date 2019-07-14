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
  public GameObject chest;
  public GameObject candleholder;
  public GameObject carpet;

  public float ghostProbability = 0.07f;
  public float gemProbability = 0.21f;
  public float healthProbability = 0.03f;
  public float chestProbability = 0.01f;
  public float candleholderProbability = 0.12f;
  public float carpetProbability = 0.21f;

  public int labyrinthWidth = 25;
  public int labyrinthHeight = 10;

  public float wallWidth = 4f;
  private float wallOffsetY = -1.6f;

  private int safeZone = 4;

  public Labyrinth labyrinth;

  private Vector3 levelOffset;

  public static int level = 1;

  // Wall to: SOUTH, WEST, NORTH, EAST
  // Is pointing: NORTH, EAST, SOUTH, WEST
  private Vector3[] directionVectors = new Vector3[] {
    new Vector3(0, 0, 1),
    new Vector3(1, 0, 0),
    new Vector3(0, 0, -1),
    new Vector3(-1, 0, 0)
  };

  private static float chOfs = 1.5f;
  // NORTHEAST, SOUTHEAST, SOUTHWEST, NORTHWEST
  private Vector3[] candleHolderCornerOffsets = new Vector3[] {
    new Vector3(chOfs, 0, chOfs),
    new Vector3(chOfs, 0, -chOfs),
    new Vector3(-chOfs, 0, -chOfs),
    new Vector3(-chOfs, 0, chOfs)
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
    Debug.Log("Creating level " + level + ", original " + labyrinthWidth + "x" + labyrinthHeight);
    labyrinthWidth += (level - 1);
    labyrinthHeight += (level - 1);
    Debug.Log("Adjusted level size " + labyrinthWidth + "x" + labyrinthHeight);
    float floorWidth = (labyrinthWidth + safeZone * 2) * wallWidth;
    float floorHeight = (labyrinthHeight + safeZone * 2) * wallWidth;
    floorCollider.transform.localScale = new Vector3(floorWidth, 1, floorHeight);
    float colliderOffsX = floorWidth / 2 - safeZone * wallWidth;
    float colliderOffsY = floorHeight / 2 - safeZone * wallWidth;
    floorCollider.transform.position = new Vector3(colliderOffsX, -0.5f, colliderOffsY);

    labyrinth = new Labyrinth(labyrinthWidth, labyrinthHeight);

    levelOffset = new Vector3(2 * wallWidth, 0, wallWidth);

    GameObject ending = Instantiate(endingArea);
    ending.transform.position = levelOffset + new Vector3(
      labyrinthWidth * wallWidth,
      0,
      (labyrinthHeight - 1) * wallWidth
    );

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

    createItems();
    createDecorations();
  }

  private void createItems()
  {
    for (int x = 0; x < labyrinthWidth; ++x)
    {
      for (int y = 0; y < labyrinthHeight; ++y)
      {
        float r = Random.Range(0f, 1f);
        if (r < ghostProbability)
        {
          createGhost(x, y);
          continue;
        }
        r -= ghostProbability;
        if (r < gemProbability)
        {
          createGem(x, y);
          continue;
        }
        r -= gemProbability;
        if (r < chestProbability)
        {
          createChest(x, y);
          continue;
        }
        r -= chestProbability;
        if (r < healthProbability)
        {
          createHealthPotion(x, y);
          continue;
        }
        r -= healthProbability;
      }
    }
  }

  private void createDecorations()
  {
    for (int x = 0; x < labyrinthWidth; ++x)
    {
      for (int y = 0; y < labyrinthHeight; ++y)
      {
        List<Corner> corners = labyrinth.getCorners(x, y);
        if (corners.Count > 0 && Random.Range(0f, 1f) < candleholderProbability)
        {
          createCandleHolder(x, y, corners[Random.Range(0, corners.Count)]);
        }
        if (labyrinth.isCorridor(x, y) && Random.Range(0f, 1f) < carpetProbability) {
          createCarpet(x, y, labyrinth.hasWallTo(x, y, Direction.SOUTH));
        }
        if (labyrinth.wallCount(x, y) == 3 && Random.Range(0f, 1f) < carpetProbability) {
          createCarpet(x, y, !labyrinth.hasWallTo(x, y, Direction.EAST) || !labyrinth.hasWallTo(x, y, Direction.WEST));
        }
      }
    }
  }

  private void createGhost(int x, int y)
  {
    GameObject g = Instantiate(ghost);
    g.transform.position = toGamePosition(x, y, 0.3f);
  }

  private void createHealthPotion(int x, int y)
  {
    GameObject g = Instantiate(healthPotion);
    g.transform.position = toGamePosition(x, y, 0.2f);
  }

  private void createGem(int x, int y)
  {
    GameObject g = Instantiate(gem);
    g.transform.position = toGamePosition(x, y, 0.26f);
  }

  private void createCandleHolder(int x, int y, Corner corner)
  {
    GameObject g = Instantiate(candleholder);
    g.transform.position = toGamePosition(x, y) + candleHolderCornerOffsets[(int) corner];
  }

  private void createCarpet(int x, int y, bool alignToX)
  {
    GameObject g = Instantiate(carpet);
    g.transform.position = toGamePosition(x, y);
    if (!alignToX) {
      g.transform.Rotate(0, 90, 0);
    }
  }

  private static float chestOffs = 1.3f;

  private void createChest(int x, int y)
  {
    if (labyrinth.hasWallTo(x, y, Direction.NORTH))
    {
      GameObject g = Instantiate(chest);
      g.transform.position = toGamePosition(x, y) + new Vector3(0, 0, chestOffs);
      g.transform.Rotate(0, 90, 0);
    }
    else if (labyrinth.hasWallTo(x, y, Direction.EAST) && x != labyrinth.width - 1 && y != labyrinth.height - 1)
    {
      GameObject g = Instantiate(chest);
      g.transform.position = toGamePosition(x, y) + new Vector3(chestOffs, 0, 0);
      g.transform.Rotate(0, 180, 0);
    }
    else if (labyrinth.hasWallTo(x, y, Direction.WEST) && x != 0 && y != 0)
    {
      GameObject g = Instantiate(chest);
      g.transform.position = toGamePosition(x, y) + new Vector3(-chestOffs, 0, 0);
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
