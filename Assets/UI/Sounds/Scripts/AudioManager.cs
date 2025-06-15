using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Cargar desde PlayerPrefs si existe
            // GetFloat devolverá el segundo argumento (valor por defecto) si la clave no existe.
            musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        }
        else
        {
            // Si ya existe una instancia, destruye esta nueva para mantener el Singleton.
            Debug.LogWarning("Se intentó crear una segunda instancia de AudioManager. Destruyendo este GameObject para mantener el patrón Singleton.", this);
            Destroy(gameObject);
        }
    }

    // Nota: Los métodos Set/Get ya son bastante seguros por su diseño,
    // pero podrías añadir Debug.Log para ver cuándo se guardan los valores si lo necesitas.
    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
        // Debug.Log("Volumen de música ajustado a: " + value); // Opcional: para depuración
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        PlayerPrefs.SetFloat("SFXVolume", value);
        // Debug.Log("Volumen de SFX ajustado a: " + value); // Opcional: para depuración
    }

    public float GetMusicVolume() => musicVolume;
    public float GetSFXVolume() => sfxVolume;
}