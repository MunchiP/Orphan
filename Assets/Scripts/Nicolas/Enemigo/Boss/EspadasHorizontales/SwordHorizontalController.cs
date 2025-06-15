using UnityEngine;
using System.Collections;

public class SwordHorizontalController : MonoBehaviour
{
    [Header("Animación de Giro")]
    public float rotationDuration = 1.0f;
    public int numberOfRotations = 2;

    [Header("Movimiento Horizontal")]
    public float moveSpeed = 5.0f;

    [Header("Límites de Desactivación")]
    public float distanceToDeactivate = 10f;

    // --- Nuevas variables para el audio ---
    [Header("Audio")]
    public AudioClip moveSound; // El clip de sonido a reproducir
    private AudioSource audioSource; // La referencia al componente AudioSource

    [HideInInspector]
    public FixedPositionSwordSpawner spawner;  // Referencia al spawner que controla las espadas

    private IEnumerator currentActionRoutine;
    private Vector3 originalScale;
    private BossOneAudioEvents bossOneAudioEvents;

    // Se llama una vez cuando el script se carga o el objeto se instancia

    void OnEnable()
    {
        bossOneAudioEvents = FindAnyObjectByType<BossOneAudioEvents>();
        originalScale = transform.localScale;
        transform.localScale = Vector3.zero;

        if (currentActionRoutine != null)
            StopCoroutine(currentActionRoutine);

        currentActionRoutine = SwordActionRoutine();
        StartCoroutine(currentActionRoutine);
    }

    private IEnumerator SwordActionRoutine()
    {
        // --- Fase de Giro + Escalado ---
        float timer = 0f;
        float totalDegrees = 360f * numberOfRotations;
        float rotationSpeed = totalDegrees / rotationDuration;

        while (timer < rotationDuration)
        {
            float progress = timer / rotationDuration;

            // Rotación
            float deltaRotation = rotationSpeed * Time.deltaTime;
            transform.Rotate(0f, 0f, deltaRotation);

            // Escalado durante la rotación
            transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, progress);

            timer += Time.deltaTime;
            yield return null;
        }

        // Asegura valores finales
        transform.localScale = originalScale;

        // Establecer rotación Z fija en 9 grados
        Vector3 finalEuler = transform.eulerAngles;
        finalEuler.z = 9f;
        transform.eulerAngles = finalEuler;
        bossOneAudioEvents.PlaySwordMove();



        float startX = transform.position.x;
        float limitX = startX - distanceToDeactivate;

        while (transform.position.x > limitX)
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime, Space.World);
            yield return null;
        }

        // Cuando termine, avisar al spawner y desactivar la espada
        if (spawner != null)
        {
            spawner.DeactivateSword(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
            Debug.LogWarning("[SwordHorizontalController] Spawner es nulo. La espada se desactiva directamente.");
        }

        currentActionRoutine = null;
    }
}