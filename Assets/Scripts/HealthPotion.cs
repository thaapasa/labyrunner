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

  void Start()
  {
    yOffs = this.transform.position.y;
  }

  void Update()
  {
    ticker += Time.deltaTime * bounceSpeed;
    transform.position = new Vector3(transform.position.x, yOffs + Mathf.Sin(ticker) * 0.15f, transform.position.z);
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.name == "Player")
    {
      if (other.gameObject.GetComponent<PlayerHealth>().giveHealth())
      {
        GameObject ef = Instantiate(effect);
        ef.transform.position = gameObject.transform.position;

        Destroy(gameObject);
        Destroy(ef, effectDurationSeconds);
      }
    }
  }

}
