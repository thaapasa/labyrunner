using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportEffect : MonoBehaviour
{
  public float spawnEffectTime = 2f;
  public float setupEffectTime = 1.3f;
  public float pause = 1;
  public AnimationCurve fadeIn;

  ParticleSystem ps;
  float solidity = 1;

  Renderer[] renderers;

  int shaderProperty;

  public static bool teleporting = false;
  public static bool settingUp = false;

  Vector3 teleportTarget = new Vector3(0, 0, 0);

  public GameObject setupEffect;
  public GameObject placeEffect;
  GameObject player;
  CharacterController controller;
  public AudioClip teleportClip;

  private AudioSource audioSource;

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
    audioSource = player.GetComponent<AudioSource>();
  }

  private float setupTimer;
  private GameObject currentSetupEffect;
  void Update()
  {
    if (settingUp) {
      if (setupTimer == 0) {
        Debug.Log("Create Teleport setup effect");
        currentSetupEffect = Instantiate(setupEffect);
      }
      currentSetupEffect.transform.position = player.transform.position;
      setupTimer += Time.deltaTime;
      if (setupTimer >= setupEffectTime) {
        Vector3 pos = player.transform.position;
        Debug.Log("Set teleport position to " + pos);
        teleportTarget = pos;
        GameObject ef = Instantiate(placeEffect);
        Destroy(currentSetupEffect);
        currentSetupEffect = null;
        ef.transform.position = player.transform.position;
        Destroy(ef, 5);
        settingUp = false;
        setupTimer = 0;
      }
    } else {
      setupTimer = 0;
    }
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
    audioSource.PlayOneShot(teleportClip);
  }
}
