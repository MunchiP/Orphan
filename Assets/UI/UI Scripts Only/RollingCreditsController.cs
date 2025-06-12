using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class RollingCreditsController : MonoBehaviour
{
    public List<GameObject> creditTextList = new List<GameObject>();
    private bool isPlaying;
   
    public void OnEnable()
    {
        isPlaying = true;
        for (int i = 0; i < creditTextList.Count; i++)
        {
            creditTextList[i].SetActive(false);
        }
        StartCoroutine(RollTheCredits());
    }

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public IEnumerator RollTheCredits()
    {
        if (isPlaying)
        {
            foreach (GameObject creditText in creditTextList)
            {
                
                creditText.SetActive(true);
                yield return new WaitForSeconds(6.2f);
                creditText.SetActive(false);
                yield return new WaitForSeconds(2f);
            }
            isPlaying = false;
        }
    }
}

