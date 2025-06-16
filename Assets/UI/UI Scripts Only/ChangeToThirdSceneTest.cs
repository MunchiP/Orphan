using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class ChangeToThirdSceneTest : MonoBehaviour
{
    public ButtonListManager universalButtonList;
    public bool hasPlayerEnterBossFight;


    private Coroutine checkPlayer;
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
        hasPlayerEnterBossFight = false;
        

        // Start coroutines again after scene load
        if (checkPlayer != null) StopCoroutine(checkPlayer);

        checkPlayer = StartCoroutine(CheckPlayerEnteringBossFight());

    }

    private IEnumerator CheckPlayerEnteringBossFight()
    {
        while (!hasPlayerEnterBossFight)
        {
            yield return null;
        }
        universalButtonList.ChangeSceneToBossFight();

    }
}
