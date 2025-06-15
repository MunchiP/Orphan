using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VerticalSwordGroupSpawner : MonoBehaviour
{
    public GameObject swordPrefab;
    public bool canPoolGrow = true;

    [Header("Límites de generación")]
    public Transform leftLimit;   // referencia X inicial
    public Transform yReference;  // altura fija Y para todas las espadas

    [Header("Configuración de espadas")]
    public int swordsPerGroup = 5;
    public float swordSpacing = 1.5f; // separación horizontal entre espadas dentro de un grupo

    [Header("Spawn config")]
    public float delayBetweenWaves = 5f;
    public int totalWaves = 3;
    private Animator anim;

    private List<GameObject> swordPool = new List<GameObject>();
    private List<Transform> swordPositions = new List<Transform>();
    private int currentWave = 0;
    private Rigidbody2D rb;

    private void Awake()
    {
        anim = GetComponentInParent<Animator>();
        if (swordPrefab == null || leftLimit == null || yReference == null)
        {
            Debug.LogError("Faltan asignaciones en el inspector.");
            enabled = false;
            return;
        }

        GenerateSwordPositions();
        FillPool(swordPositions.Count);
    }

    private void OnEnable()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        currentWave = 0;
        StartCoroutine(SpawnLoop());
    }

    private void OnDisable()
    {
        StopAllCoroutines();

        foreach (var sword in swordPool)
        {
            if (sword != null)
                sword.SetActive(false);
        }
    }

    private IEnumerator SpawnLoop()
    {
        yield return new WaitForSeconds(delayBetweenWaves);

        int totalGroups = swordPositions.Count / swordsPerGroup;

        while (currentWave < totalWaves)
        {
            Debug.Log($"[Spawner] Iniciando oleada {currentWave + 1} de {totalWaves}");

            List<int> chosenGroups = GetRandomGroupsNoThreeConsecutive(totalGroups, 3);

            foreach (int groupIndex in chosenGroups)
            {
                int startIndex = groupIndex * swordsPerGroup;
                for (int i = startIndex; i < startIndex + swordsPerGroup; i++)
                {
                    GameObject sword = GetFromPool();
                    if (sword != null)
                    {
                        sword.transform.position = swordPositions[i].position;
                        sword.transform.rotation = Quaternion.identity;
                        sword.SetActive(true);
                    }
                }
            }

            currentWave++;

            if (currentWave < totalWaves)
            {
                Debug.Log("[Spawner] Esperando antes de la siguiente oleada...");
                yield return new WaitForSeconds(delayBetweenWaves);
            }
        }

        Debug.Log("[Spawner] Última oleada terminada. Esperando 5 segundos antes de desactivar script...");
        rb.bodyType = RigidbodyType2D.Dynamic;
        anim.SetBool("special2", false);
        yield return new WaitForSeconds(5f);

        // Reactivar BossOneBehaviour en el padre
        BossOneBehaviour bossBehaviour = GetComponentInParent<BossOneBehaviour>();
        if (bossBehaviour != null)
        {
            bossBehaviour.enabled = true;
            Debug.Log("[Spawner] Reactivando BossOneBehaviour.");
        }
        else
        {
            Debug.LogWarning("[Spawner] No se encontró BossOneBehaviour en el padre.");
        }

        Debug.Log("[Spawner] Desactivando script.");
        this.enabled = false;
    }

    private List<int> GetRandomGroupsNoThreeConsecutive(int totalGroups, int count)
    {
        List<int> validCombination = null;

        List<int> allGroups = new List<int>();
        for (int i = 0; i < totalGroups; i++)
            allGroups.Add(i);

        int attempts = 0;
        while (validCombination == null && attempts < 1000)
        {
            attempts++;

            List<int> candidate = new List<int>();
            List<int> tempGroups = new List<int>(allGroups);

            for (int i = 0; i < count; i++)
            {
                int idx = Random.Range(0, tempGroups.Count);
                candidate.Add(tempGroups[idx]);
                tempGroups.RemoveAt(idx);
            }

            candidate.Sort();

            if (!HasThreeConsecutive(candidate))
                validCombination = candidate;
        }

        if (validCombination == null)
        {
            validCombination = new List<int>();
            for (int i = 0; i < count; i++)
                validCombination.Add(i);
        }

        return validCombination;
    }

    private bool HasThreeConsecutive(List<int> list)
    {
        if (list.Count < 3) return false;

        for (int i = 0; i < list.Count - 2; i++)
        {
            if (list[i + 1] == list[i] + 1 && list[i + 2] == list[i] + 2)
                return true;
        }
        return false;
    }

    private void GenerateSwordPositions()
    {
        swordPositions.Clear();

        int totalGroups = 5;

        float startX = leftLimit.position.x;
        float y = yReference.position.y;

        float groupWidth = swordSpacing * swordsPerGroup;

        for (int groupIndex = 0; groupIndex < totalGroups; groupIndex++)
        {
            float groupStartX = startX + groupIndex * groupWidth;

            for (int i = 0; i < swordsPerGroup; i++)
            {
                GameObject point = new GameObject($"SwordPos_Group{groupIndex}_Idx{i}");
                point.transform.parent = this.transform;

                float x = groupStartX + i * swordSpacing;
                point.transform.position = new Vector3(x, y, 0f);

                swordPositions.Add(point.transform);
            }
        }
    }

    private void FillPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(swordPrefab);
            obj.SetActive(false);
            swordPool.Add(obj);
        }
    }

    private GameObject GetFromPool()
    {
        foreach (var sword in swordPool)
        {
            if (!sword.activeInHierarchy)
                return sword;
        }

        if (canPoolGrow)
        {
            GameObject newSword = Instantiate(swordPrefab);
            newSword.SetActive(false);
            swordPool.Add(newSword);
            return newSword;
        }

        return null;
    }
}
