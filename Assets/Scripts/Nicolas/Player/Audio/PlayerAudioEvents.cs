using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudioEvents : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip walkClip;
    public AudioClip walkClip2;
    public AudioClip attackClip;
    public AudioClip jumpClip;
    public AudioClip hurtClip;
    public AudioClip fallClip;
    public AudioClip deathClip;

    [Header("Volumen base del jugador")]
    [Range(0f, 1f)] public float baseVolume = 1f;

    private AudioSource audioSource;

    private void Awake()
    {
        // Asegurarse de que tenemos un AudioSource adjunto al GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("PlayerAudioEvents requiere un componente AudioSource en el mismo GameObject.", this);
            enabled = false; // Deshabilita el script si no hay AudioSource
        }
    }

    public void StopFX()
    {
        if (audioSource == null) return; // Protección contra AudioSource nulo
        if (audioSource.isPlaying && audioSource.clip == fallClip)
        {
            audioSource.Stop();
        }
    }

    public void Play(AudioClip clip, float pitch = 1f)
    {
        // Verificar si el clip es nulo o si el AudioSource no existe
        if (clip == null || audioSource == null)
        {
            Debug.LogWarning("Intento de reproducir un clip nulo o con AudioSource no asignado en " + gameObject.name, this);
            return;
        }

        // Verificar la existencia del AudioManager
        if (AudioManager.Instance == null)
        {
            Debug.LogError("No se encontró una instancia de AudioManager. Asegúrate de tener uno en la escena.", this);
            return;
        }

        // Si se está reproduciendo el clip de caída, detenerlo antes de reproducir otro
        if (audioSource.isPlaying && audioSource.clip == fallClip)
        {
            StopFX(); // Usa el método existente para detener específicamente el sonido de caída
        }

        // Calcula el volumen final multiplicando el volumen base del jugador por el volumen SFX del AudioManager
        float finalVolume = baseVolume * AudioManager.Instance.GetSFXVolume(); // Aquí está el cambio clave

        audioSource.pitch = pitch;
        audioSource.PlayOneShot(clip, finalVolume);
        audioSource.pitch = 1f; // Restaurar pitch después de la reproducción del PlayOneShot
    }

    // Métodos para eventos del Animator
    public void PlayWalk() => Play(walkClip);
    public void PlayWalk2() => Play(walkClip2);
    public void PlayAttack() => Play(attackClip);
    public void PlayJump() => Play(jumpClip);
    public void PlayHurt() => Play(hurtClip, 1.5f); // Ejemplo: más agudo
    public void PlayFall() => Play(fallClip, 3f);
    public void PlayDeath() => Play(deathClip);
}