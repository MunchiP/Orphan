using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePauseMenuWatcher : MonoBehaviour
{
    public PauseMenuAccess pauseMenuAccess;

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
        if (scene.buildIndex == 0)
        {
            pauseMenuAccess.enabled = false;
            Time.timeScale = 1f;
            Debug.Log("[SceneWatcher] Reset Time.timeScale on Title scene.");
        }
        else // Any other scene
        {
            pauseMenuAccess.enabled = true;
            Debug.Log("[SceneWatcher] Enabled PauseMenuAccess in scene " + scene.buildIndex);
        }
    }
}
