using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] public int sceneNumber;
    [SerializeField] public string targetSpawnPointName; // Nombre del punto de entrada en la escena destino

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("[ChangeScene] Colisión con Player. Cambiando a spawn point: " + targetSpawnPointName);

            // ✅ Guardar el nombre del punto de entrada en PlayerPrefs
            PlayerPrefs.SetString("spawnPoint", targetSpawnPointName);
            PlayerPrefs.Save();

            // Guardar obstáculos si aplica
            if (ObstacleDestructionTracker.Instance != null)
            {
                ObstacleDestructionTracker.Instance.SaveDestroyedObstacles();
            }

            // Guardar estado TEMPORAL del jugador
            PlayerState playerState = collision.GetComponent<PlayerState>();
            if (playerState != null)
            {
                PlayerPrefs.SetInt("vida_temp", playerState.vidaActual);
                PlayerPrefs.SetInt("pureza_temp", playerState.purezaActual);
                PlayerPrefs.Save();
            }

            // Cambiar escena con fade
            ButtonListManager fadeManager = FindAnyObjectByType<ButtonListManager>();
            fadeManager.ChangeSceneByIndex(sceneNumber);
        }
    }
}
