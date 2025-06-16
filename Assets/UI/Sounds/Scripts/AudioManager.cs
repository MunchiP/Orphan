using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public SoundNameAndClip[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;
    public Image musicXmark, sfxXmark;
    public Image musicCheckmark, sfxCheckmark;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public void PlayMusic(string name)
    {
        SoundNameAndClip sound = Array.Find(musicSounds, x => x.soundName == name);

        if (sound == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            musicSource.clip = sound.clip;
            musicSource.Play();
        }

    }

    public void PlaySFX(string name)
    {
        SoundNameAndClip sound = Array.Find(sfxSounds, x => x.soundName == name);

        if (sound == null)
        {
            Debug.Log("Sound Not Found");
            return;
        }

        if (sound.clip == null)
        {
            Debug.LogWarning("Sound clip is NULL for sound: " + name);
            return;
        }

        sfxSource.PlayOneShot(sound.clip);
    }

    public void ToggleMusic()
    {
        Debug.Log("[AudioManager] ToggleMusic CALLED");
        musicSource.mute = !musicSource.mute;
        musicXmark.enabled = musicSource.mute;
        musicCheckmark.enabled = !musicSource.mute;
    }

    public void ToggleSfx()
    {
        sfxSource.mute = !sfxSource.mute;
        sfxXmark.enabled = sfxSource.mute;
        sfxCheckmark.enabled = !sfxSource.mute;
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SfxVolume(float volume)
    {
        sfxSource.volume = volume;
        PlaySFX("SoundSfxTest");
    }
    void Update()
    {

    }

    public static float GetSfxVolume()
    {
        return instance != null ? instance.sfxSource.volume : 1f;
    }
}
