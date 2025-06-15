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
        audioSource = GetComponent<AudioSource>();
    }
    public void StopFX()
    {
        if (audioSource.isPlaying && audioSource.clip == fallClip)
        {
            audioSource.Stop();
        }
    }
    public void Play(AudioClip clip, float pitch = 1f)
    {
        if (clip == null) return;

        if (audioSource.isPlaying && audioSource.clip == fallClip)
        {
            StopFX();
        }

        float finalVolume = baseVolume;

        if (AudioManager.instance != null)
        {
            finalVolume *= AudioManager.instance.sfxSource.volume;
        }
        else
        {
            Debug.LogWarning("AudioManager.instance es null, usando solo baseVolume");
        }

        audioSource.pitch = pitch;
        audioSource.PlayOneShot(clip, finalVolume);
        audioSource.pitch = 1f; // Restaurar pitch
    }



    // Métodos para eventos del Animator
    public void PlayWalk() => Play(walkClip);
    public void PlayWalk2() => Play(walkClip2);
    public void PlayAttack() => Play(attackClip);
    public void PlayJump() => Play(jumpClip);
    public void PlayHurt() => Play(hurtClip, 1.5f); // Ej: más agudo
    public void PlayFall() => Play(fallClip, 3f);
    public void PlayDeath() => Play(deathClip);
}
