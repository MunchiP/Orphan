using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.EventSystems;

public class TitleButtonListManager : MonoBehaviour
{
    public PauseMenuNavigation navigation;
    public GameObject startButton;
    public GameObject titleSettingsButton;
    public GameObject titleCreditsButton;
    public GameObject titleExitGameButton;
    public GameObject titleControlsButton;
    public GameObject titleSoundButton;

    public GameObject titleMainMenu;
    public GameObject creditsMenu;
    public GameObject titleSettingsMenu;
    public GameObject universalControlLayoutTitle;
    public GameObject universalSoundPanelPause;
    public GameObject universalMusicButton;
    public GameObject universalSfxButton;
    public GameObject universalEscapeKey;
    private EscMenuBehaviour escapeKeyScript;
    private int currentTitleMenu;
    public GameObject fadeImageObjectMainCanvas;
    private FadeToBlack fadeToScene1Script;
    

    public void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            return;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            GoToTitleMenu();
        }
    }

    public void Start()
    {
        GoToTitleMenu();
        fadeToScene1Script = fadeImageObjectMainCanvas.GetComponent<FadeToBlack>();
        escapeKeyScript = universalEscapeKey.GetComponent<EscMenuBehaviour>();
    }

    public void QuitApplication()
    {

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif


    }

    public void GoToTitleMenu()
    {
        if (escapeKeyScript == null)
            escapeKeyScript = universalEscapeKey.GetComponent<EscMenuBehaviour>();

        navigation.buttonList.Clear();


        navigation.buttonList.Add(startButton);
        navigation.buttonList.Add(titleSettingsButton);
        navigation.buttonList.Add(titleCreditsButton);
        navigation.buttonList.Add(titleExitGameButton);


        titleMainMenu.SetActive(true);
        creditsMenu.SetActive(false);
        titleSettingsMenu.SetActive(false);



        navigation.RestartSelection(0);
        EventSystem.current.SetSelectedGameObject(navigation.buttonList[0]);
    }

    public void GoToSettingstTitleMenu()
    {
        escapeKeyScript.onTitleMainMenu = false;

        navigation.BlockSubmit();

        navigation.buttonList.Clear();
        navigation.buttonList.Add(titleControlsButton);
        navigation.buttonList.Add(titleSoundButton);

        titleMainMenu.SetActive(false);
        titleSettingsMenu.SetActive(true);
        universalSoundPanelPause.SetActive(false);
        titleSoundButton.SetActive(true);
        universalSfxButton.SetActive(false);
        universalControlLayoutTitle.SetActive(false);

        currentTitleMenu = 1;

        navigation.RestartSelection(0);
        EventSystem.current.SetSelectedGameObject(navigation.buttonList[0]);


    }



    public void GoToTitleSoundBoard()
    {
        escapeKeyScript.onTitleMainMenu = false;

        navigation.BlockSubmit();

        navigation.buttonList.Clear();
        navigation.buttonList.Add(universalMusicButton);
        navigation.buttonList.Add(universalSfxButton);


        titleSettingsMenu?.SetActive(false);
        universalSoundPanelPause.SetActive(true);
        titleSoundButton.SetActive(true);
        universalSfxButton.SetActive(true);
        currentTitleMenu = 2;

        navigation.RestartSelection(0);
        EventSystem.current.SetSelectedGameObject(navigation.buttonList[0]);

    }

    public void GoToTitleControls()
    {
        escapeKeyScript.onTitleMainMenu = false;

        navigation.BlockSubmit();
        navigation.buttonList.Clear();

        titleSettingsMenu?.SetActive(false);
        universalControlLayoutTitle.SetActive(true);
        currentTitleMenu = 3;

        navigation.RestartSelection(0);
        EventSystem.current.SetSelectedGameObject(navigation.buttonList[0]);
    }

    public void GoToCredits()
    {
        escapeKeyScript.onTitleMainMenu= false;
        navigation.BlockSubmit();
        navigation.buttonList.Clear();
                
        titleMainMenu?.SetActive(false);
        creditsMenu.SetActive(true);
        currentTitleMenu = 1;

        navigation.RestartSelection(0);
        EventSystem.current.SetSelectedGameObject(navigation.buttonList[0]);
    }




    public void GoBackTitleMenus()
    {
        switch (currentTitleMenu)
        {
            case 4:
                {
                    GoToTitleMenu();
                    StartCoroutine(SetTitleMainMenuTrueNextFrame());
                    break;
                }
            case 3:
                {
                    GoToSettingstTitleMenu();
                    break;
                }
            case 2:
                {
                    GoToSettingstTitleMenu();
                    break;
                }
            case 1:
                {

                    GoToTitleMenu();
                    StartCoroutine(SetTitleMainMenuTrueNextFrame());
                    break;
                }
        }
    }
    private IEnumerator SetTitleMainMenuTrueNextFrame()
    {
        yield return null; // Wait 1 frame
        escapeKeyScript.onTitleMainMenu = true;
        Debug.Log("[MenuButtonListManager] onTitleMainMenu set to TRUE after returning to pause menu.");
    }

    public void ChangeSceneToInGame()
    {

        Debug.Log("[MenuButtonListManager] Attempting to change scene to In Game...");
        fadeToScene1Script = fadeImageObjectMainCanvas.GetComponent<FadeToBlack>();
        if (fadeToScene1Script != null)
        {
            Debug.Log("[MenuButtonListManager] Fading to scene 1.");
            fadeToScene1Script.FadeToScene(1);

            
        }
        else
        {
            //Debug.LogWarning("FadeImageObjectMainCanvas missing on reload.");
        }

    }
}
