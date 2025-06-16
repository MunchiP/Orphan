using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistenceManager : MonoBehaviour
{
    private void Awake()
    {
        PlayerPrefs.DeleteAll();
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        Debug.Log("[PersistenceManager] Awake: Registered OnSceneLoaded event.");
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Debug.Log("[PersistenceManager] OnDestroy: Unregistered OnSceneLoaded event.");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[PersistenceManager] OnSceneLoaded: Scene '{scene.name}' loaded. Scanning obstacles...");

        // Paso 4: limpiar la memoria de obstáculos destruidos para evitar inconsistencias
        if (ObstacleDestructionTracker.Instance != null)
        {
            ObstacleDestructionTracker.Instance.ClearDestroyedObstacles();
            Debug.Log("[PersistenceManager] Cleared in-memory destroyed obstacles list.");
        }

        ObstacleID[] obstacles = GameObject.FindObjectsByType<ObstacleID>(FindObjectsSortMode.None);
        Debug.Log($"[PersistenceManager] Found {obstacles.Length} obstacles in the scene.");

        foreach (var obstacle in obstacles)
        {
            string id = obstacle.obstacleID;
            string key = $"obstacle_{id}";
            int val = PlayerPrefs.GetInt(key, 0);

            Debug.Log($"[PersistenceManager] Obstacle ID: {id} | PlayerPrefs Key: {key} | Value: {val}");

            if (val == 1)
            {
                obstacle.gameObject.SetActive(false);
                Debug.Log($"[PersistenceManager] Deactivating obstacle with ID: {id}");
            }
            else
            {
                Debug.Log($"[PersistenceManager] Leaving obstacle with ID: {id} active.");
            }
        }
    }
}
