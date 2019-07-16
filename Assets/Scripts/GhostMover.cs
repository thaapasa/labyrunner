using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMover : MonoBehaviour
{
  public float wallWidth = 4.0f;
  public float speed = 1.0f;
  public GameObject level;

  private Direction direction = Direction.NORTH;
  private float moveLeft;

  private CreateLevel levelScript;

  private Vector3[] moveVectors = new Vector3[] {
        new Vector3(0, 0, -1),
        new Vector3(-1, 0, 0),
        new Vector3(0, 0, 1),
        new Vector3(1, 0, 0),
    };

  // Start is called before the first frame update
  void Start()
  {
    level = GameObject.Find("Level");
    levelScript = level.gameObject.GetComponent<CreateLevel>();
  }

  private void newTarget()
  {
    Labyrinth lab = levelScript.labyrinth;
    if (lab == null || !lab.initialized)
    {
      return;
    }

    Coordinates pos = levelScript.toLabyrinthPosition(transform.position);
    // Debug.Log("Ghost is now at " + pos);
    if (!lab.inRange(pos.x, pos.y))
    {
      // Debug.Log("Ghost is outside labyrinth, cannot move!");
      return;
    }

    direction = selectNewDirection(lab, pos, direction);
    moveLeft = wallWidth;
    // Debug.Log("Ghost moves " + direction);
  }

  private Direction selectNewDirection(Labyrinth lab, Coordinates pos, Direction dir)
  {
    Direction right = DirectionHelper.turnRight(dir);
    if (!lab.hasWallTo(pos.x, pos.y, right)) {
      return right;
    }
    // Debug.Log("Could not turn right from " + pos + " towards " + right);
    if (!lab.hasWallTo(pos.x, pos.y, dir)) {
      return dir;
    }
    // Debug.Log("Could not move straight from " + pos + " towards " + dir);
    Direction left = DirectionHelper.turnLeft(dir);
    if (!lab.hasWallTo(pos.x, pos.y, left)) {
      return left;
    }
    // Debug.Log("Could not turn left from " + pos + " towards " + left);
    dir = DirectionHelper.turnRight(right);
    // Debug.Log("Returning towards " + dir + ", wall: " + lab.hasWallTo(pos.x, pos.y, dir));
    return dir;
  }

  void FixedUpdate()
  {
    if (moveLeft <= 0)
    {
      newTarget();
      if (moveLeft <= 0)
      {
        return;
      }
    }

    float moveNow = speed * Time.fixedDeltaTime;
    if (moveNow > moveLeft)
    {
      moveNow = moveLeft;
    }

    transform.position = transform.position + moveVectors[(int)direction] * moveNow;
    transform.rotation.SetLookRotation(moveVectors[(int)direction]);
    moveLeft -= moveNow;

  }
}
