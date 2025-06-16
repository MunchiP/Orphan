using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameCredits : MonoBehaviour
{
    public ButtonListManager universalButtonList;
    public GameObject EndCredits;
    public GameObject returnButton;
    public GameObject exitButton;
    public EscMenuBehaviour escScript;
    public RollingCreditsController RollingCreditsController;
    public GameObject thankYou;
    public bool hasPlayerWon = false;

    private Coroutine checkCoroutine;
    private Coroutine showButtons;

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
        if (showButtons != null) StopCoroutine(showButtons);
        
        EndCredits.SetActive(false);
        escScript.isCreditsActive = false;
        returnButton.SetActive(false);
        exitButton.SetActive(false);

        checkCoroutine = StartCoroutine(CheckPlayerHasFinished());
        showButtons = StartCoroutine(ShowTheButtons());
    }

    private IEnumerator CheckPlayerHasFinished()
    {
        while (!hasPlayerWon)
        {
            yield return null;
        }

        EndCredits.SetActive(true);
        escScript.isCreditsActive = true;
        RollingCreditsController.PlayCredits();
    }

    private IEnumerator ShowTheButtons()
    {
        while (!EndCredits.activeInHierarchy)
        {
            yield return null;
        }

        yield return new WaitForSecondsRealtime(47);

        returnButton.SetActive(true);
        exitButton.SetActive(true);
        universalButtonList.GoToEndCredits();
    }

    

   
}
