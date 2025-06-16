using UnityEngine;

public class ObstacleID : MonoBehaviour
{
    public string obstacleID;

    void Start()
    {
        if (ObstacleDestructionTracker.Instance.IsObstacleDestroyed(obstacleID))
        {
            gameObject.SetActive(false); // o Destroy(gameObject);
        }
    }
}