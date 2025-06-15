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
        if (scene.buildIndex == 1) // In-game scene
        {
            pauseMenuAccess.enabled = true;
        }
        else if (scene.buildIndex == 0)
        {
            Time.timeScale = 1f;
            Debug.Log("[SceneWatcher] Reset Time.timeScale on Title scene.");
        }
        else
        {
            pauseMenuAccess.enabled = false;
        }
    }
}
