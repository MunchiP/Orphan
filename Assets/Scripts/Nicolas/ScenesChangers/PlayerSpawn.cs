using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    void Start()
    {
        string spawnPointName = PlayerSpawnManager.spawnPointName;
        GameObject spawnPoint = GameObject.Find(spawnPointName);

        if (spawnPoint != null)
        {
            transform.position = spawnPoint.transform.position;
        }
        else
        {
            Debug.LogWarning($"No se encontró el punto de spawn con el nombre '{spawnPointName}'");
        }
    }
}
