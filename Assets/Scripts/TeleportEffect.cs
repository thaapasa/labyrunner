using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportEffect : MonoBehaviour
{
  public float spawnEffectTime = 2;
  public float pause = 1;
  public AnimationCurve fadeIn;

  ParticleSystem ps;
  float timer = 0;
  Renderer[] _renderers;

  int shaderProperty;

  void Start()
  {
    shaderProperty = Shader.PropertyToID("_cutoff");
    _renderers = GetComponentsInChildren<Renderer>();
    ps = GetComponentInChildren<ParticleSystem>();

    var main = ps.main;
    main.duration = spawnEffectTime;

    ps.Play();

  }

  void Update()
  {
    if (timer < spawnEffectTime + pause)
    {
      timer += Time.deltaTime;
    }
    else
    {
      ps.Play();
      timer = 0;
    }

    float cutoffValue = fadeIn.Evaluate(Mathf.InverseLerp(0, spawnEffectTime, timer));
    for (int i = 0; i < _renderers.Length; ++i) {
      _renderers[i].material.SetFloat(shaderProperty, cutoffValue);
    }
  }
}
