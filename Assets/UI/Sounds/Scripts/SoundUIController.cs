using UnityEngine;
using UnityEngine.UI;

public class SoundUIController : MonoBehaviour
{
    public static SoundUIController instance;

    public Slider musicSlider, sfxSlider;

    public void Awake()
    {
        
            
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
        
    }

    
    void Update()
    {
        
    }
}
