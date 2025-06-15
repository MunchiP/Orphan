using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameCredits : MonoBehaviour
{
    public ButtonListManager universalButtonList;
    public GameObject EndCredits;
    public RollingCreditsController RollingCreditsController;
    public GameObject thankYou;
    public bool hasPlayerWon = false;

    private Coroutine checkCoroutine;
    private Coroutine sendBackToTitle;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        hasPlayerWon = false;

        // Start coroutines again after scene load
        if (checkCoroutine != null) StopCoroutine(checkCoroutine);
        if (sendBackToTitle != null) StopCoroutine(sendBackToTitle);

        checkCoroutine = StartCoroutine(CheckPlayerHasFinished());
        sendBackToTitle = StartCoroutine(SendBackToTitle());
    }

    private IEnumerator CheckPlayerHasFinished()
    {
        while (!hasPlayerWon)
        {
            yield return null;
        }

        EndCredits.SetActive(true);
        RollingCreditsController.PlayCredits();
    }

    private IEnumerator SendBackToTitle()
    {
        while (!EndCredits.activeInHierarchy)
        {
            yield return null;
        }

        yield return new WaitForSecondsRealtime(47);

        universalButtonList.ChangeSceneToTitle();
        yield return new WaitForSecondsRealtime(2);
        EndCredits.SetActive(false);
    }

    public void TurnBoolBackToFalse()
    {
        hasPlayerWon = false;
        StartCoroutine(TurnOffEndCredits());
    }

    private IEnumerator TurnOffEndCredits()
    {
        yield return new WaitForSecondsRealtime(1);
        EndCredits.SetActive(false);
    }
}
