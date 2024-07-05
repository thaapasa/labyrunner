using UnityEngine;

public class AttractToPlayer
{
  private Transform target;
  private Transform player;
  private float attractionDistanceSqr = 4;
  private float attractionSpeed = 4;
  private bool hasSeenPlayer = false;

  public AttractToPlayer(GameObject target, GameObject player)
  {
    this.target = target.transform;
    this.player = player.transform;
  }

  public void Update()
  {
    float distanceToPlayerSqr = (player.position - target.position).sqrMagnitude;

    if (hasSeenPlayer || distanceToPlayerSqr < attractionDistanceSqr)
    {
      hasSeenPlayer = true;
        target.position = Vector3.MoveTowards(target.position, player.position, attractionSpeed * Time.deltaTime);
    }
  }
}
