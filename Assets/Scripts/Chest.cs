using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

  public bool canClose = true;

  private bool open = false;

  private Animator animator;

  private void Start()
  {
    animator = GetComponent<Animator>();
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
    }
    else
    {
      Debug.Log("Opening chest");
      open = true;
      player.GetComponent<PlayerScore>().addScore(50);
    }
    animator.SetBool("Open", open);
  }

}
