using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
  private GameInputActions inputActions;
  private Vector3 direction;

  private CharacterController characterController;
  private Animator animator;
  public GameObject strikeEffect;
  public AudioClip[] swordAttackClips;
  public FixedJoystick joystick;

  public float jumpSpeed = 8.0f;
  public float gravity = 20.0f;

  public float acceleration = 15.0f;
  public float walkSpeed = 5.0f;
  public float runSpeed = 8.0f;
  public float joySpeed = 8.0f;
  public float runTolerance = 5.2f;
  public float rotationSpeed = 720.0f;

  private float currentSpeed = 0f;
  private bool isGrounded = true;
  private bool isRunning = false;

  private Vector3 directionFromInput = Vector3.zero;
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
    inputActions = new GameInputActions();
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
    inputActions.Gameplay.Attack.performed += OnAttack;
    inputActions.Gameplay.Teleport.performed += OnTeleport;
    inputActions.Gameplay.Teleport.canceled += OnTeleport;
    inputActions.Gameplay.PlacePortal.performed += OnPlacePortal;
    inputActions.Gameplay.PlacePortal.canceled += OnPlacePortal;
  }

  private void OnDisable()
  {
    inputActions.Gameplay.Move.performed -= OnMove;
    inputActions.Gameplay.Move.canceled -= OnMove;
    inputActions.Gameplay.Quit.performed -= OnQuit;
    inputActions.Gameplay.Run.performed -= OnRun;
    inputActions.Gameplay.Run.canceled -= OnRun;
    inputActions.Gameplay.Jump.performed -= OnJump;
    inputActions.Gameplay.Attack.performed -= OnAttack;
    inputActions.Gameplay.Teleport.performed -= OnTeleport;
    inputActions.Gameplay.Teleport.canceled -= OnTeleport;
    inputActions.Gameplay.PlacePortal.performed -= OnPlacePortal;
    inputActions.Gameplay.PlacePortal.canceled -= OnPlacePortal;

    inputActions.Gameplay.Disable();
  }

  private void OnMove(InputAction.CallbackContext context)
  {
    Vector2 input = context.ReadValue<Vector2>();
    directionFromInput = new Vector3(input.x, 0, input.y);
  }

  private void OnJump(InputAction.CallbackContext context)
  {
    if (isGrounded)
    {
      animator.SetBool("Jump", true);
      playerVelocity.y += jumpSpeed;
    }
  }

  private void OnAttack(InputAction.CallbackContext context)
  {
    TriggerAttack();
  }

  public void TriggerAttack()
  {
    if (isGrounded)
    {
      AttackWithSword();
    }
  }


  private void OnTeleport(InputAction.CallbackContext context)
  {
    TriggerTeleport(context.ReadValueAsButton());
  }

  public void TriggerTeleport(bool doTeleport)
  {
    if (!isGrounded)
    {
      return;
    }
    if (doTeleport)
    {
      Debug.Log("Teleporting...");
      TeleportEffect.teleporting = true;
    }
    else
    {
      TeleportEffect.teleporting = false;
    }
  }

  private void OnPlacePortal(InputAction.CallbackContext context)
  {
    TriggerPlacePortal(context.ReadValueAsButton());
  }

  public void TriggerPlacePortal(bool doPlace)
  {
    if (!isGrounded)
    {
      return;
    }
    if (doPlace)
    {
      Debug.Log("Placing portal...");
      TeleportEffect.settingUp = true;
    }
    else
    {
      TeleportEffect.settingUp = false;
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

  private Vector3 GetTargetDirection()
  {
    if (joystick == null)
    {
      return directionFromInput;
    }
    float joyHorizontal = joystick.Horizontal;
    float joyVertical = joystick.Vertical;
    if (joyHorizontal == 0f && joyVertical == 0f)
    {
      return directionFromInput;
    }
    return new Vector3(joyHorizontal, 0.0f, joyVertical);
  }

  private float GetMaxSpeed()
  {
    float joyHorizontal = joystick.Horizontal;
    float joyVertical = joystick.Vertical;
    if (joyHorizontal != 0f || joyVertical != 0f)
    {
      return joySpeed;
    }
    return isRunning ? runSpeed : walkSpeed;
  }

  private void FixedUpdate()
  {
    Vector3 targetDirection = GetTargetDirection();
    float maxSpeed = GetMaxSpeed();

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

  private void Update()
  {
    UpdateAnimator();
  }

  private void UpdateAnimator()
  {
    bool hasMovement = currentSpeed > 0.1f;

    if (animator.GetCurrentAnimatorStateInfo(1).IsName("Sword_Iai") ||
        animator.GetCurrentAnimatorStateInfo(1).IsName("Sword_Guard") ||
        animator.GetCurrentAnimatorStateInfo(1).IsName("Sword_Store"))
    {
      return;
    }

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

  private void AttackWithSword()
  {
    animator.SetTrigger("Slash");
    GameObject effect = Instantiate(strikeEffect);
    effect.transform.position = transform.position;
    effect.transform.rotation = transform.rotation;
    Destroy(effect, 3f);

    audioSource.PlayOneShot(swordAttackClips[Random.Range(0, swordAttackClips.Length)]);
  }
}
