using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneNext : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Guardar el estado real solo cuando pasa a la siguiente escena
            if (ObstacleDestructionTracker.Instance != null)
            {
                ObstacleDestructionTracker.Instance.SaveDestroyedObstacles();
            }

            ButtonListManager fadeManager = FindAnyObjectByType<ButtonListManager>();
            fadeManager.ChangeSceneToNextScene();
        }        
    }
}
