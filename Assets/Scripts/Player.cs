using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
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

  private bool lastGrounded = false;

  void Start()
  {
    characterController = GetComponent<CharacterController>();
    animator = GetComponent<Animator>();
    Debug.Log("Start Player");
  }

  void FixedUpdate()
  {
    bool grounded = characterController.isGrounded;
    if (grounded != lastGrounded) {
      Debug.Log("Grounded: " + grounded);
      lastGrounded = grounded;
    }
    animator.SetBool("Grounded", grounded);

    #region ACTION
    if (grounded)
    {
      if (Input.GetButtonDown("Fire1"))
      {
        animator.SetTrigger("Slash");
      }
      animator.SetBool("Guard", Input.GetButton("Fire2"));

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

	bool CheckGrounded() {
    // Judge whether player is on the ground or not
    // Shoot ray at 0.05f upper from Junkochan's feet position to the ground with its length of 0.1f
		Ray ray = new Ray(this.transform.position + Vector3.up * 0.05f, Vector3.down * 0.1f);
    // If the ray hit the ground, return true
		return Physics.Raycast(ray, 0.1f);
	}

}
