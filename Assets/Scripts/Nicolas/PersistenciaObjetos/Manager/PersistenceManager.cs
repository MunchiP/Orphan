using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistenceManager : MonoBehaviour
{
    private void Awake()
    {
        PlayerPrefs.DeleteAll();
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    ObstacleID[] obstacles = GameObject.FindObjectsByType<ObstacleID>(FindObjectsSortMode.None);

    foreach (var obstacle in obstacles)
    {
        string id = obstacle.obstacleID;
        string key = $"obstacle_{id}";
        int val = PlayerPrefs.GetInt(key, 0);

        if (val == 1)
        {
            obstacle.gameObject.SetActive(false);
        }
    }
}



}
