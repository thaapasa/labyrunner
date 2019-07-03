using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{

  public float rotationSpeed = 70f;

  void Update()
  {
    transform.Rotate(0, Time.deltaTime * rotationSpeed, 0);
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.name == "Player")
    {
      other.gameObject.GetComponent<PlayerScore>().addScore(10);
      Destroy(gameObject);
    }
  }

}
