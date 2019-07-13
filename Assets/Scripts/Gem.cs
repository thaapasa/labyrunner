using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{

  public float rotationSpeed = 70f;
  public GameObject gemEffect;
  public float gemEffectDurationSeconds = 2.3f;

  void Update()
  {
    transform.Rotate(0, Time.deltaTime * rotationSpeed, 0);
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.name == "Player")
    {
      other.gameObject.GetComponent<PlayerScore>().addScore(10);
            
      GameObject effect = Instantiate(gemEffect);
      effect.transform.position = gameObject.transform.position;

      Destroy(gameObject, 0.04f);
      Destroy(effect, gemEffectDurationSeconds);
    }
  }

}
