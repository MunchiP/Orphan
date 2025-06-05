using UnityEngine;
using UnityEngine.InputSystem; // Necesario para usar el nuevo sistema de Input.

[RequireComponent(typeof(Rigidbody2D))]
public class WallJumpController : MonoBehaviour
{
    // --- Referencia al Input System ---
    private InputSystem_Actions inputActions; // Instancia de nuestro Input Action Asset.
    private bool jumpPressedThisFrame;        // Bandera para capturar la pulsación del salto.

    [Header("Componentes")]
    private Rigidbody2D rb;
    private PlayerController playerController;
    private Animator animator;

    [Header("Configuración Wall Jump")]
    public float wallSlideSpeed = 0.5f;     // Velocidad máxima de deslizamiento por la pared.
    public float wallJumpForceX = 8f;       // Fuerza horizontal del salto de pared.
    public float wallJumpForceY = 15f;      // Fuerza vertical del salto de pared.
    public float wallCheckDistance = 0.6f;  // Distancia para detectar una pared.
    public LayerMask wallLayer;             // Capa que define qué es una "pared".

    [Header("Temporizadores")]
    public float wallJumpCooldown = 0.2f;   // Tiempo mínimo entre saltos de pared consecutivos.
    private float lastWallJumpTime;         // Momento del último salto de pared.
    public float wallJumpingDuration = 0.4f; // Duración del estado de "wall jumping" (control de movimiento).

    // Variables de estado
    private bool isTouchingWall;            // True si el personaje está tocando una pared.
    private bool isWallSliding;             // True si el personaje está deslizándose por una pared.
    private bool isWallJumping;             // True si el personaje acaba de realizar un salto de pared.
    private int wallDirection;              // 1 para pared a la derecha, -1 para pared a la izquierda.

    // Variables para evitar doble salto en la misma pared
    private GameObject lastWallJumpedFrom;  // Referencia a la última pared de la que se saltó.

    private float currentHorizontalInput;   // Guarda el input horizontal del PlayerController.

    /// <summary>
    /// Se llama cuando el script se carga. Inicializa componentes y suscribe eventos de input.
    /// </summary>
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
        animator = GetComponentInChildren<Animator>();

        // Inicializa el Input Action Asset.
        inputActions = new InputSystem_Actions();
        // Suscribe el evento 'performed' de la acción de salto.
        inputActions.Player.Jump.performed += ctx => jumpPressedThisFrame = true;
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
    /// Se llama una vez por frame. Maneja la detección de pared y el deslizamiento.
    /// </summary>
    void Update()
    {
        // Verifica que los componentes necesarios estén asignados.
        if (playerController == null || animator == null) return;

        // Si el personaje está en el suelo, reinicia todos los estados de pared.
        if (playerController.IsGroundedPublic)
        {
            lastWallJumpedFrom = null;  // Permite volver a saltar de la misma pared.
            isTouchingWall = false;
            isWallSliding = false;
            isWallJumping = false;
        }
        else // Si no está en el suelo, verifica la interacción con la pared.
        {
            // Obtiene el input horizontal del PlayerController.
            currentHorizontalInput = playerController.GetHorizontalInputPublic;

            CheckForWall();         // Detecta si está tocando una pared.
            HandleWallSliding();    // Controla el comportamiento de deslizamiento.
        }

        HandleWallJumpInput(); // Maneja la lógica de salto de pared.

        // Reinicia la bandera de salto después de usarla, para que solo se active una vez por pulsación.
        jumpPressedThisFrame = false;

        // Actualiza el parámetro "isWallSliding" del Animator.
        animator.SetBool("isWallSliding", isWallSliding);
    }

    /// <summary>
    /// Verifica si el personaje está tocando una pared usando Raycasts.
    /// Determina la dirección de la pared (izquierda o derecha).
    /// </summary>
    private void CheckForWall()
    {
        isTouchingWall = false; // Por defecto, no está tocando una pared.
        wallDirection = 0;      // Por defecto, no hay dirección de pared.

        // Raycast hacia la derecha para detectar paredes.
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, wallCheckDistance, wallLayer);
        Debug.DrawRay(transform.position, Vector2.right * wallCheckDistance, Color.blue);

        // Si se detecta una pared a la derecha y no es la pared de la que acabamos de saltar.
        if (hitRight.collider != null && hitRight.collider.gameObject != lastWallJumpedFrom)
        {
            isTouchingWall = true;
            wallDirection = 1; // Pared a la derecha.
        }

        // Si no se detectó pared a la derecha o si también hay una pared a la izquierda.
        // Esto permite detectar paredes a ambos lados si el jugador está entre ellas.
        if (!isTouchingWall)
        {
            // Raycast hacia la izquierda para detectar paredes.
            RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, wallCheckDistance, wallLayer);
            Debug.DrawRay(transform.position, Vector2.left * wallCheckDistance, Color.red);

