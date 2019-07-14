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

  private void Start()
  {
    animator = GetComponent<Animator>();
    source = GetComponent<AudioSource>();
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
    }
    else
    {
      Debug.Log("Opening chest");
      open = true;
      source.PlayOneShot(openSound);
      player.GetComponent<PlayerScore>().addScore(50);
    }
    animator.SetBool("Open", open);
  }

}
