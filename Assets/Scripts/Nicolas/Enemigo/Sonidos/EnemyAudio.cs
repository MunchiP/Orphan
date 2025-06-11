using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyAudio : MonoBehaviour
{
    private AudioSource audioSource;
    private Transform listener;

    [Header("Volumen base y distancia")]
    [Range(0f, 1f)] public float baseVolume = 1f;
    public float maxDistance = 15f;

    [Header("Sonidos adicionales")]
    public AudioClip hurtClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0f;
        audioSource.loop = true;
        audioSource.Play(); // Reproducimos desde el inicio, pero en volumen 0
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
            Debug.LogWarning("No se encontró ningún objeto con tag 'Player'");
        }
    }

    private void Update()
    {
        if (listener == null || AudioManager.instance == null) return;

        float distance = Vector3.Distance(transform.position, listener.position);

        if (distance > maxDistance)
        {
            audioSource.volume = 0f;
            return;
        }

        float distanceFactor = Mathf.Clamp01(1f - (distance / maxDistance));
        float finalVolume = baseVolume * distanceFactor * AudioManager.instance.sfxSource.volume;

        audioSource.volume = finalVolume;
    }

    // 🔊 Reproduce un sonido de daño sin interrumpir el loop
   public void PlayHurtSound()
{
    if (hurtClip != null)
    {
        float originalPitch = audioSource.pitch;

        // Ajusta la velocidad. >1 = más rápido. Ej: 1.2f es 20% más rápido
        audioSource.pitch = 1.6f;

        audioSource.PlayOneShot(hurtClip, AudioManager.instance.sfxSource.volume);

        // Restaurar el pitch original después de reproducir
        Invoke(nameof(ResetPitch), hurtClip.length / audioSource.pitch);
    }
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
