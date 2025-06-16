using UnityEngine;
using Pathfinding;
using System.Collections.Generic;

public class ObstaclesDestroyWeapon : MonoBehaviour
{
    public AstarPath astar;

    [System.Serializable]
    public struct ObstacleParticle
    {
        public string tag;
        public ParticleSystem particlePrefab;
    }

    public List<ObstacleParticle> obstacleParticlesList = new List<ObstacleParticle>();
    private Dictionary<string, ParticleSystem> obstacleParticlesMap;

    private void Awake()
    {
        obstacleParticlesMap = new Dictionary<string, ParticleSystem>();
        foreach (var op in obstacleParticlesList)
        {
            if (!obstacleParticlesMap.ContainsKey(op.tag))
            {
                obstacleParticlesMap.Add(op.tag, op.particlePrefab);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
{
    Debug.Log("Colisión con: " + other.name);

    ObstacleID obstacleIDComponent = other.GetComponent<ObstacleID>();
    if (obstacleIDComponent == null)
    {
        Debug.LogWarning("Objeto no tiene ObstacleID, no se procesa.");
        return;
    }

    string id = obstacleIDComponent.obstacleID;

    if (PlayerPrefs.GetInt($"obstacle_{id}", 0) == 1)
    {
        Debug.Log($"Obstáculo {id} ya fue destruido.");
        return;
    }

    PlayerPrefs.SetInt($"obstacle_{id}", 1);
    PlayerPrefs.Save();

    if (obstacleParticlesMap.ContainsKey(other.tag))
    {
        ParticleSystem particlePrefab = obstacleParticlesMap[other.tag];
        if (particlePrefab != null)
        {
            ParticleSystem psInstance = Instantiate(particlePrefab, other.transform.position, other.transform.rotation);
            psInstance.Play();
            Destroy(psInstance.gameObject, psInstance.main.duration + psInstance.main.startLifetime.constantMax);
        }
    }

    ObstacleDrop drop = other.GetComponent<ObstacleDrop>();
    if (drop != null && drop.prefabPurityObstacle != null)
    {
        Instantiate(drop.prefabPurityObstacle, other.transform.position, Quaternion.identity);
    }

    if (astar != null)
    {
        Bounds bounds = other.bounds;
        GraphUpdateObject guo = new GraphUpdateObject(bounds)
        {
            updatePhysics = true
        };
        AstarPath.active.UpdateGraphs(guo);
    }

    Destroy(other.gameObject);
    Debug.Log($"Obstáculo {id} destruido.");
}

}