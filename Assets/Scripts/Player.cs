using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  CharacterController characterController;
  Animator animator;

  public float speed = 6.0f;
  public float jumpSpeed = 8.0f;
  public float gravity = 20.0f;

  private Vector3 moveDirection = Vector3.zero;
  private float movementTolerance = 0.001f;
  private float runTolerance = 5;

  void Start()
  {
    characterController = GetComponent<CharacterController>();
    animator = GetComponent<Animator>();
  }

  void FixedUpdate()
  {
    float horizontal = Input.GetAxis("Horizontal");
    float vertical = Input.GetAxis("Vertical");
    bool hasMovement = Mathf.Abs(horizontal) > movementTolerance || Mathf.Abs(vertical) > movementTolerance;

    animator.SetBool("Grounded", characterController.isGrounded);
    if (characterController.isGrounded)
    {
      // We are grounded, so recalculate
      // move direction directly from axes
      animator.SetBool("Jump", false);
      animator.SetBool("Fall", false);

      moveDirection = new Vector3(horizontal, 0.0f, vertical);
      moveDirection *= speed;

      if (hasMovement) {
        animator.SetBool("Move", true);
        animator.SetBool("Run", moveDirection.sqrMagnitude > runTolerance);
      } else {
        animator.SetBool("Move", false);
      }
      if (Input.GetButton("Jump"))
      {
        moveDirection.y = jumpSpeed;
        animator.SetBool("Jump", true);
      }
    
    } else {
      if (moveDirection.y < 0) {
        animator.SetBool("Fall", true);
      }
    }

    // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
    // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
    // as an acceleration (ms^-2)
    moveDirection.y -= gravity * Time.fixedDeltaTime;

    if (hasMovement) {
      // Rotate player when moving
      Vector3 lookDirection = new Vector3(horizontal, 0.0f, vertical).normalized;
      transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
    }

    // Move the player
    characterController.Move(moveDirection * Time.fixedDeltaTime);
  }
}
