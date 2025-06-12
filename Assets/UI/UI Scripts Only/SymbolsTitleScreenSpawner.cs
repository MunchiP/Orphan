using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem.XR;

public class SymbolsTitleScreenSpawner : MonoBehaviour
{
    public List<GameObject> clonedSymbolList = new List<GameObject>();
    public List<GameObject> originalSymbolList = new List<GameObject>();
    public int eyesPoolSizePerPrefab = 3;
    private int eyesAmmounToShow;
    private float spawnRangeX = 244f;
    private float spawnRangeY = 70f;

    private RectTransform canvasTransform;
    void Start()
    {
        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null)
        {
            canvasTransform = canvas.GetComponent<RectTransform>();
        }
        else
        {
            Debug.LogError("Canvas not found for CultSymbolController.");
        }
        AddToPool(eyesPoolSizePerPrefab);
        StartCoroutine(SpawnTheEyes());
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddToPool(int eyesPerPrefab)
    {
        for (int i = 0; i < eyesPerPrefab; i++) 
        {
            foreach (GameObject prefab in originalSymbolList)
            {
                GameObject eye = Instantiate(prefab, canvasTransform);
                RectTransform rt = eye.GetComponent<RectTransform>();
                rt.anchoredPosition = Vector2.zero;
                eye.SetActive(false);
                clonedSymbolList.Add(eye);
            }
        }
    }

    private IEnumerator SpawnTheEyes()
    {
        while (true)
        {
            
            yield return new WaitForSeconds(6f);
            eyesAmmounToShow = Random.Range(1, 4);
            for (int i = 0; i < eyesAmmounToShow; i++)
            {
                float positionX = Random.Range(-spawnRangeX, spawnRangeX);
                float positionZ = Random.Range(-spawnRangeY, spawnRangeY);
                GameObject spawnedCultEye = GetEyeFree();
                RectTransform eyeRt = spawnedCultEye.GetComponent<RectTransform>();
                eyeRt.anchoredPosition = new Vector2(positionX, positionZ);
                spawnedCultEye.SetActive(true);
                StartCoroutine(DespawnAfterTime(spawnedCultEye, 6f));

            }
            

        }
    }

    public void DeactivateSymbols()
    {
        Debug.Log("Kenneth es severa loca");
        StopAllCoroutines();
        for (int i = 0; i < clonedSymbolList.Count; i++)
        {
            clonedSymbolList[i].SetActive(false);
        }    
    }

    public IEnumerator DespawnAfterTime(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
    }

    public GameObject GetEyeFree()
    {
        List<GameObject> inactiveSymbols = new List<GameObject>();

        foreach (GameObject eye in clonedSymbolList)
        {
            if (!eye.activeInHierarchy)
            {
                inactiveSymbols.Add(eye);
            }
        }

        if (inactiveSymbols.Count > 0)
        {
            return inactiveSymbols[Random.Range(0, inactiveSymbols.Count)];
        }

        AddToPool(1);
        return clonedSymbolList[clonedSymbolList.Count - 1];
    }
}
