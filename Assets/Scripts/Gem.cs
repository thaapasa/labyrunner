using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{

  public float rotationSpeed = 70f;
  public GameObject gemEffect;
  public float gemEffectDurationSeconds = 2.3f;
  
  private AttractToPlayer attractor;

  void Awake() {
    this.attractor = new AttractToPlayer(gameObject, PlayerControl.GetPlayer());
  }

  void Update()
  {
    transform.Rotate(0, Time.deltaTime * rotationSpeed, 0);
    attractor.Update();
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
