using UnityEngine;
using System.Collections;

public class SwordVerticalController : MonoBehaviour
{
    [Header("Animación de Giro")]
    public float rotationDuration = 1.0f;
    public int numberOfRotations = 2;

    [Header("Movimiento Vertical")]
    public float moveSpeed = 5.0f;

    [Header("Límites de Desactivación")]
    public float distanceToDeactivate = 10f;

    [Header("Audio")]
    public AudioClip moveSound;
    private AudioSource audioSource;

    [HideInInspector]
    public MonoBehaviour spawner; // Soporta spawner genérico

    private IEnumerator currentActionRoutine;
    private Vector3 originalScale;
    BossOneAudioEvents bossOneAudioEvents;


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
        float timer = 0f;
        float totalDegrees = 360f * numberOfRotations;
        float rotationSpeed = totalDegrees / rotationDuration;

        while (timer < rotationDuration)
        {
            float progress = timer / rotationDuration;

            float deltaRotation = rotationSpeed * Time.deltaTime;
            transform.Rotate(0f, 0f, deltaRotation);

            transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, progress);

            timer += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;

        Vector3 finalEuler = transform.eulerAngles;
        finalEuler.z = 99f;
        transform.eulerAngles = finalEuler;
        bossOneAudioEvents.PlaySwordMove();

        float startY = transform.position.y;
        float limitY = startY - distanceToDeactivate;

        while (transform.position.y > limitY)
        {
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime, Space.World);
            yield return null;
        }

        if (spawner != null)
        {
            var method = spawner.GetType().GetMethod("DeactivateSword");
            if (method != null)
                method.Invoke(spawner, new object[] { gameObject });
            else
                gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
            Debug.LogWarning("[SwordVerticalController] Spawner es nulo. La espada se desactiva directamente.");
        }

        currentActionRoutine = null;
    }
}
