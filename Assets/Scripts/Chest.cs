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
      ToggleChest();
    }
  }

  private void ToggleChest()
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
    }
    animator.SetBool("Open", open);
  }

}
