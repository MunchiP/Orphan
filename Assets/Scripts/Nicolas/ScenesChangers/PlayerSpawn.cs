using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        StartCoroutine(SetPositionNextFrame());
    }

    private System.Collections.IEnumerator SetPositionNextFrame()
    {
        yield return null;

        string spawnPointName = PlayerPrefs.GetString("spawnPoint", "Default");
        Debug.Log("[PlayerSpawn] Intentando buscar punto de spawn: " + spawnPointName);

        GameObject spawnPoint = GameObject.Find(spawnPointName);

        if (spawnPoint != null)
        {
            transform.position = spawnPoint.transform.position;
            Debug.Log("[PlayerSpawn] Posicionado exitosamente en: " + spawnPointName + " con posición " + spawnPoint.transform.position);
        }
        else
        {
            Debug.LogWarning($"[PlayerSpawn] No se encontró el punto de spawn con el nombre '{spawnPointName}'");
        }

        PlayerPrefs.DeleteKey("spawnPoint");

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
