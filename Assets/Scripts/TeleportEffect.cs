using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportEffect : MonoBehaviour
{
  public float spawnEffectTime = 2;
  public float pause = 1;
  public AnimationCurve fadeIn;

  ParticleSystem ps;
  float solidity = 1;

  Renderer[] renderers;

  int shaderProperty;

  public static bool teleporting = false;
  Vector3 teleportTarget = new Vector3(0, 0, 0);

  GameObject player;
  CharacterController controller;

  void Start()
  {
    player = GameObject.Find("Player");
    controller = player.GetComponent<CharacterController>();
    shaderProperty = Shader.PropertyToID("_cutoff");
    renderers = GetComponentsInChildren<Renderer>();
    ps = GetComponentInChildren<ParticleSystem>();

    var main = ps.main;
    main.duration = spawnEffectTime;

    ps.Play();
  }

  void Update()
  {
    if (teleporting) {
      if (solidity == 1) {
        ps.Play();
      }
      if (solidity > 0) {
        solidity = Mathf.Max(solidity - Time.deltaTime / spawnEffectTime, 0);
      }
      if (solidity == 0) {
        teleportPlayer();
      }
    } else {
      if (solidity < 1) {
        solidity = Mathf.Min(solidity + Time.deltaTime / spawnEffectTime, 1);
      }
      if (solidity == 1) {
        ps.Stop();
      }
    }

    float cutoffValue = fadeIn.Evaluate(solidity);
    for (int i = 0; i < renderers.Length; ++i) {
      renderers[i].material.SetFloat(shaderProperty, cutoffValue);
    }
  }

  void teleportPlayer() {
    Debug.Log("Teleporting player to " + teleportTarget);
    teleporting = false;
    controller.enabled = false;
    player.transform.position = teleportTarget;
    controller.enabled = true; 
    ps.Play();
  }
}
