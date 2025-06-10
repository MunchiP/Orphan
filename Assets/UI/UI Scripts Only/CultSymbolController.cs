using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem.XR;

public class CultSymbolController : MonoBehaviour
{
    public List<GameObject> eyeList = new List<GameObject>();
    public GameObject cultSymbol;
    public int eyesPoolSize;
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
        AddToPool(eyesPoolSize);
        StartCoroutine(SpawnTheEyes());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToPool(int eyesPoolSize)
    {
        for(int i = 0; i < eyesPoolSize; i++)
        {
            GameObject eye;
            eye = Instantiate(cultSymbol, canvasTransform);
            RectTransform rt = eye.GetComponent<RectTransform>();
            rt.anchoredPosition = Vector2.zero;
            eye.SetActive(false);
            eyeList.Add(eye);
        } 
            
    }

    private IEnumerator SpawnTheEyes()
    {
        while (true)
        {
            float randomWaiting = Random.Range(7, 9);
            yield return new WaitForSeconds(5f);
            eyesAmmounToShow = Random.Range(0, 4);
            for (int i = 0; i < eyesAmmounToShow; i++)
            {
                float positionX = Random.Range(-spawnRangeX, spawnRangeX);
                float positionZ = Random.Range(-spawnRangeY, spawnRangeY);
                GameObject spawnedCultEye = GetEyeFree();
                RectTransform eyeRt = spawnedCultEye.GetComponent<RectTransform>();
                eyeRt.anchoredPosition = new Vector2(positionX, positionZ);
                spawnedCultEye.SetActive(true);

            }
            yield return new WaitForSeconds(randomWaiting);

        }
    }

    public GameObject GetEyeFree()
    {
        for(int i = 0;i < eyeList.Count;i++)
        {
            if(!eyeList[i].activeInHierarchy)
            {
                return eyeList[i];
            }
            
        }
        AddToPool(1);
        return eyeList[eyeList.Count - 1];
    }
}
