using UnityEngine;

public class WallJumpController : MonoBehaviour
{
    [Header("Componentes")]
    private Rigidbody2D rb;
    private PlayerController playerController; 
    private Animator animator; 

    [Header("Configuración Wall Jump")]
    public float wallSlideSpeed = 0.5f; 
    public float wallJumpForceX = 8f; 
    public float wallJumpForceY = 15f; 
    public float wallCheckDistance = 0.6f; 
    public LayerMask wallLayer; 

    [Header("Temporizadores")]
    public float wallJumpCooldown = 0.2f; // Tiempo mínimo entre saltos de pared
    private float lastWallJumpTime;
    public float wallJumpingDuration = 0.4f; // Duración del estado de "wall jumping"

    // Variables de estado
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool isWallJumping; 
    private int wallDirection; // 1 para derecha, -1 para izquierda

    // Variables para evitar doble salto en la misma pared
    private GameObject lastWallJumpedFrom; 

    private float currentHorizontalInput; 

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>(); 
        animator = GetComponentInChildren<Animator>(); 
    }

    void Update()
    {
        if (playerController == null || animator == null) return; 

        // Si el personaje está en el suelo, reinicia los estados de pared
        if (playerController.IsGroundedPublic)
        {
            lastWallJumpedFrom = null; 
            isTouchingWall = false; 
            isWallSliding = false;
            isWallJumping = false; 
        }
        else // Si no está en el suelo, verifica la interacción con la pared
        {
            currentHorizontalInput = playerController.GetHorizontalInputPublic; 
            
            CheckForWall();
            HandleWallSliding();
        }

        HandleWallJumpInput(); 

        // ¡Actualiza el parámetro del Animator!
        animator.SetBool("isWallSliding", isWallSliding); 
    }

    /// <summary>
    /// Verifica si el personaje está tocando una pared usando Raycasts.
    /// Determina la dirección de la pared (izquierda o derecha).
    /// </summary>
    private void CheckForWall()
    {
        isTouchingWall = false;
        wallDirection = 0;

        // Raycast hacia la derecha
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, wallCheckDistance, wallLayer);
        Debug.DrawRay(transform.position, Vector2.right * wallCheckDistance, Color.blue);

        if (hitRight.collider != null && hitRight.collider.gameObject != lastWallJumpedFrom)
        {
            isTouchingWall = true;
            wallDirection = 1; // Pared a la derecha
        }

        // Si no se detectó pared a la derecha o si también hay una pared a la izquierda
        if (!isTouchingWall) 
        {
            RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, wallCheckDistance, wallLayer);
            Debug.DrawRay(transform.position, Vector2.left * wallCheckDistance, Color.red);

            if (hitLeft.collider != null && hitLeft.collider.gameObject != lastWallJumpedFrom)
            {
                isTouchingWall = true;
                wallDirection = -1; // Pared a la izquierda
            }
        }
    }

    /// <summary>
    /// Maneja el comportamiento de deslizamiento por la pared.
    /// Limita la velocidad vertical y decide el movimiento horizontal (pegarse/despegarse).
    /// </summary>
    private void HandleWallSliding()
    {
        // Si está tocando una pared, no está en el suelo y está cayendo (o su velocidad vertical es 0)
        if (isTouchingWall && !playerController.IsGroundedPublic && rb.linearVelocity.y <= 0)
        {
            isWallSliding = true;
            
            // Limita la velocidad vertical para el deslizamiento por la pared
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -wallSlideSpeed));

            // Si el jugador está intentando alejarse de la pared
            bool isMovingAwayFromWall = (wallDirection == 1 && currentHorizontalInput < -0.05f) || 
                                         (wallDirection == -1 && currentHorizontalInput > 0.05f);

            if (!isMovingAwayFromWall)
            {
                // Si el jugador no está intentando alejarse (presiona hacia la pared o no hay input),
                // se pega a la pared (velocidad horizontal a cero).
                rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            }
            else // if (isMovingAwayFromWall)
            {
                // Si el jugador intenta alejarse de la pared, aplica su velocidad de movimiento normal
                // para que se "despegue".
                // Usamos playerController.moveSpeed para la velocidad de movimiento normal.
                rb.linearVelocity = new Vector2(currentHorizontalInput * playerController.moveSpeed, rb.linearVelocity.y);
            }
        }
        else
        {
            isWallSliding = false;
        }
    }

    /// <summary>
    /// Maneja la entrada del salto de pared.
    /// El salto se realiza en la dirección opuesta a la pared detectada.
    /// </summary>
    private void HandleWallJumpInput()
    {
        bool jumpButtonPressed = Input.GetButtonDown("Jump");
        bool cooldownIsReady = Time.time >= lastWallJumpTime + wallJumpCooldown;
        
        // --- DEBUGGING WALL JUMP DELAY ---
        // Activa estas líneas para ver qué está pasando.
        // Desactívalas (comenta o borra) cuando hayas encontrado la causa.
        // Debug.Log($"[WallJumpInput] isWallSliding: {isWallSliding} | JumpButton: {jumpButtonPressed} | CooldownReady: {cooldownIsReady} | Time Remaining: {Mathf.Max(0, (lastWallJumpTime + wallJumpCooldown) - Time.time):F2}s");

        // if (jumpButtonPressed)
        // {
        //     if (!isWallSliding)
        //     {
        //         Debug.LogWarning("[WallJumpInput] Saltó (Espacio) pero NO está deslizándose por la pared. Posiblemente no ha tocado la pared aún, o no está marcado como 'isWallSliding'.");
        //     }
        //     if (!cooldownIsReady)
        //     {
        //         Debug.LogWarning($"[WallJumpInput] Saltó (Espacio) pero COOLDOWN ACTIVO. Restan: {Mathf.Max(0, (lastWallJumpTime + wallJumpCooldown) - Time.time):F2}s. El último salto fue hace: {Time.time - lastWallJumpTime:F2}s");
        //     }
        // }
        // --- FIN DEBUGGING ---

        // Si está deslizándose por la pared, presiona el botón de salto y ha pasado el cooldown
        if (isWallSliding && jumpButtonPressed && cooldownIsReady)
        {
            // Debug.Log("--- WALL JUMP EJECUTADO! ---"); // Mensaje cuando el wall jump SÍ se ejecuta

            // Guarda la pared de la que se saltó para evitar doble salto en la misma pared
            RaycastHit2D currentWallHitRight = Physics2D.Raycast(transform.position, Vector2.right, wallCheckDistance, wallLayer);
            RaycastHit2D currentWallHitLeft = Physics2D.Raycast(transform.position, Vector2.left, wallCheckDistance, wallLayer);

            if (currentWallHitRight.collider != null)
            {
                lastWallJumpedFrom = currentWallHitRight.collider.gameObject;
            }
            else if (currentWallHitLeft.collider != null)
            {
                lastWallJumpedFrom = currentWallHitLeft.collider.gameObject;
            }

            // Aplica la fuerza del salto de pared en la dirección opuesta a la pared
            rb.linearVelocity = new Vector2(-wallDirection * wallJumpForceX, wallJumpForceY);
            
            // Voltea el sprite del personaje para que mire en la dirección del salto
            Vector3 newScale = transform.localScale;
            newScale.x = -wallDirection; 
            transform.localScale = newScale;

            isWallJumping = true; 
            Invoke(nameof(StopWallJumping), wallJumpingDuration); 
            lastWallJumpTime = Time.time; 
        }
    }

    /// <summary>
    /// Función llamada para detener el estado de "wall jumping".
    /// </summary>
    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    /// <summary>
    /// Dibuja Gizmos en el editor para visualizar los Raycasts de detección de pared.
    /// </summary>
    void OnDrawGizmos()
    {
        if (rb == null) return;

        if (Application.isPlaying)
        {
            // Dibuja los Raycasts en tiempo de ejecución según la dirección del input o velocidad
            if (currentHorizontalInput > 0.05f || rb.linearVelocity.x > 0.05f)
            {
                Gizmos.color = Color.blue; 
                Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * wallCheckDistance);
            }
            if (currentHorizontalInput < -0.05f || rb.linearVelocity.x < -0.05f)
            {
                Gizmos.color = Color.red; 
                Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.left * wallCheckDistance);
            }
        }
        else 
        {
            // Dibuja los Raycasts en el editor para configuración
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * wallCheckDistance);
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.left * wallCheckDistance);
        }
    }

    // Propiedades públicas para acceder al estado desde otros scripts
    public bool IsWallSliding => isWallSliding; 
    public bool IsWallJumping => isWallJumping; 
}