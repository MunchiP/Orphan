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

    public void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnEnable()
    {
        
    }

    void OnDisable()
    {
        
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        soundManagerScript = AudioManager.GetComponent<AudioManager>(); // <- esta línea es necesaria

        musicToggle = music.GetComponent<Button>();
        sfxToggle = sfx.GetComponent<Button>();

        musicToggle.onClick.RemoveAllListeners();
        musicToggle.onClick.AddListener(() => soundManagerScript.ToggleMusic());

        sfxToggle.onClick.RemoveAllListeners();
        sfxToggle.onClick.AddListener(() => soundManagerScript.ToggleSfx());
    }
}
