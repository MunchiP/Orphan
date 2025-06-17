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

            // Guardar estado TEMPORAL del jugador
            PlayerState playerState = collision.GetComponent<PlayerState>();
            if (playerState != null)
            {
                PlayerPrefs.SetInt("vida_temp", playerState.vidaActual);
                PlayerPrefs.SetInt("pureza_temp", playerState.purezaActual);
                PlayerPrefs.SetFloat("posX_temp", playerState.transform.position.x);
                PlayerPrefs.SetFloat("posY_temp", playerState.transform.position.y);
                PlayerPrefs.Save();
            }

            // Cambiar escena
            ButtonListManager fadeManager = FindAnyObjectByType<ButtonListManager>();
            fadeManager.ChangeSceneByIndex(sceneNumber);
        }
    }
}
