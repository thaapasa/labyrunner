using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportSetup : MonoBehaviour
{

  public float durationSecs = 2.5f;

  private ParticleSystem system;
  private float ticker;
  private float maxRadius = 3;

  // Start is called before the first frame update
  void Start()
  {
    system = GetComponent<ParticleSystem>();
  }

  // Update is called once per frame
  void Update()
  {
    ticker += Time.deltaTime;
    var sh = system.shape;
    sh.radius = Mathf.Min(maxRadius * ticker / durationSecs, maxRadius);

    if (ticker > durationSecs) {
      system.Stop();
    }
  }
}
