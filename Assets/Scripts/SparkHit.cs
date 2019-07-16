using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkHit : MonoBehaviour
{
  private void OnTriggerEnter(Collider other)
  {
    Debug.Log("Sparks hit " + other.gameObject.name);
    if (other.gameObject.name == "GhostWrapper")
    {
      Debug.Log("Sparks hit ghost");
      killGhost(other.gameObject);
    }
  }

  void killGhost(GameObject ghost)
  {
    ghost.GetComponent<GhostHit>().killGhost();
  }

}
