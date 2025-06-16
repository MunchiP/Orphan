using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem; // Necesario para usar el nuevo sistema de Input.

[RequireComponent(typeof(Rigidbody2D))]
public class GrappleLiana : MonoBehaviour
{
    // --- Variables de Input System ---
    private InputSystem_Actions inputActions;    // Referencia al Asset de Input Actions.
    private bool swingPressedThisFrame;          // Bandera para la acción de enganchar/soltar la liana.
    private float horizontalMoveInput;           // Input de movimiento horizontal para balanceo.

    [Header("Grapple (Liana)")]
    public float detectionRadius = 5f;
    public LayerMask grappleLayer;
    public LineRenderer lineRenderer;

    [Header("Swing Boost")]
    public float swingBoostForce = 8f;

    Rigidbody2D rb;
    DistanceJoint2D swingJoint;
    public Vector2 grapplePoint;
    public bool isAttached; // Indica si el jugador está actualmente enganchado.

    public GameObject boquita; // Objeto que representa la "boca" del jugador.
    public GameObject cabeza;  // Objeto que representa la "cabeza" del jugador.
    public Quaternion rotationSave;
    private Animator anim;

    /// <summary>
    /// Se llama cuando el script se carga. Inicializa el Input System.
    /// </summary>
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lineRenderer.positionCount = 0; // Oculta la línea al inicio.

        // Asegúrate de que estos objetos estén asignados en el Inspector o existen en la escena.
        // Si "Boquita" y "Cabeza" son parte de tu prefab de jugador, es mejor arrastrarlos al inspector.
        // Si no, asegúrate de que existen como GameObjects con esos nombres en la jerarquía.
        if (boquita == null) boquita = GameObject.Find("Boquita");
        if (cabeza == null) cabeza = GameObject.Find("Cabeza");

        if (boquita == null) Debug.LogWarning("GrappleLiana: No se encontró el GameObject 'Boquita'. Asegúrate de que esté asignado o exista en la escena.");


        inputActions = new InputSystem_Actions();

        // Suscribe el evento 'performed' de la acción 'Swing'.
        // Cada vez que 'Swing' se presione, se activa 'swingPressedThisFrame'.
        inputActions.Player.Swing.performed += ctx => swingPressedThisFrame = true;

        // Suscribe los eventos de la acción 'Move' para el balanceo.
        inputActions.Player.Move.performed += ctx => horizontalMoveInput = ctx.ReadValue<Vector2>().x;
        inputActions.Player.Move.canceled += ctx => horizontalMoveInput = 0f;
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

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }
    /// <summary>
    /// Se llama una vez por frame. Maneja la lógica de enganche y soltar.
    /// </summary>
    void Update()
    {
        anim.SetBool("swing", isAttached);
        // --- Lógica de enganche y desenganche de liana con la acción 'Swing' ---
        // Si la acción 'Swing' fue presionada en este frame...
        if (swingPressedThisFrame)
        {
            if (swingJoint == null) // Si NO estamos enganchados, intentamos enganchar.
            {
                rotationSave = transform.rotation; // Guarda la rotación actual del jugador.
                Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius, grappleLayer);

                // --- Variables para encontrar el mejor punto de enganche válido ---
                Transform bestValidTarget = null; // Cambiado a 'bestValidTarget' para claridad
                float bestValidDist = float.MaxValue;

                // === EL FILTRADO DEBE OCURRIR AQUÍ, PARA CADA 'HIT' ===
                foreach (var c in hits)
                {
                    // === CONDICIÓN DE Y: Tu jugador (o tu boquita) debe estar por debajo de la Y del objeto.
                    // Asumiendo que 'boquita' es el punto exacto desde donde sale la liana.
                    // Si quieres que el jugador DEBA estar por debajo del objeto, la condición es:
                    // boquita.transform.position.y < c.transform.position.y
                    // Si quieres una tolerancia (como el "Y + 2" que mencionaste, que significa que puedes estar
                    // hasta 2 unidades por encima del punto de enganche), entonces:
                    // boquita.transform.position.y < c.transform.position.y + 2f // Usando '2f' directamente o una variable

                    // Usaremos la variable grappleYThreshold para que sea configurable desde el inspector.
                    // Asumimos que 'boquita' es el punto de referencia del jugador para el enganche.
                    if (boquita != null && boquita.transform.position.y < c.transform.position.y )
                    {
                        float d = Vector2.Distance(transform.position, c.transform.position); // Distancia del jugador al punto.
                        if (d < bestValidDist)
                        {
                            bestValidDist = d;
                            bestValidTarget = c.transform;
                        }
                    }
                }

                // === DESPUÉS DE FILTRAR, SI ENCONTRAMOS UN bestValidTarget ===
                if (bestValidTarget != null) // Si se encontró un punto de enganche VÁLIDO.
                {
                    isAttached = true;
                    grapplePoint = bestValidTarget.position; // Ahora sí asignamos el grapplePoint
                    CreateSwingJoint();
                }
            }
            else // Si SÍ estamos enganchados, soltamos la liana.
            {
                RemoveSwingJoint();
            }
        }
        swingPressedThisFrame = false;

        // --- Dibuja la cuerda de la liana ---
        if (swingJoint != null && lineRenderer.positionCount >= 2 && boquita != null)
        {
            lineRenderer.SetPosition(0, boquita.transform.position);
            lineRenderer.SetPosition(1, grapplePoint);
        }
    }


    /// <summary>
    /// Se llama a intervalos de tiempo fijos. Ideal para aplicar fuerzas.
    /// </summary>
    void FixedUpdate()
    {
        if (swingJoint != null) // Solo aplica fuerzas si estamos enganchados.
        {
            // Aplica una fuerza lateral para el balanceo usando el input horizontal.
            if (Mathf.Abs(horizontalMoveInput) > 0.1f)
                rb.AddForce(new Vector2(horizontalMoveInput * swingBoostForce, 0));
        }
    }

    /// <summary>
    /// Crea y configura la articulación de distancia (liana).
    /// </summary>
    void CreateSwingJoint()
    {
        swingJoint = gameObject.AddComponent<DistanceJoint2D>();
        swingJoint.connectedAnchor = grapplePoint;
        swingJoint.autoConfigureDistance = false;
        swingJoint.distance = Vector2.Distance(transform.position, grapplePoint);
        swingJoint.enableCollision = false;

        lineRenderer.positionCount = 2;
    }

    /// <summary>
    /// Elimina la articulación de distancia y resetea el estado de la liana.
    /// Este método es público para flexibilidad, pero aquí solo es llamado internamente.
    /// </summary>
    public void RemoveSwingJoint()
    {
        isAttached = false;
        Destroy(swingJoint);
        swingJoint = null;
        lineRenderer.positionCount = 0;
    }

    /// <summary>
    /// Dibuja un Gizmo en el editor para visualizar el radio de detección.
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}