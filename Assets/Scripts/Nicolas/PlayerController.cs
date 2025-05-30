using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Componentes")]
    private Animator anim;
    private Rigidbody2D rb;
    private GrappleLiana grappleLiana; 
    private WallJumpController wallJumpController; 

    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;

    [Header("Ataque")]
    public float attackCooldown = 1f;
    private bool canAttack = true;

    // Variables de estado
    private float horizontalInput; 
    private bool isGrounded; 
    private bool jumpInputReceived; // Para almacenar la entrada del salto de forma fiable

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        grappleLiana = GetComponent<GrappleLiana>(); 
        wallJumpController = GetComponent<WallJumpController>(); 

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
        HandleInput();
        CheckGrounded();
        UpdateAnimations();
    }

    void FixedUpdate()
    {
        // Determina si el WallJumpController está activo (deslizándose o saltando de pared)
        bool isHandlingWallJump = (wallJumpController != null && 
                                   (wallJumpController.IsWallSliding || wallJumpController.IsWallJumping));

        // Determina si está enganchado a la liana
        bool attachedToLiana = (grappleLiana != null && grappleLiana.isAttached);

        // --- Lógica de Prioridad de Movimiento ---
        if (isHandlingWallJump)
        {
            // Prioridad 1: WallJumpController toma el control total de la física.
            // El PlayerController no hace nada en este FixedUpdate.
        }
        else if (attachedToLiana)
        {
            // Prioridad 2: Si está enganchado a la liana.
            // Se espera que el script GrappleLiana maneje el movimiento horizontal (balanceo).
            // Solo permitimos que el PlayerController maneje el salto (por si es un "salto para soltar").
            HandleJump(); 
        }
        else // Prioridad 3: Movimiento normal del jugador (cuando no está en pared ni en liana).
        {
            HandleMovement();
            HandleJump();
        }
        
        // ¡IMPORTANTE! Reinicia la variable de salto al final de FixedUpdate
        // para que solo se use una vez por pulsación, previniendo múltiples saltos por un solo toque.
        jumpInputReceived = false; 
    }

    private void HandleInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal"); 
        
        // Captura la entrada de salto en Update para mayor fiabilidad.
        if (Input.GetButtonDown("Jump"))
            jumpInputReceived = true;

        if (Input.GetMouseButtonDown(0) && canAttack)
            StartCoroutine(Attack());
    }

    private void CheckGrounded()
    {
        bool attachedToLiana = (grappleLiana != null && grappleLiana.isAttached);
        // isGrounded es true si está en el suelo O si está enganchado a la liana.
        isGrounded = attachedToLiana || Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void UpdateAnimations()
    {
        anim.SetBool("isGrounded", isGrounded);
        bool attachedToLiana = (grappleLiana != null && grappleLiana.isAttached);
        anim.SetBool("isAttachedAnim", attachedToLiana);
        
        bool isWallActionActive = (wallJumpController != null && 
                                   (wallJumpController.IsWallSliding || wallJumpController.IsWallJumping));

        // "walk" animation: solo si NO está en una acción de pared Y NO está atado a la liana
        float walkValue = 0;
        if (!isWallActionActive && !attachedToLiana) 
        {
            walkValue = Mathf.Abs(rb.linearVelocity.x);
        }
        anim.SetFloat("walk", walkValue, 0.2f, 0.2f);
        
        // "jump" animation: resetear si está en una acción de pared O está atado a la liana
        float jumpValue = 0;
        if (!isWallActionActive && !attachedToLiana) 
        {
            jumpValue = rb.linearVelocity.y;
        }
        anim.SetFloat("jump", jumpValue);

        // --- Volteo del Sprite ---
        // El PlayerController solo voltea el sprite si NO está en una acción de pared.
        // Permite volteo si está en liana.
        if (!isWallActionActive)
        {
            if (horizontalInput > 0)
                transform.localScale = new Vector3(1f, 1f, 1f);
            else if (horizontalInput < 0)
                transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    private void HandleMovement()
    {
        float targetYVelocity = rb.linearVelocity.y;
        
        // Si está en el suelo (y no solo por la liana) y su velocidad vertical es muy baja
        if (isGrounded && !(grappleLiana != null && grappleLiana.isAttached) && rb.linearVelocity.y <= 0.1f) 
        {
            targetYVelocity = 0;
        }

        // Aplica el movimiento horizontal. Este método SOLO se llama cuando no está en pared ni en liana.
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, targetYVelocity);
    }

    private void HandleJump()
    {
        // Ahora verifica la variable jumpInputReceived, que fue establecida en Update.
        // Solo permite el salto normal si está en el suelo Y NO está atado a la liana.
        if (jumpInputReceived && isGrounded && (grappleLiana == null || !grappleLiana.isAttached))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            // jumpInputReceived = false; // Se reinicia al final de FixedUpdate
        } 
    }

    private IEnumerator Attack()
    {
        canAttack = false;
        anim.SetTrigger("attack");
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    // Propiedades públicas para que otros scripts puedan leer el estado del jugador.
    public bool IsGroundedPublic => isGrounded; 
    public float GetHorizontalInputPublic => horizontalInput; 
}