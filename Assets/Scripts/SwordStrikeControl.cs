using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordStrikeControl : MonoBehaviour
{

  public GameObject effectHandle;
  private float hitLifetime = 0.8f;

  private ParticleSystem ps;
  private GameObject hitCollider;
  private float ticker;

  void Start()
  {
    ps = GetComponentInChildren<ParticleSystem>();
    hitCollider = gameObject.transform.GetChild(0).GetChild(1).gameObject;
  }

  void Update()
  {
    ticker += Time.deltaTime;

    if (ticker > 0.2) {
      ps.Stop();
    } else {
      var sh = ps.shape;
      sh.rotation = new Vector3(-35, ticker * 45 * 5 - 22.5f, 0);
    }
    if (ticker > hitLifetime) {
      Destroy(hitCollider);
      hitCollider = null;
    }
    if (hitCollider != null) {
      hitCollider.transform.localPosition = new Vector3(0, 0.6f, ticker * 5.8f);
      hitCollider.transform.localScale = new Vector3(1 + ticker * 2.5f, 2f, 0.5f);
    }
  }
}
