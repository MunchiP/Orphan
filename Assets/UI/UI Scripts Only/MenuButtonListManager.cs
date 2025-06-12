using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MenuButtonListManager : MonoBehaviour
{
    public PauseMenuNavigation navigation;
    public GameObject continueButton;
    public GameObject settingsButton;
    public GameObject returnTitleButton;
    public GameObject exitGameButton;
    public GameObject controlsButton;
    public GameObject soundButton;
    
    public GameObject mainPauseMenu;
    public GameObject settingsPauseMenu;
    public GameObject controlLayoutPause;
    public GameObject soundPanelPause;
    public GameObject musicButton;
    public GameObject sfxButton;
    public GameObject escapeKey;
    private EscMenuBehaviour escapeKeyScript;
    private int currentPauseMenu;
    public bool isFirstTimeOpeningPause = true;
    public GameObject fadeToBlackObject;
    public GameObject GameManager;
    private FadeToBlack fadetoBlackScript;
    private TitleSceneAndButtonFunction titleSceneAndButtonFunction;


    public void Start()
    {
        titleSceneAndButtonFunction = GameManager.GetComponent<TitleSceneAndButtonFunction>();    
        fadetoBlackScript = fadeToBlackObject.GetComponent<FadeToBlack>();
        escapeKeyScript = escapeKey.GetComponent<EscMenuBehaviour>();
    }

    public void QuitApplication()
    {

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif


    }

    public void ShowPauseMenu()
    {
        if (escapeKeyScript == null)
        escapeKeyScript = escapeKey.GetComponent<EscMenuBehaviour>();

        if (isFirstTimeOpeningPause)
        {
            escapeKeyScript.onPauseMainMenu = true;
            isFirstTimeOpeningPause = false;
        }
        navigation.buttonList.Clear();


        navigation.buttonList.Add(continueButton);
        navigation.buttonList.Add(settingsButton);
        navigation.buttonList.Add(returnTitleButton);
        navigation.buttonList.Add(exitGameButton);


        mainPauseMenu.SetActive(true);
        settingsPauseMenu.SetActive(false);
        


        navigation.RestartSelection(0);
    }

    public void GoToSettingsMenu()
    {
        escapeKeyScript.onPauseMainMenu = false;

        navigation.BlockSubmit();

        navigation.buttonList.Clear();
        navigation.buttonList.Add(controlsButton);
        navigation.buttonList.Add(soundButton);

        mainPauseMenu.SetActive(false);
        settingsPauseMenu.SetActive(true);
        soundPanelPause.SetActive(false);
        soundButton.SetActive(true);
        sfxButton.SetActive(false);
        controlLayoutPause.SetActive(false);

        currentPauseMenu = 1;

        navigation.RestartSelection(0);

        StartCoroutine(UnblockSubmitNextFrame());

    }

    private IEnumerator UnblockSubmitNextFrame()
    {
        yield return null; 
        navigation.UnblockSubmit();
    }

    public void GoToSoundBoard()
    {
        navigation.BlockSubmit();
        navigation.buttonList.Clear();

        navigation.buttonList.Add(musicButton);
        navigation.buttonList.Add(sfxButton);
        

        settingsPauseMenu?.SetActive(false);
        soundPanelPause.SetActive(true);
        soundButton.SetActive(true);
        sfxButton.SetActive(true);
        currentPauseMenu = 2;

        navigation.RestartSelection(0);
        StartCoroutine(UnblockSubmitNextFrame());
    }

    public void GoToControls()
    {
        navigation.buttonList.Clear();

        
        

        settingsPauseMenu?.SetActive(false);
        controlLayoutPause.SetActive(true);
        currentPauseMenu = 3;

        navigation.RestartSelection(0);
    }




    public void GoBack()
    { 
        switch(currentPauseMenu)
        {
            case 3:
                {
                    GoToSettingsMenu();
                    break;
                }
            case 2:
                {
                    GoToSettingsMenu();
                    break;
                }
            case 1:
                {
                    
                    ShowPauseMenu();
                    StartCoroutine(SetPauseMainMenuTrueNextFrame());
                    break ;
                }
        }    
    }
    private IEnumerator SetPauseMainMenuTrueNextFrame()
    {
        yield return null; // Wait 1 frame
        escapeKeyScript.onPauseMainMenu = true;
        Debug.Log("[MenuButtonListManager] onPauseMainMenu set to TRUE after returning to pause menu.");
    }

    public void ChangeSceneToTitle()
    {

        Debug.Log("[MenuButtonListManager] Attempting to change scene to title...");
        fadetoBlackScript = fadeToBlackObject.GetComponent<FadeToBlack>();
        if (fadetoBlackScript != null)
        {
            Debug.Log("[MenuButtonListManager] Fading to scene 0.");
            fadetoBlackScript.FadeToScene(0);

            titleSceneAndButtonFunction.enabled = true;
        }
        else
        {
            //Debug.LogWarning("FadeToBlack missing on reload.");
        }

    }
}
