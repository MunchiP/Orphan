using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundBoardListenerController : MonoBehaviour
{
    public GameObject music;
    public GameObject sfx;
    private Button musicToggle;
    private Button sfxToggle;
    public GameObject AudioManager;
    private AudioManager soundManagerScript;
      

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        musicToggle = music.GetComponent<Button>();

        
        musicToggle.onClick.RemoveAllListeners();
        musicToggle.onClick.AddListener(() => soundManagerScript.ToggleMusic());

        sfxToggle.onClick.RemoveAllListeners();
        sfxToggle.onClick.AddListener(() => soundManagerScript.ToggleSfx());
    }
}
