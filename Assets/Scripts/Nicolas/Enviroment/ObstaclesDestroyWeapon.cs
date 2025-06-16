using UnityEngine;
using Pathfinding;

public class ObstaclesDestroyWeapon : MonoBehaviour
{
    public AstarPath astar;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Colisión detectada con: " + other.name);

        if (other.CompareTag("Obstacle"))
        {
            // Drop si el obstáculo lo tiene asignado
            ObstacleDrop drop = other.GetComponent<ObstacleDrop>();
            if (drop != null && drop.prefabPurityObstacle != null)
            {
                Instantiate(drop.prefabPurityObstacle, other.transform.position, Quaternion.identity);
            }

            // Actualizar grafo de navegación
            Bounds bounds = other.bounds;
            GraphUpdateObject guo = new GraphUpdateObject(bounds)
            {
                updatePhysics = true
            };
            AstarPath.active.UpdateGraphs(guo);

            Destroy(other.gameObject);
        }
    }
}
