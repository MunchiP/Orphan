using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
        else
        {
            Debug.LogWarning("[PlayerSpawn] No se encontró Rigidbody2D en el jugador.");
        }

        StartCoroutine(SetPositionNextFrame());
    }

    private System.Collections.IEnumerator SetPositionNextFrame()
    {
        yield return null;

        string spawnPointName = PlayerSpawnManager.spawnPointName;
        GameObject spawnPoint = GameObject.Find(spawnPointName);

        if (spawnPoint != null)
        {
            transform.position = spawnPoint.transform.position;
        }
        else
        {
            Debug.LogWarning($"[PlayerSpawn] No se encontró el punto de spawn con el nombre '{spawnPointName}'");
        }

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}