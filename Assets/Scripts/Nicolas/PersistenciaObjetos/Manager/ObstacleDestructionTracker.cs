using System.Collections.Generic;
using UnityEngine;

public class ObstacleDestructionTracker : MonoBehaviour
{
    public static ObstacleDestructionTracker Instance { get; private set; }

    private HashSet<string> destroyedObstacles = new HashSet<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void MarkObstacleDestroyed(string id)
    {
        destroyedObstacles.Add(id);
    }

    public bool IsObstacleDestroyed(string id)
    {
        return destroyedObstacles.Contains(id);
    }

    public void SaveDestroyedObstacles()
    {
        foreach (var id in destroyedObstacles)
        {
            PlayerPrefs.SetInt($"obstacle_{id}", 1);
        }
        PlayerPrefs.Save();
    }

    public void ClearDestroyedObstacles()
    {
        destroyedObstacles.Clear();
    }
}
