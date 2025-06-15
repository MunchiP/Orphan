using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class RollingCreditsController : MonoBehaviour
{
    public List<GameObject> creditTextList = new List<GameObject>();
    private bool isPlaying;
    private Coroutine runningCreditsCoroutine;
    

    public void PlayCredits()
    {
        if (runningCreditsCoroutine != null)
            StopCoroutine(runningCreditsCoroutine);

        isPlaying = true;

        foreach (GameObject creditText in creditTextList)
        {
            creditText.SetActive(false);
        }

        runningCreditsCoroutine = StartCoroutine(RollTheCredits());
    }

    private IEnumerator RollTheCredits()
    {
        foreach (GameObject creditText in creditTextList)
        {
            if (!isPlaying) yield break; // Exit early if stopped
            creditText.SetActive(true);
            yield return new WaitForSeconds(6.2f);
            creditText.SetActive(false);
            yield return new WaitForSeconds(2f);
        }

        isPlaying = false;
        runningCreditsCoroutine = null;
    }
}

