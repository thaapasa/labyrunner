using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
  private PlayerInputActions inputActions;
  private Vector3 direction;

  CharacterController characterController;
  Animator animator;
  public Camera playerCamera;
  public GameObject strikeEffect;
  public AudioClip[] swordAttackClips;


  public float speed = 6.0f;
  public float jumpSpeed = 8.0f;
  public float gravity = 20.0f;

  public float runTolerance = 5f;

  private float verticalSpeed = 0f;

  private Vector3 moveDirection = Vector3.zero;

  private AudioSource audioSource;

  private static GameObject _playerRef;

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
  }

  private void OnDisable()
  {
    inputActions.Gameplay.Move.performed -= OnMove;
    inputActions.Gameplay.Move.canceled -= OnMove;
    inputActions.Gameplay.Quit.performed -= OnQuit;
    inputActions.Gameplay.Disable();
  }

  public static GameObject GetPlayer()
  {
    return _playerRef;
  }

  private void OnMove(InputAction.CallbackContext context)
  {
    Vector2 input = context.ReadValue<Vector2>();
    direction = new Vector3(input.x, 0, input.y);
  }

  private void Update()
  {
    characterController.Move(direction * Time.deltaTime);
  }

  public Vector3 GetInputTranslationDirection()
  {
    return direction;
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

  void OldFixedUpdate()
  {
    bool grounded = characterController.isGrounded;
    animator.SetBool("Grounded", grounded);

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
