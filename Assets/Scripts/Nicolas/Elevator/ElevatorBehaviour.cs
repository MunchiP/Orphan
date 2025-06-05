using UnityEngine;
using System.Collections; // Necesario para usar Coroutines

public class ElevatorBehaviour : MonoBehaviour
{
    // --- Variables públicas que puedes ajustar en el Inspector ---
    [Header("Configuración del Ascensor")]
    public float speed = 2f; // Velocidad de movimiento en unidades/segundo
    public Transform topPoint; // Objeto vacío que define la posición superior del ascensor
    public float waitAtTopTime = 3f; // Tiempo de espera en la cima (en segundos)
    public float crushingDetectionThreshold = 0.5f; // Distancia desde la posición inicial para detectar aplastamiento (al bajar)
    
    // --- Variables Internas ---
    private Rigidbody2D rb; // Referencia al Rigidbody2D del ascensor
    private Vector3 initialPosition; // Almacena la posición inicial del ascensor
    private bool elevatorActivated = false; // Bandera para controlar si el ascensor está en movimiento
    private Coroutine elevatorRoutine; // Referencia a la Coroutine de movimiento del ascensor

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position; // Guarda la posición de inicio del ascensor

        // Aseguramos que el Rigidbody2D del elevador es Kinematic.
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void FixedUpdate()
    {
        // Al ser Kinematic y mover el ascensor directamente por transform.position,
        // su Rigidbody2D no se moverá por sí mismo.
        rb.linearVelocity = Vector2.zero; 
    }

    // --- Lógica de Colisión (usando el COLLIDER PRINCIPAL del ascensor, que NO es Trigger) ---
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Comprueba si el objeto que colisionó es el "Player" y si el ascensor está en su posición inicial y no está activado.
        if (collision.gameObject.CompareTag("Player") && 
            Vector3.Distance(transform.position, initialPosition) < 0.1f && 
            !elevatorActivated) 
        {
            elevatorActivated = true; 
            if (elevatorRoutine != null)
            {
                StopCoroutine(elevatorRoutine); 
            }
            elevatorRoutine = StartCoroutine(MoveElevatorRoutine(true)); 
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // LÓGICA DE ANTIPLASTAMIENTO:
            if (elevatorActivated &&
                transform.position.y <= initialPosition.y + crushingDetectionThreshold && 
                collision.transform.position.y < transform.position.y - (GetComponent<Collider2D>().bounds.extents.y * 0.75f) && 
                (elevatorRoutine != null))
            {
                Debug.Log("¡Jugador detectado debajo del ascensor! Volviendo a subir.");
                StopCoroutine(elevatorRoutine); 
                elevatorRoutine = StartCoroutine(MoveElevatorRoutine(true)); 
            }
        }
    }

    // --- MÉTODOS PARA EL TRIGGER DE DETECCIÓN DE JUGADOR ENCIMA ("PlayerDetectionZone") ---
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(transform); 
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.transform.parent == transform)
            {
                other.transform.SetParent(null); 
            }
        }
    }

    // --- Coroutine para el movimiento del ascensor ---
    IEnumerator MoveElevatorRoutine(bool startGoingUp)
    {
        if (startGoingUp)
        {
            while (Vector3.Distance(transform.position, topPoint.position) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, topPoint.position, speed * Time.deltaTime);
                yield return null; 
            }
            transform.position = topPoint.position; 

            yield return new WaitForSeconds(waitAtTopTime); 
        }

        while (Vector3.Distance(transform.position, initialPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, speed * Time.deltaTime);
            yield return null; 
        }
        transform.position = initialPosition; 

        elevatorActivated = false; 
    }
}