            // Si se detecta una pared a la izquierda y no es la pared de la que acabamos de saltar.
            if (hitLeft.collider != null && hitLeft.collider.gameObject != lastWallJumpedFrom)
            {
                isTouchingWall = true;
                wallDirection = -1; // Pared a la izquierda.
            }
        }
    }

    /// <summary>
    /// Maneja el comportamiento de deslizamiento por la pared.
    /// Limita la velocidad vertical y ajusta el movimiento horizontal.
    /// </summary>
    private void HandleWallSliding()
    {
        // El deslizamiento por la pared ocurre si:
        // - Está tocando una pared.
        // - No está en el suelo.
        // - Está cayendo (o su velocidad vertical es muy baja/cero, para iniciar el deslizamiento).
        if (isTouchingWall && !playerController.IsGroundedPublic && rb.linearVelocity.y <= 0)
        {
            isWallSliding = true;

            // Limita la velocidad vertical del Rigidbody para simular deslizamiento.
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -wallSlideSpeed));

            // Determina si el jugador está intentando alejarse de la pared.
            bool isMovingAwayFromWall = (wallDirection == 1 && currentHorizontalInput < -0.05f) ||
                                        (wallDirection == -1 && currentHorizontalInput > 0.05f);

            if (!isMovingAwayFromWall)
            {
                // Si el jugador se está pegando a la pared (o no hay input horizontal),
                // la velocidad horizontal se fuerza a cero para que se "adhiera".
                rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            }
            else // Si el jugador intenta alejarse de la pared.
            {
                // Permite que el jugador se "despegue" aplicando su velocidad de movimiento normal.
                rb.linearVelocity = new Vector2(currentHorizontalInput * playerController.moveSpeed, rb.linearVelocity.y);
            }
        }
        else
        {
            // Si no cumple las condiciones, no está deslizándose por la pared.
            isWallSliding = false;
        }
    }

    /// <summary>
    /// Maneja la lógica para realizar un salto de pared.
    /// </summary>
    private void HandleWallJumpInput()
    {
        // Comprueba si se ha presionado el botón de salto y si el cooldown ha terminado.
        bool cooldownIsReady = Time.time >= lastWallJumpTime + wallJumpCooldown;

        // El salto de pared se ejecuta si:
        // - Está deslizándose por una pared (`isWallSliding`).
        // - Se presionó el botón de salto en este frame (`jumpPressedThisFrame`).
        // - El cooldown del salto de pared ha terminado.
        if (isWallSliding && jumpPressedThisFrame && cooldownIsReady)
        {
            // Guarda la pared de la que se saltó para evitar saltos repetidos en la misma pared.
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

            // Aplica la fuerza del salto de pared.
            // La fuerza horizontal se aplica en la dirección opuesta a la pared (`-wallDirection`).
            rb.linearVelocity = new Vector2(-wallDirection * wallJumpForceX, wallJumpForceY);

            // Voltea el sprite del personaje para que mire en la dirección del salto.
            Vector3 newScale = transform.localScale;
            newScale.x = -wallDirection;
            transform.localScale = newScale;

            isWallJumping = true; // Establece el estado de "wall jumping".
            // Usa Invoke para llamar a StopWallJumping después de un tiempo,
            // permitiendo que el jugador tenga control total durante el "wall jumping"
            // y evitando que se pegue a la pared inmediatamente.
            Invoke(nameof(StopWallJumping), wallJumpingDuration);
            lastWallJumpTime = Time.time; // Registra el momento del salto.
        }
    }

    /// <summary>
    /// Detiene el estado de "wall jumping" y permite volver a interactuar con paredes.
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
        if (rb == null) return; // Asegura que el Rigidbody exista antes de dibujar.

        if (Application.isPlaying) // Si el juego está en ejecución.
        {
            // Dibuja los Raycasts en tiempo de ejecución según la dirección del input o velocidad.
            if (currentHorizontalInput > 0.05f || rb.linearVelocity.x > 0.05f)
            {
                Gizmos.color = Color.blue; // Azul para la derecha.
                Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * wallCheckDistance);
            }
            if (currentHorizontalInput < -0.05f || rb.linearVelocity.x < -0.05f)
            {
                Gizmos.color = Color.red; // Rojo para la izquierda.
                Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.left * wallCheckDistance);
            }
        }
        else // Si el editor no está en modo de juego.
        {
            // Dibuja los Raycasts en el editor para facilitar la configuración.
            Gizmos.color = Color.yellow; // Amarillo por defecto.
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * wallCheckDistance);
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.left * wallCheckDistance);
        }
    }

    // --- Propiedades públicas para acceder al estado desde otros scripts ---
    public bool IsWallSliding => isWallSliding; // Permite saber si el jugador está deslizándose por la pared.
    public bool IsWallJumping => isWallJumping; // Permite saber si el jugador está realizando un salto de pared.
}