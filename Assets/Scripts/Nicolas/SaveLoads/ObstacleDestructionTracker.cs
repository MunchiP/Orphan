using UnityEngine;
using System.Collections.Generic;

public class ObstacleDestructionTracker : MonoBehaviour
{
    public static ObstacleDestructionTracker Instance;
    private HashSet<string> destroyedObstacles = new HashSet<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void MarkObstacleDestroyed(string id) => destroyedObstacles.Add(id);

    public bool IsObstacleDestroyed(string id) => destroyedObstacles.Contains(id);

    public void SaveDestroyedObstacles()
    {
        string data = string.Join(",", destroyedObstacles);
        PlayerPrefs.SetString("obstacles", data);
    }

    public void LoadDestroyedObstacles()
    {
        destroyedObstacles.Clear();
        string data = PlayerPrefs.GetString("obstacles", "");
        if (!string.IsNullOrEmpty(data))
        {
            foreach (var id in data.Split(','))
            {
                if (!string.IsNullOrWhiteSpace(id))
                    destroyedObstacles.Add(id);
            }
        }
    }

    public string GetObstaclesData() => string.Join(",", destroyedObstacles);
}