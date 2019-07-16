using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHit : MonoBehaviour
{

  public AnimationCurve deathEffectCurve;
  public float deathDurationSecs = 1.0f;
  public GameObject deathEffect;
  public GameObject ghostParent;
  public float deathEffectDurationSeconds = 2f;

  private Renderer ghostRenderer;

  private int shaderProperty;

  void Start()
  {
    ghostRenderer = GetComponentInChildren<Renderer>();
    shaderProperty = Shader.PropertyToID("_cutoff");
  }

  void Update()
  {
    if (hasBeenHit)
    {
      deathTicker += Time.deltaTime;
      float dissolved = deathTicker / deathDurationSecs;
      float cutoffValue = deathEffectCurve.Evaluate(dissolved);
      ghostRenderer.material.SetFloat(shaderProperty, cutoffValue);
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.name == "Player")
    {
      Debug.Log("Collision with ghost");
      other.gameObject.GetComponent<PlayerHealth>().damagePlayer();

      GameObject effect = Instantiate(deathEffect);
      effect.transform.position = gameObject.transform.position;

      Destroy(ghostParent);
      Destroy(effect, deathEffectDurationSeconds);
    }
  }

  private bool hasBeenHit = false;
  private float deathTicker = 0;

  public void killGhost()
  {
    Destroy(ghostParent, deathDurationSecs);
    hasBeenHit = true;
  }

}
