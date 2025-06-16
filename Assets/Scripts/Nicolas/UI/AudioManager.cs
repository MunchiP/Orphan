using UnityEngine;

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

            // Cargar volúmenes guardados
            musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

            // Aplicar volumen inicial al musicSource si existe
            if (musicSource != null)
                musicSource.volume = musicVolume;
        }
        else
        {
            Debug.LogWarning("Ya existe una instancia de AudioManager. Destruyendo duplicado.");
            Destroy(gameObject);
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
        // Aquí podrías aplicar volumen a efectos de sonido si tienes control sobre ellos.
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

    // ---------------------------------------------------------
    // Método para reproducir música por nombre
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
            // Ya está sonando esta música, no hacemos nada
            return;
        }

        musicSource.clip = track.clip;
        musicSource.volume = musicVolume;
        musicSource.loop = true; // Música suele ir en loop
        musicSource.Play();
    }
}
