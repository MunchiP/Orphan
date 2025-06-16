using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FixedPositionSwordSpawner : MonoBehaviour
{
    public BossOneBehaviour bossOneBehaviour;
    public GameObject swordPrefab;
    public int poolSize = 10;
    public bool canPoolGrow = true;

    public Transform xReferenceTransform;
    public float xOffsetFromReference = 0f;

    public Transform floorReferenceTransform;
    public float initialYSpawnOffset = 0f;
    public float ySpacingBetweenPositions = 2f;
    public int numberOfPatternPositions = 5;

    public float spawnInterval = 1f;
    public int maxSwordsToSpawn = 20;

    private List<GameObject> swordPool = new List<GameObject>();
    private int spawnedSwordCount = 0;

    private int lastPatternIndex = -1;
    private Animator anim;
    private Rigidbody2D rb;

    private bool isSpawningActive = false;

    private void Awake()
    {
        anim = GetComponentInParent<Animator>();

        if (swordPrefab == null || xReferenceTransform == null || floorReferenceTransform == null)
        {
            Debug.LogError("[Spawner] Asigna los objetos faltantes en el inspector.");
            enabled = false;
            return;
        }

        if (bossOneBehaviour == null)
        {
            bossOneBehaviour = GetComponentInParent<BossOneBehaviour>();
            Debug.Log("[Spawner] Referencia a BossOneBehaviour obtenida en Awake.");
        }

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(swordPrefab);
            obj.SetActive(false);
            swordPool.Add(obj);

            SwordHorizontalController swordController = obj.GetComponent<SwordHorizontalController>();
            if (swordController != null)
            {
                swordController.spawner = this;
            }
            else
            {
                Debug.LogWarning($"[Spawner] La espada prefab '{swordPrefab.name}' no tiene un SwordHorizontalController.");
            }
        }
    }

    private void OnEnable()
    {
        Debug.Log("[Spawner] OnEnable llamado.");

        if (bossOneBehaviour == null)
        {
            bossOneBehaviour = GetComponentInParent<BossOneBehaviour>();
            Debug.Log("[Spawner] Referencia a BossOneBehaviour obtenida en OnEnable.");
        }

        if (rb == null)
        {
            rb = GetComponentInParent<Rigidbody2D>();
            Debug.Log("[Spawner] Referencia a Rigidbody2D obtenida.");
        }

        if (rb != null)
            rb.bodyType = RigidbodyType2D.Kinematic;

        spawnedSwordCount = 0;
        isSpawningActive = false;

        if (bossOneBehaviour != null)
        {
            bossOneBehaviour.enabled = false;
            Debug.Log("[Spawner] BossOneBehaviour desactivado.");
        }
    }

    private void OnDisable()
    {
        Debug.Log("[Spawner] OnDisable llamado.");
        ManualDisable(); // Por si acaso el GO sí se desactiva
    }

    public void StartSpawningFromAnimation()
    {
        if (!isSpawningActive)
        {
            Debug.Log("[Spawner] Recibido evento de animación. Iniciando generación de espadas.");
            isSpawningActive = true;
            spawnedSwordCount = 0;
            StartCoroutine(SpawnSwords());
        }
    }

    private IEnumerator SpawnSwords()
    {
        if (!isSpawningActive) yield break;

        while (spawnedSwordCount < maxSwordsToSpawn && isSpawningActive)
        {
            GameObject sword = GetFromPool();
            if (sword != null)
            {
                int newPatternIndex;
                do
                {
                    newPatternIndex = Random.Range(0, numberOfPatternPositions);
                } while (newPatternIndex == lastPatternIndex && numberOfPatternPositions > 1);

                lastPatternIndex = newPatternIndex;

                float y = floorReferenceTransform.position.y + initialYSpawnOffset + (newPatternIndex * ySpacingBetweenPositions);
                float x = xReferenceTransform.position.x + xOffsetFromReference;
                Vector3 spawnPos = new Vector3(x, y, 0f);

                sword.transform.position = spawnPos;
                sword.transform.rotation = Quaternion.identity;
                sword.SetActive(true);

                spawnedSwordCount++;
            }

            yield return new WaitForSeconds(spawnInterval);
        }

        if (spawnedSwordCount >= maxSwordsToSpawn)
        {
            if (rb != null)
                rb.bodyType = RigidbodyType2D.Dynamic;

            isSpawningActive = false;
        }
    }

    private GameObject GetFromPool()
    {
        foreach (var obj in swordPool)
        {
            if (!obj.activeInHierarchy)
                return obj;
        }

        if (canPoolGrow)
        {
            GameObject newSword = Instantiate(swordPrefab);
            swordPool.Add(newSword);

            SwordHorizontalController swordController = newSword.GetComponent<SwordHorizontalController>();
            if (swordController != null)
            {
                swordController.spawner = this;
            }
            else
            {
                Debug.LogWarning("[Spawner] La nueva espada no tiene SwordHorizontalController.");
            }

            return newSword;
        }

        return null;
    }

    public void DeactivateSword(GameObject sword)
    {
        sword.SetActive(false);

        int activeCount = 0;
        foreach (var s in swordPool)
        {
            if (s.activeInHierarchy)
                activeCount++;
        }

        if (activeCount == 1 && anim != null && anim.GetBool("special1"))
        {
            anim.SetBool("special1", false);
        }
        else if (activeCount == 0)
        {
            Debug.Log("[Spawner] Todas las espadas desactivadas. Llamando ManualDisable y desactivando el script.");
            ManualDisable();
            this.enabled = false;
        }
    }

    private void ManualDisable()
    {
        Debug.Log("[Spawner] ManualDisable llamado.");

        StopAllCoroutines();
        isSpawningActive = false;

        foreach (var sword in swordPool)
        {
            if (sword != null)
                sword.SetActive(false);
        }

        if (bossOneBehaviour != null)
        {
            bossOneBehaviour.enabled = true;
            Debug.Log("[Spawner] BossOneBehaviour reactivado desde ManualDisable.");
        }
        else
        {
            Debug.Log("[Spawner] BossOneBehaviour no existe en ManualDisable.");
        }
    }
}
