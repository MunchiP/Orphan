using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BossOneAudioEvents : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip specialAttack1Clip;
    public AudioClip specialAttack12Clip;
    public AudioClip specialAttack2Clip;
    public AudioClip specialAttack22Clip;
    public AudioClip attack1Clip;
    public AudioClip attack2Clip;
    public AudioClip attack22Clip;
    public AudioClip jumpAttackClip;
    public AudioClip jumpAttackClip2;
    public AudioClip walkClip;
    public AudioClip hurtClip;
    public AudioClip dieClip;
    public AudioClip swordMove;
    public AudioClip teleportAudio;

    [Header("Volumen base del jefe")]
    [Range(0f, 1f)] public float baseVolume = 1f;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StopFX()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void Play(AudioClip clip, float pitch = 1f)
    {
        if (clip == null)
        {
            Debug.LogWarning("Clip null en Play");
            return;
        }

        float finalVolume = baseVolume;

        if (AudioManager.Instance != null) // 🔧 Corrección: Instance con mayúscula
        {
            finalVolume *= AudioManager.Instance.sfxVolume; // 🔧 sfxVolume en lugar de sfxSource.volume
        }
        else
        {
            Debug.LogWarning("AudioManager.Instance es null, usando solo baseVolume");
        }

        if (pitch == 1f)
        {
            // Pitch normal
            audioSource.PlayOneShot(clip, finalVolume);
        }
        else
        {
            // Crear AudioSource temporal para pitch personalizado
            GameObject tempGO = new GameObject("TempAudio_" + clip.name);
            tempGO.transform.position = transform.position;

            AudioSource tempAS = tempGO.AddComponent<AudioSource>();
            tempAS.clip = clip;
            tempAS.pitch = pitch;
            tempAS.volume = finalVolume;
            tempAS.spatialBlend = 0f;
            tempAS.outputAudioMixerGroup = audioSource.outputAudioMixerGroup;

            tempAS.Play();
            Destroy(tempGO, clip.length / Mathf.Abs(pitch));
        }
    }

    // Métodos para eventos del Animator
    public void PlaySpecialAttack1() => Play(specialAttack1Clip);
    public void PlaySpecialAttack12() => Play(specialAttack12Clip);
    public void PlaySpecialAttack2() => Play(specialAttack2Clip, 0.6f);
    public void PlaySpecialAttack22() => Play(specialAttack22Clip);
    public void PlayAttack1() => Play(attack1Clip, 1f);
    public void PlayAttack2() => Play(attack2Clip, 0.2f);
    public void PlayAttack22() => Play(attack22Clip, 0.2f);
    public void PlayJumpAttack() => Play(jumpAttackClip, 0.6f);
    public void PlayJumpAttack2() => Play(jumpAttackClip2, 0.7f);
    public void PlayWalk() => Play(walkClip, 1f);
    public void PlayHurt() => Play(hurtClip, 1f);
    public void PlayDie() => Play(dieClip);
    public void PlaySwordMove() => Play(swordMove);
    public void PlayTeleportAudio() => Play(teleportAudio);
}
