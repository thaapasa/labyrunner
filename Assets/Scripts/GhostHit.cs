using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHit : MonoBehaviour
{

  public AnimationCurve deathEffectCurve;
  public float deathDurationSecs = 1.0f;
  public GameObject deathEffect;
  public GameObject ghostParent;
  public int ghostKillScore = 25;
  public float deathEffectDurationSeconds = 2f;

  private Renderer ghostRenderer;
  private ParticleSystem deathPs;
  private AudioSource deathSource;

  private int shaderProperty;

  void Start()
  {
    ghostRenderer = GetComponentInChildren<Renderer>();
    deathPs = GetComponentInChildren<ParticleSystem>();
    shaderProperty = Shader.PropertyToID("_cutoff");
    deathSource = GetComponent<AudioSource>();
  }

  void Update()
  {
    if (hasBeenHit)
    {
      deathTicker += Time.deltaTime;
      float dissolved = deathTicker / deathDurationSecs;
      float cutoffValue = Mathf.Min(deathEffectCurve.Evaluate(dissolved), 1f);
      ghostRenderer.material.SetFloat(shaderProperty, cutoffValue);
      if (deathTicker >= deathDurationSecs) {
        hasDied = true;
      }
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.name == "Player" && !hasDied)
    {
      Debug.Log("Collision with ghost");
      other.gameObject.GetComponent<PlayerHealth>().DamagePlayer();

      GameObject effect = Instantiate(deathEffect);
      effect.transform.position = gameObject.transform.position;

      Destroy(ghostParent);
      Destroy(effect, deathEffectDurationSeconds);
    }
  }

  private bool hasBeenHit = false;
  private float deathTicker = 0;
  private bool hasDied = false;

  public void killGhost()
  {
    if (hasBeenHit) {
      return;
    }
    Destroy(ghostParent, deathDurationSecs + 3f);
    deathSource.Play();
    deathPs.Play();
    hasBeenHit = true;

    PlayerControl.GetPlayer().GetComponent<PlayerScore>().addScore(ghostKillScore);
  }

}
