using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    [Header("Audio Sources")]
    public AudioSource musicSource;

    [System.Serializable]
    public class MusicTrack
    {
        public string trackName;
        public AudioClip clip;
    }

    [Header("Lista de pistas de música")]
    public MusicTrack[] musicTracks;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

            if (musicSource != null)
                musicSource.volume = musicVolume;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int activeIndex = scene.buildIndex;
        int lastIndex = SceneManager.sceneCountInBuildSettings - 1;

        if (activeIndex != lastIndex)
        {
            if (musicSource.clip == null || musicSource.clip.name != "MainTheme")
            {
                PlayMusic("MainTheme");
            }
        }
        else
        {
            if (musicSource.clip == null || musicSource.clip.name != "BossFight")
            {
                PlayMusic("BossFight");
            }
        }
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();

        if (musicSource != null)
            musicSource.volume = musicVolume;
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save();
    }

    public float GetMusicVolume() => musicVolume;
    public float GetSFXVolume() => sfxVolume;

    public void ToggleMusic()
    {
        SetMusicVolume(musicVolume > 0f ? 0f : 1f);
    }

    public void ToggleSfx()
    {
        SetSFXVolume(sfxVolume > 0f ? 0f : 1f);
    }

    public void MusicVolume(float value)
    {
        SetMusicVolume(value);
    }

    public void SfxVolume(float value)
    {
        SetSFXVolume(value);
    }

    public void PlayMusic(string trackName)
    {
        if (musicSource == null)
        {
            Debug.LogWarning("AudioManager: No hay AudioSource asignado para música.");
            return;
        }

        MusicTrack track = System.Array.Find(musicTracks, t => t.trackName == trackName);

        if (track == null)
        {
            Debug.LogWarning($"AudioManager: No se encontró la pista de música '{trackName}'.");
            return;
        }

        if (musicSource.clip == track.clip && musicSource.isPlaying)
        {
            return; // Ya está sonando
        }

        musicSource.clip = track.clip;
        musicSource.volume = musicVolume;
        musicSource.loop = true;
        musicSource.Play();
    }
}
