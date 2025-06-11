using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RollingCreditsController : MonoBehaviour
{
    public List<GameObject> creditTextList = new List<GameObject>();
    private bool isPlaying;

    public void OnEnable()
    {
        isPlaying = true;
        StartCoroutine(RollTheCredits());
    }

    public void OnDisable()
    {
        isPlaying = false;
        StopAllCoroutines();
    }

    public IEnumerator RollTheCredits()
    {
        while (isPlaying)
        {
            for (int i = 0; i < creditTextList.Count; i++)
            {
                creditTextList[i].SetActive(true);
                yield return new WaitForSeconds(6.2f);
                creditTextList[i].SetActive(false);
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
