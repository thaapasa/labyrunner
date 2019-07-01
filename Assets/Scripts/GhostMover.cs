using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMover : MonoBehaviour
{
  public float wallWidth = 4.0f;
  public float speed = 1.0f;

  private Direction direction;
  private float moveLeft;

  private Vector3[] moveVectors = new Vector3[] {
        new Vector3(0, 0, -1),
        new Vector3(-1, 0, 0),
        new Vector3(0, 0, 1),
        new Vector3(1, 0, 0),
    };

  // Start is called before the first frame update
  void Start()
  {
    newTarget();
  }

  private void newTarget()
  {
    direction = selectNewDirection();
    moveLeft = wallWidth;
    Debug.Log("Ghost moves " + DirectionHelper.name(direction));
  }

  private Direction selectNewDirection()
  {
    int next = Random.Range(0, 4);
    switch (next)
    {
      case 0: return Direction.NORTH;
      case 1: return Direction.SOUTH;
      case 2: return Direction.EAST;
      default: return Direction.WEST;
    }
  }

  void FixedUpdate()
  {
    float moveNow = speed * Time.fixedDeltaTime;
    if (moveNow > moveLeft)
    {
      moveNow = moveLeft;
    }

    transform.position = transform.position + moveVectors[(int)direction] * moveNow;
    moveLeft -= moveNow;

    if (moveLeft <= 0)
    {
      newTarget();
    }
  }
}
