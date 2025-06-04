using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Seeker))]
public class EnemigoVoladorIA : MonoBehaviour
{
    public Transform jugador;
    private Vector2 puntoOrigen;

    [Header("Configuración")]
    public float velocidad = 3f;
    public float rangoPersecucion = 5f;
    public float rangoPatrullaje = 3f;
    public float tiempoCambioPatrulla = 3f;

    [Tooltip("Cada cuánto actualizar el camino (segundos)")]
    public float intervaloActualizacionCamino = 0.5f;

    private Seeker seeker;
    private Rigidbody2D rb;
    private Path path;
    private int indiceCamino = 0;

    private float tiempoDesdeUltimaActualizacion = 0f;
    private float tiempoDesdeUltimoCambioPatrulla = 0f;
    private Vector2 destinoPatrulla;

    private Vector2 knockbackVelocity = Vector2.zero;
    private float knockbackDamping = 5f;

    private bool persiguiendoJugador = false;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        puntoOrigen = rb.position; // Guarda la posición inicial como punto origen
        destinoPatrulla = puntoOrigen;

        BuscarNuevoDestinoPatrulla();
        ActualizarCamino(destinoPatrulla);
    }

    void Update()
    {
        if (jugador == null)
            return;

        tiempoDesdeUltimaActualizacion += Time.deltaTime;
        tiempoDesdeUltimoCambioPatrulla += Time.deltaTime;

        float distanciaJugador = Vector2.Distance(rb.position, jugador.position);

        if (!persiguiendoJugador && distanciaJugador <= rangoPersecucion)
        {
            persiguiendoJugador = true;
            tiempoDesdeUltimaActualizacion = intervaloActualizacionCamino; // fuerza actualización inmediata
        }

        if (persiguiendoJugador)
        {
            if (tiempoDesdeUltimaActualizacion >= intervaloActualizacionCamino)
            {
                ActualizarCamino(jugador.position);
                tiempoDesdeUltimaActualizacion = 0f;
            }
        }
        else
        {
            if (tiempoDesdeUltimoCambioPatrulla >= tiempoCambioPatrulla)
            {
                BuscarNuevoDestinoPatrulla();
                ActualizarCamino(destinoPatrulla);
                tiempoDesdeUltimoCambioPatrulla = 0f;
            }
        }
    }

    void FixedUpdate()
    {
        if (path == null || indiceCamino >= path.vectorPath.Count)
            return;

        Vector2 targetPos = (Vector2)path.vectorPath[indiceCamino];
        Vector2 direccion = (targetPos - rb.position).normalized;
        Vector2 movimiento = direccion * velocidad * Time.fixedDeltaTime;

        // Suaviza la velocidad de knockback
        knockbackVelocity = Vector2.Lerp(knockbackVelocity, Vector2.zero, knockbackDamping * Time.fixedDeltaTime);

        rb.MovePosition(rb.position + movimiento + knockbackVelocity);

        if (Vector2.Distance(rb.position, targetPos) < 0.1f)
        {
            indiceCamino++;
        }

        // Dibuja línea hacia el siguiente punto del camino
        Debug.DrawLine(rb.position, targetPos, Color.red);
    }

    void ActualizarCamino(Vector2 objetivo)
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, objetivo, OnPathComplete);
            Debug.Log("Actualizando camino hacia: " + objetivo);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            indiceCamino = 0;
        }
    }

    void BuscarNuevoDestinoPatrulla()
    {
        Vector2 randomOffset = Random.insideUnitCircle * rangoPatrullaje;
        destinoPatrulla = puntoOrigen + randomOffset;
    }

    public void AplicarKnockback(Vector2 fuerza)
    {
        knockbackVelocity += fuerza;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.2f);

        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawWireSphere(transform.position, rangoPersecucion);

        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawWireSphere(transform.position, rangoPatrullaje);
    }
}
