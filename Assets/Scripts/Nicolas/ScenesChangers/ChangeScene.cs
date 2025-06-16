using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] public int sceneNumber;
    [SerializeField] public string targetSpawnPointName; // Nombre del punto de entrada en la escena destino

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Guardar nombre del punto de entrada
            PlayerSpawnManager.spawnPointName = targetSpawnPointName;

            // Guardar obstáculos si aplica
            if (ObstacleDestructionTracker.Instance != null)
            {
                ObstacleDestructionTracker.Instance.SaveDestroyedObstacles();
            }

            // Cambiar de escena
            ButtonListManager fadeManager = FindAnyObjectByType<ButtonListManager>();
            fadeManager.ChangeSceneByIndex(sceneNumber);
        }
    }
}
