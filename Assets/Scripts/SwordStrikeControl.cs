using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordStrikeControl : MonoBehaviour
{

  public GameObject effectHandle;

  private ParticleSystem ps;
  private float ticker;

  void Start()
  {
    ps = GetComponentInChildren<ParticleSystem>();
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
  }
}
