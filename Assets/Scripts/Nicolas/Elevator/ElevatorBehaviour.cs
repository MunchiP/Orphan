using UnityEngine;
using System.Collections;

public class ElevatorBehaviour : MonoBehaviour
{
    [Header("Configuración del Ascensor")]
    public float speed = 2f;
    public Transform topPoint;
    public float waitAtTopTime = 3f;

    private Rigidbody2D rb;
    private Vector3 initialPosition;
    private bool elevatorActivated = false;
    private Coroutine elevatorRoutine;

    public UnderElevator underElevator; // Asegúrate de que este script detecta correctamente si el jugador está debajo
    private bool isUnder = false;
    private PlayerState playerState;

    void Start()
    {
        playerState = FindAnyObjectByType<PlayerState>();
        rb = GetComponent<Rigidbody2D>();
        // Aseguramos que el Rigidbody2D sea Kinematic para control directo de la posición
        rb.bodyType = RigidbodyType2D.Kinematic;
        initialPosition = transform.position;
    }

    void Update()
    {
        // Obtenemos el estado actual del script UnderElevator
        isUnder = underElevator.isUnder;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Activamos el ascensor cuando el jugador lo toca en su posición inicial
        if (playerState.purezaActual >=90 && collision.gameObject.CompareTag("Player") &&
            Vector3.Distance(transform.position, initialPosition) < 0.1f &&
            !elevatorActivated)
        {
            elevatorActivated = true;
            // Detenemos cualquier rutina existente para evitar múltiples coroutines
            if (elevatorRoutine != null)
                StopCoroutine(elevatorRoutine);

            elevatorRoutine = StartCoroutine(MoveElevatorRoutine(true));
        }
    }

    IEnumerator MoveElevatorRoutine(bool startGoingUp)
    {
        // --- Subir ---
        if (startGoingUp)
        {
            // Mover el ascensor al punto superior
            while (Vector3.Distance(transform.position, topPoint.position) > 0.01f)
            {
                rb.MovePosition(Vector2.MoveTowards(rb.position, topPoint.position, speed * Time.fixedDeltaTime));
                yield return new WaitForFixedUpdate(); // Esperar la siguiente actualización de física
            }
            rb.MovePosition(topPoint.position); // Ajustar a la posición superior exacta
            yield return new WaitForSeconds(waitAtTopTime); // Esperar en la parte superior
        }

        // --- Bajar ---
        while (Vector3.Distance(transform.position, initialPosition) > 0.01f)
        {
            // Si se detecta al jugador debajo del ascensor durante el descenso
            if (isUnder)
            {
                Debug.Log("Jugador debajo detectado durante bajada. Volviendo a subir.");
                yield return new WaitForSeconds(0.1f); // Pequeño retraso antes de reiniciar el ascenso
                // Detener el descenso actual y reiniciar la rutina, forzando un ascenso
                if (elevatorRoutine != null)
                    StopCoroutine(elevatorRoutine);
                elevatorRoutine = StartCoroutine(MoveElevatorRoutine(true));
                yield break; // Salir de esta rutina actual
            }

            // Continuar bajando
            rb.MovePosition(Vector2.MoveTowards(rb.position, initialPosition, speed * Time.fixedDeltaTime));
            yield return new WaitForFixedUpdate();
        }

        rb.MovePosition(initialPosition); // Ajustar a la posición inicial exacta
        elevatorActivated = false; // Reiniciar el estado de activación
    }
}