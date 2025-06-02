using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem; // Necesario para usar el nuevo sistema de Input de Unity.

public class PlayerController : MonoBehaviour
{
    // --- Variables de Input ---
    private InputSystem_Actions inputActions; // Referencia al Asset de Input Actions.
    private Vector2 moveInput;               // Almacena la entrada de movimiento (horizontal y vertical).
    private bool jumpPressedThisFrame;       // Indica si el botón de salto fue presionado en el frame actual.
    private bool attackPressedThisFrame;     // Indica si el botón de ataque fue presionado en el frame actual.

    [Header("Componentes")]
    private Animator anim;                   // Componente Animator para controlar animaciones.
    private Rigidbody2D rb;                  // Componente Rigidbody2D para la física del jugador.
    private GrappleLiana grappleLiana;       // Script para el manejo de la liana.
    private WallJumpController wallJumpController; // Script para el manejo del salto de pared.

    [Header("Movimiento")]
    public float moveSpeed = 5f;             // Velocidad de movimiento horizontal.
    public float jumpForce = 12f;            // Fuerza aplicada al saltar.
    public Transform groundCheck;            // Objeto para verificar la posición del suelo.
    public LayerMask groundLayer;            // Capa que define el "suelo".
    public float groundCheckRadius = 0.1f;   // Radio de detección del suelo.

    [Header("Ataque")]
    public float attackCooldown = 1f;        // Tiempo de espera entre ataques.
    private bool canAttack = true;           // Controla si el jugador puede atacar.

    // --- Variables de Estado Internas ---
    private bool isGrounded;                 // Indica si el jugador está en el suelo.

    /// <summary>
    /// Se llama cuando el script se carga. Ideal para inicializar el Input System.
    /// </summary>
    void Awake()
    {
        inputActions = new InputSystem_Actions(); // Instancia el Asset de Input Actions.

        // Suscribe los eventos de las acciones de Input.
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Player.Jump.performed += ctx => jumpPressedThisFrame = true;
        inputActions.Player.Attack.performed += ctx => attackPressedThisFrame = true;
    }

    /// <summary>
    /// Habilita el Action Map del jugador cuando el GameObject está activo.
    /// </summary>
    void OnEnable()
    {
        inputActions.Player.Enable();
    }

    /// <summary>
    /// Deshabilita el Action Map del jugador cuando el GameObject se desactiva.
    /// </summary>
    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    /// <summary>
    /// Se llama una vez al inicio del juego. Obtiene referencias a componentes.
    /// </summary>
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        grappleLiana = GetComponent<GrappleLiana>();
        wallJumpController = GetComponent<WallJumpController>();

        // Advertencias si faltan componentes cruciales.
        if (wallJumpController == null)
        {
            Debug.LogWarning("PlayerController: No se encontró el componente WallJumpController. Las restricciones de movimiento del Wall Jump podrían no funcionar.");
        }
        if (grappleLiana == null)
        {
            Debug.LogWarning("PlayerController: No se encontró el componente GrappleLiana. La detección de suelo y animaciones podrían no funcionar correctamente.");
        }
    }

    /// <summary>
    /// Se llama una vez por frame. Lógica no física.
    /// </summary>
    void Update()
    {
        CheckGrounded();
        UpdateAnimations();

        // Manejo del ataque (con cooldown).
        if (attackPressedThisFrame && canAttack)
        {
            StartCoroutine(Attack());
        }
        attackPressedThisFrame = false; // Reinicia la bandera de ataque.
    }

    /// <summary>
    /// Se llama a intervalos fijos. Ideal para lógica de física y movimiento.
    /// </summary>
    void FixedUpdate()
    {
        bool isHandlingWallJump = (wallJumpController != null && (wallJumpController.IsWallSliding || wallJumpController.IsWallJumping));
        bool attachedToLiana = (grappleLiana != null && grappleLiana.isAttached);

        // Lógica de prioridad de movimiento: Wall Jump > Liana > Movimiento normal.
        if (isHandlingWallJump)
        {
            // El WallJumpController toma el control total de la física.
        }
        else if (attachedToLiana)
        {
            // Si está enganchado a la liana, solo se ejecuta la lógica de salto (para desenganchar).
            HandleJump();
        }
        else
        {
            // Movimiento y salto normal.
            HandleMovement();
            HandleJump();
        }

        // Reinicia la bandera de salto después de procesar la física.
        jumpPressedThisFrame = false;
    }

    /// <summary>
    /// Verifica si el jugador está en el suelo o enganchado a la liana.
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

        // Animación de "caminar" solo si no está en acción de pared ni en liana.
        float walkValue = 0;
        if (!isWallActionActive && !attachedToLiana)
        {
            walkValue = Mathf.Abs(rb.linearVelocity.x);
        }
        anim.SetFloat("walk", walkValue, 0.2f, 0.2f);

        // Animación de "salto" solo si no está en acción de pared ni en liana.
        float jumpValue = 0;
        if (!isWallActionActive && !attachedToLiana)
        {
            jumpValue = rb.linearVelocity.y;
        }
        anim.SetFloat("jump", jumpValue);

        // Volteo del sprite según la dirección del movimiento, si no está en acción de pared.
        if (!isWallActionActive)
        {
            if (moveInput.x > 0)
                transform.localScale = new Vector3(1f, 1f, 1f);
            else if (moveInput.x < 0)
                transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    /// <summary>
    /// Maneja el movimiento horizontal del jugador.
    /// </summary>
    private void HandleMovement()
    {
        float targetYVelocity = rb.linearVelocity.y;

        // Si está en el suelo (no por liana) y su velocidad vertical es mínima, la fija en 0.
        if (isGrounded && !(grappleLiana != null && grappleLiana.isAttached) && rb.linearVelocity.y <= 0.1f)
        {
            targetYVelocity = 0;
        }

        // Aplica el movimiento horizontal usando el input del sistema de Input Actions.
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, targetYVelocity);
    }

    /// <summary>
    /// Maneja la acción de salto del jugador, incluyendo el desenganche de la liana.
    /// </summary>
    private void HandleJump()
    {
        // Solo permite el salto si la bandera 'jumpPressedThisFrame' está activa.
        if (jumpPressedThisFrame)
        {
            // Primero, comprueba si estamos enganchados a la liana.
            if (grappleLiana != null && grappleLiana.isAttached)
            {
                grappleLiana.RemoveSwingJoint(); // ¡Llama al método para desenganchar la liana!
                // Aplica un pequeño impulso al Rigidbody al soltar la liana.
                // Puedes ajustar el multiplicador (ej. 0.7f) para la fuerza del salto al desenganchar.
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * 0.7f); 
            }
            // Si no estamos enganchados Y estamos en el suelo, realiza un salto normal.
            else if (isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
        }
    }

    /// <summary>
    /// Coroutine para el ciclo de ataque (animación y cooldown).
    /// </summary>
    private IEnumerator Attack()
    {
        canAttack = false;
        anim.SetTrigger("attack");
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    // --- Propiedades Públicas para Acceso Externo ---
    public bool IsGroundedPublic => isGrounded;          // Devuelve si el jugador está en el suelo.
    public float GetHorizontalInputPublic => moveInput.x; // Devuelve el input horizontal actual.
}