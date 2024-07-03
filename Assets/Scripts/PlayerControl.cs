using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
  private PlayerInputActions inputActions;
  private Vector3 direction;

  private CharacterController characterController;
  private Animator animator;
  public Camera playerCamera;
  public GameObject strikeEffect;
  public AudioClip[] swordAttackClips;

  public float jumpSpeed = 8.0f;
  public float gravity = 20.0f;

  public float acceleration = 15.0f;
  public float walkSpeed = 5.0f;
  public float runSpeed = 8.0f;
  public float runTolerance = 5.2f;
  public float rotationSpeed = 720.0f;

  public float currentSpeed = 0f;
  public bool isGrounded;
  private bool isRunning = false;

  private Vector3 targetDirection = Vector3.zero;
  private Vector3 movementDirection = Vector3.zero;
  public Vector3 playerVelocity = Vector3.zero;

  private AudioSource audioSource;

  private static GameObject _playerRef;

  public static GameObject GetPlayer()
  {
    return _playerRef;
  }

  private void Awake()
  {
    _playerRef = gameObject;
    inputActions = new PlayerInputActions();
    characterController = GetComponent<CharacterController>();
    animator = GetComponent<Animator>();
    audioSource = GetComponent<AudioSource>();
  }

  private void OnDestroy()
  {
    if (_playerRef == gameObject)
    {
      _playerRef = null;
    }
  }

  private void OnEnable()
  {
    inputActions.Gameplay.Enable();
    inputActions.Gameplay.Move.performed += OnMove;
    inputActions.Gameplay.Move.canceled += OnMove;
    inputActions.Gameplay.Quit.performed += OnQuit;
    inputActions.Gameplay.Run.performed += OnRun;
    inputActions.Gameplay.Run.canceled += OnRun;
    inputActions.Gameplay.Jump.performed += OnJump;
  }

  private void OnDisable()
  {
    inputActions.Gameplay.Move.performed -= OnMove;
    inputActions.Gameplay.Move.canceled -= OnMove;
    inputActions.Gameplay.Quit.performed -= OnQuit;
    inputActions.Gameplay.Run.performed -= OnRun;
    inputActions.Gameplay.Run.canceled -= OnRun;
    inputActions.Gameplay.Jump.performed -= OnJump;

    inputActions.Gameplay.Disable();
  }

  private void OnMove(InputAction.CallbackContext context)
  {
    Vector2 input = context.ReadValue<Vector2>();
    targetDirection = new Vector3(input.x, 0, input.y);
  }

  private void OnJump(InputAction.CallbackContext context)
  {
    if (isGrounded)
    {
      animator.SetBool("Jump", true);
      playerVelocity.y += jumpSpeed;
    }
  }

  private void FixedUpdate()
  {
    float maxSpeed = isRunning ? runSpeed : walkSpeed;


    // Smoothly interpolate the movement direction
    movementDirection = Vector3.Lerp(movementDirection, targetDirection, Time.fixedDeltaTime * acceleration);
    // Calculate the current speed based on the smoothed direction
    currentSpeed = Mathf.Lerp(currentSpeed, Mathf.Min(movementDirection.magnitude * maxSpeed, maxSpeed), Time.fixedDeltaTime * acceleration);
    if (currentSpeed < 0.1f)
    {
      currentSpeed = 0f;
    }
  
    bool hasMovement = currentSpeed > 0.1f;
    
    Vector3 move = movementDirection.normalized * currentSpeed;

    // Apply gravity
    playerVelocity.y -= gravity * Time.fixedDeltaTime;

    move.y = playerVelocity.y;
    // Move player. Only call this once each update!
    characterController.Move(move * Time.fixedDeltaTime);
    isGrounded = characterController.isGrounded;

    // Reset fall speed after player has moved
    if (isGrounded && playerVelocity.y < 0)
    {
      playerVelocity.y = 0f;
    }

    if (hasMovement)
    {
      // Rotate player towards movement
      transform.rotation = Quaternion.LookRotation(movementDirection, Vector3.up);
    }
  }

  private void Update() {
    bool hasMovement = currentSpeed > 0.1f;

    animator.SetBool("Grounded", isGrounded);

    if (isGrounded)
    {
      animator.SetBool("Jump", false);
      animator.SetBool("Fall", false);

      if (hasMovement)
      {
        animator.SetBool("Move", true);
        animator.SetBool("Run", currentSpeed > runTolerance);
      }
      else
      {
        animator.SetBool("Move", false);
      }
    }
    else
    {
      if (playerVelocity.y <= -0.1)
      {
        animator.SetBool("Fall", true);
        animator.SetBool("Jump", false);
      }
      else
      {
        animator.SetBool("Jump", true);
      }
    }
  }

  private void OnQuit(InputAction.CallbackContext context)
  {
    // Quit the game
    Application.Quit();
    #if UNITY_EDITOR
    // Stop playing the scene in the Unity Editor
    UnityEditor.EditorApplication.isPlaying = false;
    #endif
  }

  private void OnRun(InputAction.CallbackContext context)
  {
    isRunning = context.ReadValueAsButton();
  }

/*
  void OldFixedUpdate()
  {

    #region ACTION
    if (grounded)
    {
      if (Input.GetButtonDown("Fire"))
      {
        attackWithSword();
      }

      // animator.SetBool("Guard", Input.GetButton("Block"));

      if (Input.GetButtonDown("Teleport"))
      {
        Debug.Log("Teleporting...");
        TeleportEffect.teleporting = true;
      }
      if (Input.GetButtonUp("Teleport"))
      {
        TeleportEffect.teleporting = false;
      }

      if (Input.GetButtonDown("PlacePortal"))
      {
        TeleportEffect.settingUp = true;
      }
      if (Input.GetButtonUp("PlacePortal"))
      {
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

    // Move the player
    characterController.Move(moveDirection * speed * Time.fixedDeltaTime);
    characterController.Move(Vector3.up * verticalSpeed * Time.fixedDeltaTime);
    #endregion
  }
  */

  private void attackWithSword()
  {
    animator.SetTrigger("Slash");
    GameObject effect = Instantiate(strikeEffect);
    effect.transform.position = transform.position;
    effect.transform.rotation = transform.rotation;
    Destroy(effect, 3f);

    audioSource.PlayOneShot(swordAttackClips[Random.Range(0, swordAttackClips.Length)]);
  }
}
