using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyAudio : MonoBehaviour
{
    private AudioSource audioSource;
    private Transform listener; // Generalmente será el Transform del jugador

    [Header("Volumen base y distancia")]
    [Range(0f, 1f)] public float baseVolume = 1f;
    public float maxDistance = 15f;

    [Header("Sonidos adicionales")]
    public AudioClip hurtClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("EnemyAudio requiere un componente AudioSource en el mismo GameObject.", this);
            enabled = false; // Deshabilita el script si no hay AudioSource
            return;
        }

        // Configuración inicial del AudioSource
        audioSource.volume = 0f; // Inicialmente en 0
        audioSource.loop = true; // Para sonidos ambientales o de respiración del enemigo
        audioSource.playOnAwake = false; // No reproducir automáticamente al despertar
    }

    private void Start()
    {
        // Busca al jugador por tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            listener = player.transform;
        }
        else
        {
            Debug.LogWarning("No se encontró ningún GameObject con el tag 'Player'. La atenuación de volumen por distancia para el AudioSource de " + gameObject.name + " no funcionará.", this);
        }

        // Asegúrate de que el AudioManager esté presente
        if (AudioManager.instance == null)
        {
            Debug.LogError("No se encontró una instancia de AudioManager en la escena. El volumen de los SFX de este enemigo no se ajustará correctamente.", this);
        }

        if (audioSource.clip != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    private void Update()
    {
        if (listener == null || AudioManager.instance == null || audioSource == null)
        {
            audioSource.volume = 0f;
            return;
        }

        float distance = Vector3.Distance(transform.position, listener.position);

        if (distance > maxDistance)
        {
            audioSource.volume = 0f;
            return;
        }

        float distanceFactor = Mathf.Clamp01(1f - (distance / maxDistance));

        float finalVolume = baseVolume * distanceFactor * AudioManager.instance.GetSFXVolume();

        audioSource.volume = finalVolume;
    }

    public void PlayHurtSound()
    {
        if (hurtClip == null)
        {
            Debug.LogWarning("hurtClip no asignado en EnemyAudio para " + gameObject.name + ". No se puede reproducir el sonido de daño.", this);
            return;
        }

        if (audioSource == null) return;

        if (AudioManager.instance == null)
        {
            Debug.LogError("No se encontró una instancia de AudioManager. No se puede ajustar el volumen del sonido de daño.", this);
            audioSource.PlayOneShot(hurtClip, baseVolume);
            return;
        }

        audioSource.PlayOneShot(hurtClip, baseVolume * AudioManager.instance.GetSFXVolume());

        float originalPitch = audioSource.pitch;
        audioSource.pitch = 1.6f;

        CancelInvoke(nameof(ResetPitch));
        Invoke(nameof(ResetPitch), hurtClip.length / 1.6f);
    }

    private void ResetPitch()
    {
        audioSource.pitch = 1f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}
