using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public GameObject gameOverScreen;
    public EscMenuBehaviour escScript;
    public ButtonListManager universalButtonListScript;
    public bool hasPlayerLost;

    private Coroutine checkPlayerStatus;
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
        hasPlayerLost = false;
        gameOverScreen.SetActive(false);

        // Start coroutines again after scene load
        if (checkPlayerStatus != null) StopCoroutine(checkPlayerStatus);

        escScript.isGameOverActive = false;

        checkPlayerStatus = StartCoroutine(CheckPlayerHasLost());
        
    }

    private IEnumerator CheckPlayerHasLost()
    {
        while(!hasPlayerLost)
        {
            yield return null;
        }
        gameOverScreen.SetActive(true);
        escScript.isGameOverActive = true;
        universalButtonListScript.GameOver();

    }
}
