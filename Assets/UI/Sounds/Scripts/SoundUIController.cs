using UnityEngine;
using UnityEngine.UI;

public class SoundUIController : MonoBehaviour
{
    public static SoundUIController instance;

    public Slider musicSlider, sfxSlider;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Evita duplicados
            return;
        }

        instance = this;
        // Opcional: DontDestroyOnLoad(gameObject); si quieres que persista entre escenas
    }

    public void ToggleMusic()
    {
        AudioManager.instance.ToggleMusic();
    }

    public void ToggleSfx()
    {
        AudioManager.instance.ToggleSfx();
    }

    public void MusicVolume()
    {
        AudioManager.instance.MusicVolume(musicSlider.value);
    }

    public void SfxVolume()
    {
        AudioManager.instance.SfxVolume(sfxSlider.value);
    }

    void Start()
    {
        // Opcional: puedes sincronizar sliders aquí si es necesario
    }

    void Update()
    {
        // No usado por ahora
    }
}
