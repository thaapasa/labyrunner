using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
  CharacterController characterController;
  Animator animator;
  public Camera playerCamera;

  public float speed = 6.0f;
  public float jumpSpeed = 8.0f;
  public float gravity = 20.0f;

  public float runTolerance = 5f;

  private float verticalSpeed = 0f;

  private Vector3 moveDirection = Vector3.zero;

  void Start()
  {
    characterController = GetComponent<CharacterController>();
    animator = GetComponent<Animator>();
  }

  void FixedUpdate()
  {
    bool grounded = characterController.isGrounded;
    animator.SetBool("Grounded", grounded);

    #region ACTION
    if (grounded)
    {
      if (Input.GetButtonDown("Fire"))
      {
        animator.SetTrigger("Slash");
      }

      // animator.SetBool("Guard", Input.GetButton("Block"));

      if (Input.GetButtonDown("Teleport")) {
        Debug.Log("Teleporting...");
        TeleportEffect.teleporting = true;
      }
      if (Input.GetButtonUp("Teleport")) {
        TeleportEffect.teleporting = false;
      }

      if (Input.GetButtonDown("PlacePortal")) {
        TeleportEffect.settingUp = true;
      }
      if (Input.GetButtonUp("PlacePortal")) {
        TeleportEffect.settingUp = false;
      }

      if (animator.GetCurrentAnimatorStateInfo(1).IsName("Sword_Iai") ||
          animator.GetCurrentAnimatorStateInfo(1).IsName("Sword_Guard") ||
          animator.GetCurrentAnimatorStateInfo(1).IsName("Sword_Store"))
      {
        return;
      }
    }
    #endregion


    #region MOVEMENT
    float horizontal = Input.GetAxis("Horizontal");
    float vertical = Input.GetAxis("Vertical");

    bool hasMovement = moveDirection.sqrMagnitude > 0.01f;

    if (grounded)
    {
      verticalSpeed = 0;
      Vector3 camForward = Vector3.Scale(playerCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
      moveDirection = camForward * vertical + playerCamera.transform.right * horizontal;
      hasMovement = moveDirection.sqrMagnitude > 0.01f;

      animator.SetBool("Jump", false);
      animator.SetBool("Fall", false);

      if (hasMovement)
      {
        animator.SetBool("Move", true);
        animator.SetBool("Run", moveDirection.sqrMagnitude > runTolerance);
      }
      else
      {
        animator.SetBool("Move", false);
      }
      if (Input.GetButton("Jump"))
      {
        verticalSpeed = jumpSpeed;
        animator.SetBool("Jump", true);
      }
    }
    else
    {
      if (verticalSpeed < 0)
      {
        animator.SetBool("Fall", true);
      }
    }

    if (hasMovement)
    {
      // Rotate player towards movement
      transform.rotation = Quaternion.LookRotation(moveDirection, Vector3.up);
    }

    // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
    // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
    // as an acceleration (ms^-2)
    if (!grounded)
    {
      verticalSpeed -= gravity * Time.fixedDeltaTime;
    }

    // Move the player
    characterController.Move(moveDirection * speed * Time.fixedDeltaTime);
    characterController.Move(Vector3.up * verticalSpeed * Time.fixedDeltaTime);
    #endregion
  }
}
