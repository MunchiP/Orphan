using UnityEngine;

public class EnemyDead : MonoBehaviour
{
    public GameObject prefabPurity;

    public void GenerarLoot()
    {
        if (prefabPurity != null)
        {
            Instantiate(prefabPurity, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("No se asignó el prefabPurity en el inspector.");
        }
    }
}
