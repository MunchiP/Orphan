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
            Bounds bounds = other.bounds;

            // Actualiza el grafo ANTES de destruir el objeto
            GraphUpdateObject guo = new GraphUpdateObject(bounds);
            guo.updatePhysics = true;
            AstarPath.active.UpdateGraphs(guo); // <- ESTO FALTABA

            Destroy(other.gameObject);
        }
    }
}

    