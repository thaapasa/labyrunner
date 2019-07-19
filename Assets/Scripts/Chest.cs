using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

  public AudioClip openSound;
  public AudioClip closeSound;

  public bool canClose = true;

  private bool open = false;

  private Animator animator;
  private AudioSource source;
  private ParticleSystem ps;

  private void Start()
  {
    animator = GetComponent<Animator>();
    source = GetComponent<AudioSource>();
    ps = GetComponentInChildren<ParticleSystem>();
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.name == "Player")
    {
      ToggleChest(other.gameObject);
    }
  }

  private void ToggleChest(GameObject player)
  {
    if (open && canClose)
    {
      Debug.Log("Closing chest");
      open = false;
      source.PlayOneShot(closeSound);
      ps.Stop();
    }
    else if (!open)
    {
      Debug.Log("Opening chest");
      open = true;
      source.PlayOneShot(openSound);
      player.GetComponent<PlayerScore>().addScore(50);
      ps.Play();
    }
    animator.SetBool("Open", open);
  }

}
