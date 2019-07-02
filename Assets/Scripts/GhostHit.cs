using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHit : MonoBehaviour
{

  public GameObject deathEffect;
  public float deathEffectDurationSeconds = 2f;


  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.name == "Player")
    {
      Debug.Log("Collision with ghost");
      other.gameObject.GetComponent<PlayerHealth>().damagePlayer();

      GameObject effect = Instantiate(deathEffect);
      effect.transform.position = gameObject.transform.position;

      Destroy(gameObject);
      Destroy(effect, deathEffectDurationSeconds);
    }
  }
}
