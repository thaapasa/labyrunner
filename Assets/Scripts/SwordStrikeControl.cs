using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordStrikeControl : MonoBehaviour
{

  public GameObject effectHandle;

  private ParticleSystem ps;
  private BoxCollider hitCollider;
  private float ticker;

  void Start()
  {
    ps = GetComponentInChildren<ParticleSystem>();
    hitCollider = GetComponentInChildren<BoxCollider>();
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
    hitCollider.center = new Vector3(0, 0.6f, ticker * 2.8f);
    hitCollider.size = new Vector3(1 + ticker, 2f, 0.5f);
  }
}
