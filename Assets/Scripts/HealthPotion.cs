using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{

  public GameObject effect;
  public float effectDurationSeconds;

  public float bounceSpeed = 3; 

  private float yOffs = 0;
  private float ticker = 0;

  private PlayerHealth health;
  private AttractToPlayer attractor;

  void Awake()
  {
    this.health = PlayerControl.GetPlayer().GetComponent<PlayerHealth>();
    this.attractor = new AttractToPlayer(gameObject, PlayerControl.GetPlayer());
  }

  void Start()
  {
    yOffs = this.transform.position.y;
  }

  void Update()
  {
    ticker += Time.deltaTime * bounceSpeed;
    transform.position = new Vector3(transform.position.x, yOffs + Mathf.Sin(ticker) * 0.15f, transform.position.z);
    
    if (!health.isAtMaxHealth())
    {
      attractor.Update();
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.name == "Player")
    {
      if (health.GiveHealth())
      {
        GameObject ef = Instantiate(effect);
        ef.transform.position = gameObject.transform.position;

        Destroy(gameObject);
        Destroy(ef, effectDurationSeconds);
      }
    }
  }

}
