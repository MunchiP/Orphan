using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ElevatorBehaviour : MonoBehaviour
{
    [Header("Configuración del Ascensor")]
    public float speed = 2f;

    public Transform topPoint;
    public float waitAtTopTime = 3f;

    public bool isFirstTime = false;

    private Rigidbody2D rb;
    private Vector3 initialPosition;
    private bool elevatorActivated = false;
    private Coroutine elevatorRoutine;

    public UnderElevator underElevator;
    private bool isUnder = false;
    private PlayerState playerState;

    private bool hasUnlockedElevator = false; // 👈 Nuevo bool

    void Start()
    {
        playerState = FindAnyObjectByType<PlayerState>();
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        initialPosition = transform.position;
    }

    void Update()
    {
        isUnder = underElevator.isUnder;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") &&
            Vector3.Distance(transform.position, initialPosition) < 0.1f &&
            !elevatorActivated)
        {
            // Solo pide pureza si estamos en escena 1 y no se ha desbloqueado aún
            // if (SceneManager.GetActiveScene().buildIndex == 1 && !hasUnlockedElevator)
            // {
            //     if (playerState.purezaActual < 90)
            //     {
            //         Debug.Log("No tienes suficiente pureza para activar el ascensor.");
            //         return;
            //     }

            //     // ✅ Si tenía la pureza suficiente, ya no se vuelve a pedir
            //     hasUnlockedElevator = true;
            // }

            elevatorActivated = true;

            if (elevatorRoutine != null)
                StopCoroutine(elevatorRoutine);

            elevatorRoutine = StartCoroutine(MoveElevatorRoutine(true));
        }
    }

    IEnumerator MoveElevatorRoutine(bool startGoingUp)
    {
        if (startGoingUp)
        {
            while (Vector3.Distance(transform.position, topPoint.position) > 0.01f)
            {
                rb.MovePosition(Vector2.MoveTowards(rb.position, topPoint.position, speed * Time.fixedDeltaTime));
                yield return new WaitForFixedUpdate();
            }
            rb.MovePosition(topPoint.position);
            yield return new WaitForSeconds(waitAtTopTime);
        }

        while (Vector3.Distance(transform.position, initialPosition) > 0.01f)
        {
            if (isUnder)
            {
                Debug.Log("Jugador debajo detectado durante bajada. Volviendo a subir.");
                yield return new WaitForSeconds(0.1f);

                if (elevatorRoutine != null)
                    StopCoroutine(elevatorRoutine);
                elevatorRoutine = StartCoroutine(MoveElevatorRoutine(true));
                yield break;
            }

            rb.MovePosition(Vector2.MoveTowards(rb.position, initialPosition, speed * Time.fixedDeltaTime));
            yield return new WaitForFixedUpdate();
        }

        rb.MovePosition(initialPosition);
        elevatorActivated = false;
    }
}
