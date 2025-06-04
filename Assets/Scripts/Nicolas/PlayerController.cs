using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem; 

public class PlayerController : MonoBehaviour
{
    // --- Variables públicas que puedes ajustar en el Inspector ---
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
    
    // --- Componentes ---
    private Animator anim;                   
    private Rigidbody2D rb;                  
    private GrappleLiana grappleLiana;       
    private WallJumpController wallJumpController; 

    // --- Variables de Estado Internas ---
    private bool isGrounded;                 
    private bool canAttack = true;           

    // --- Variables de Input ---
    private InputSystem_Actions inputActions; 
    private Vector2 moveInput;                
    private bool jumpPressedThisFrame;        
    private bool jumpHeld;                    
    private bool attackPressedThisFrame;      

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
        {
            Debug.LogWarning("PlayerController: No se encontró el componente WallJumpController. Las restricciones de movimiento del Wall Jump podrían no funcionar.");
        }
        if (grappleLiana == null)
        {
            Debug.LogWarning("PlayerController: No se encontró el componente GrappleLiana. La detección de suelo y animaciones podrían no funcionar correctamente.");
        }
    }

    void Update()
    {
        CheckGrounded();
        UpdateAnimations();

        if (attackPressedThisFrame && canAttack)
        {
            StartCoroutine(Attack());
        }
        attackPressedThisFrame = false; 
    }

    void FixedUpdate()
    {
        bool isHandlingWallJump = (wallJumpController != null && (wallJumpController.IsWallSliding || wallJumpController.IsWallJumping));
        bool attachedToLiana = (grappleLiana != null && grappleLiana.isAttached);

        if (isHandlingWallJump)
        {
            // WallJumpController toma el control
        }
        else if (attachedToLiana)
        {
            // Liana controla el movimiento
            HandleJump();
        }
        else
        {
            // Movimiento normal
            HandleMovement();
            HandleJump();
            ApplyVariableJumpForce(); 
        }

        jumpPressedThisFrame = false; 
    }

    /// <summary>
    /// Comprueba si el jugador está en el suelo.
    /// </summary>
    private void CheckGrounded()
    {
        bool attachedToLiana = (grappleLiana != null && grappleLiana.isAttached);
        isGrounded = attachedToLiana || Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    /// <summary>
    /// Actualiza los parámetros del Animator para controlar las animaciones.
    /// </summary>
    private void UpdateAnimations()
    {
        anim.SetBool("isGrounded", isGrounded);
        bool attachedToLiana = (grappleLiana != null && grappleLiana.isAttached);
        anim.SetBool("isAttachedAnim", attachedToLiana);

        bool isWallActionActive = (wallJumpController != null && (wallJumpController.IsWallSliding || wallJumpController.IsWallJumping));

        float walkValue = 0;
        if (!isWallActionActive && !attachedToLiana)
        {
            walkValue = Mathf.Abs(rb.linearVelocity.x);
        }
        anim.SetFloat("walk", walkValue, 0.2f, 0.2f); 

        float jumpValue = 0;
        if (!isWallActionActive && !attachedToLiana)
        {
            jumpValue = rb.linearVelocity.y;
        }
        anim.SetFloat("jump", jumpValue);

        if (!isWallActionActive)
        {
            if (moveInput.x > 0)
                transform.localScale = new Vector3(1f, 1f, 1f);
            else if (moveInput.x < 0)
                transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    /// <summary>
    /// Maneja el movimiento horizontal del jugador, adaptándose si es hijo de un ascensor.
    /// </summary>
    private void HandleMovement()
    {
        float targetSpeedX = moveInput.x * moveSpeed; 
        float currentYVelocity = rb.linearVelocity.y; 

        // Detecta si el jugador es hijo del ascensor (asumiendo que el ascensor tiene el Tag "Elevator").
        bool isParentedToElevator = (transform.parent != null && transform.parent.CompareTag("Elevator")); 

        if (isParentedToElevator)
        {
            // --- LÓGICA PARA EL MOVIMIENTO HORIZONTAL DEL JUGADOR EN PLATAFORMAS (Paternidad) ---

            // Cuando el jugador es hijo del ascensor, controlamos su movimiento horizontal
            // directamente usando su posición local (respecto al padre).
            // Esto evita que el Rigidbody2D "luche" contra la manipulación del transform del padre en X.
            
            Vector3 newLocalPosition = transform.localPosition;
            // Movemos la posición local en X. Usamos Time.fixedDeltaTime porque estamos en FixedUpdate.
            newLocalPosition.x += targetSpeedX * Time.fixedDeltaTime; 
            transform.localPosition = newLocalPosition;

            // Importantísimo: Reiniciar la velocidad X del Rigidbody.
            // Si el Rigidbody intentara moverse en X, lucharía contra transform.localPosition.
            rb.linearVelocity = new Vector2(0f, currentYVelocity); 
        }
        else
        {
            // Si el jugador NO es hijo de un ascensor, su movimiento horizontal es normal,
            // completamente controlado por el Rigidbody2D y su linearVelocity.
            rb.linearVelocity = new Vector2(targetSpeedX, currentYVelocity); 
        }
    }

    /// <summary>
    /// Maneja la acción de salto del jugador.
    /// </summary>
    private void HandleJump()
    {
        if (jumpPressedThisFrame)
        {
            if (grappleLiana != null && grappleLiana.isAttached)
            {
                grappleLiana.RemoveSwingJoint(); 
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * 0.7f); 
            }
            else if (isGrounded)
            {
                // Si el jugador es hijo de algún objeto (como el ascensor), se desparenta al saltar.
                if (transform.parent != null)
                {
                    transform.SetParent(null); 
                }
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); 
            }
        }
    }

    /// <summary>
    /// Aplica gravedad variable para un salto más controlable.
    /// </summary>
    private void ApplyVariableJumpForce()
    {
        if (rb.linearVelocity.y > 0.1f && !jumpHeld) 
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y < 0) 
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    /// <summary>
    /// Coroutine para la lógica de ataque.
    /// </summary>
    private IEnumerator Attack()
    {
        canAttack = false;
        anim.SetTrigger("attack");
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    // --- Propiedades Públicas para acceso desde otros scripts ---
    public bool IsGroundedPublic => isGrounded;         
    public float GetHorizontalInputPublic => moveInput.x; 
}