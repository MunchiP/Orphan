using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public float lowJumpMultiplier = 2f;
    public float fallMultiplier = 2.5f;

    [Header("Ataque")]
    public float attackCooldown = 0.5f;

    private Animator anim;
    private Rigidbody2D rb;
    private GrappleLiana grappleLiana;
    private WallJumpController wallJumpController;

    public bool isGrounded;
    private bool canAttack = true;

    private InputSystem_Actions inputActions;
    private Vector2 moveInput;
    private bool jumpPressedThisFrame;
    private bool jumpHeld;
    private bool attackPressedThisFrame;

    public bool isKnockedBack { get; set; } = false; // ← Nueva bandera pública

    void Awake()
    {
        inputActions = new InputSystem_Actions();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Player.Jump.performed += ctx => jumpPressedThisFrame = true;
        inputActions.Player.Jump.started += ctx => jumpHeld = true;
        inputActions.Player.Jump.canceled += ctx => jumpHeld = false;

        inputActions.Player.Attack.performed += ctx => attackPressedThisFrame = true;
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        grappleLiana = GetComponent<GrappleLiana>();
        wallJumpController = GetComponent<WallJumpController>();

        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        if (wallJumpController == null)
            Debug.LogWarning("No se encontró WallJumpController.");
        if (grappleLiana == null)
            Debug.LogWarning("No se encontró GrappleLiana.");
    }

    void Update()
    {
        CheckGrounded();
        UpdateAnimations();

        if (attackPressedThisFrame && canAttack && !isKnockedBack)
            StartCoroutine(Attack());

        attackPressedThisFrame = false;
    }

    void FixedUpdate()
    {
        if (isKnockedBack) return;

        bool isHandlingWallJump = (wallJumpController != null && (wallJumpController.IsWallSliding || wallJumpController.IsWallJumping));
        bool attachedToLiana = (grappleLiana != null && grappleLiana.isAttached);

        if (isHandlingWallJump)
        {
            // WallJumpController toma el control
        }
        else if (attachedToLiana)
        {
            HandleJump();
        }
        else
        {
            HandleMovement();
            HandleJump();
            ApplyVariableJumpForce();
        }

        jumpPressedThisFrame = false;
    }

    private void CheckGrounded()
    {
        bool attachedToLiana = (grappleLiana != null && grappleLiana.isAttached);
        isGrounded = attachedToLiana || Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void UpdateAnimations()
    {
        anim.SetBool("isGrounded", isGrounded);
        bool attachedToLiana = (grappleLiana != null && grappleLiana.isAttached);
        anim.SetBool("isAttachedAnim", attachedToLiana);

        bool isWallActionActive = (wallJumpController != null && (wallJumpController.IsWallSliding || wallJumpController.IsWallJumping));

        float walkValue = 0;
        if (!isWallActionActive && !attachedToLiana && !isKnockedBack)
            walkValue = Mathf.Abs(rb.linearVelocity.x);
        anim.SetFloat("walk", walkValue, 0.2f, 0.2f);

        float jumpValue = 0;
        if (!isWallActionActive && !attachedToLiana)
            jumpValue = rb.linearVelocity.y;
        anim.SetFloat("jump", jumpValue);

        if (!isWallActionActive && !isKnockedBack)
        {
            if (moveInput.x > 0)
                transform.localScale = new Vector3(1f, 1f, 1f);
            else if (moveInput.x < 0)
                transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    private void HandleMovement()
    {
        if (isKnockedBack) return;

        float targetSpeedX = moveInput.x * moveSpeed;
        float currentYVelocity = rb.linearVelocity.y;

        bool isParentedToElevator = (transform.parent != null && transform.parent.CompareTag("Elevator"));

        if (isParentedToElevator)
        {
            Vector3 newLocalPosition = transform.localPosition;
            newLocalPosition.x += targetSpeedX * Time.fixedDeltaTime;
            transform.localPosition = newLocalPosition;

            rb.linearVelocity = new Vector2(0f, currentYVelocity);
        }
        else
        {
            rb.linearVelocity = new Vector2(targetSpeedX, currentYVelocity);
        }
    }

    private void HandleJump()
    {
        if (isKnockedBack) return;

        if (jumpPressedThisFrame)
        {
            if (grappleLiana != null && grappleLiana.isAttached)
            {
                grappleLiana.RemoveSwingJoint();
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * 0.7f);
            }
            else if (isGrounded)
            {
                if (transform.parent != null)
                    transform.SetParent(null);

                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
        }
    }

    private void ApplyVariableJumpForce()
    {
        if (isKnockedBack) return;

        if (rb.linearVelocity.y > 0.1f && !jumpHeld)
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        else if (rb.linearVelocity.y < 0)
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
    }

    private IEnumerator Attack()
    {
        canAttack = false;
        anim.SetBool("isAttacking", true);
        anim.SetTrigger("attack");
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    public bool IsGroundedPublic => isGrounded;
    public float GetHorizontalInputPublic => moveInput.x;
}
