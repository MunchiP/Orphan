using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;

public class ButtonListManager : MonoBehaviour
{
    public PauseMenuNavigation navigation;

    //Title Buttons and Menus
    public GameObject startButton;
    public GameObject continueButton;
    public GameObject titleSettingsButton;
    public GameObject titleCreditsButton;
    public GameObject titleExitGameButton;
    public GameObject titleControlsButton;
    public GameObject titleSoundButton;
    public GameObject titleMainMenu;
    public GameObject creditsMenu;
    public GameObject titleSettingsMenu;
    private int currentTitleMenu;

    //InGame Buttons and Menus
    public GameObject pauseContinueButton;
    public GameObject pauseSettingsButton;
    public GameObject pauseReturnTitleButton;
    public GameObject pauseExitGameButton;
    public GameObject pauseControlsButton;
    public GameObject pauseSoundButton;
    public GameObject pauseMainMenu;
    public GameObject pauseSettingsMenu;
    private RollingCreditsController rollingCreditsController;
    private int currentPauseMenu;
    public bool isFirstTimeOpeningPause = true;

    //scene changes with fades
    public GameObject fadeImageObjectMainCanvas;
    public GameObject fadeImageObjectInGameCanvas;
    private FadeToBlack fadeToSceneScript;

    //Shared Buttons and Menus
    public GameObject universalControlLayoutTitle;
    public GameObject universalSoundPanelPause;
    public GameObject universalMusicButton;
    public GameObject universalSfxButton;
    public GameObject universalEscapeKey;
    private EscMenuBehaviour escapeKeyScript;

    // GameOverButtons and Menus
    public GameObject returnFromGameOver;
    public GameObject exitFromGameOver;

    // EndCreditsButtons and Menus
    public GameObject returnFromCredits;
    public GameObject exitFromCredits;

    //behaviour variables
    public bool isTitleScene;

    public void Awake()
    {
        //
    }

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

        isTitleScene = scene.buildIndex == 0;

        if (isTitleScene)
        {
            StartCoroutine(DelayedGoToTitleMenu());
            ResetPauseUI(); // just in case we came from the game scene
        }
        else
        {
            ResetPauseUI(); // ensures pause UI is hidden when re-entering gameplay
        }
    }

    public void Start()
    {
        fadeToSceneScript = fadeImageObjectInGameCanvas.GetComponent<FadeToBlack>();
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

    public bool IsOnSubmenu
    {
        get
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                return currentTitleMenu != 0;
            }
            else if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                return currentPauseMenu != 0;
            }
            return false;
        }
    }

    public void ResetPauseUI()
    {
        pauseMainMenu?.SetActive(false);
        pauseSettingsMenu?.SetActive(false);
        universalSoundPanelPause?.SetActive(false);
        universalControlLayoutTitle?.SetActive(false);
        universalSfxButton?.SetActive(false);
        pauseSoundButton?.SetActive(false);
        pauseControlsButton?.SetActive(false);

        navigation.buttonList.Clear();
        isFirstTimeOpeningPause = true;
        currentPauseMenu = 0;

        if (escapeKeyScript == null)
            escapeKeyScript = universalEscapeKey.GetComponent<EscMenuBehaviour>();
    }

    IEnumerator DelayedGoToTitleMenu()
    {
        yield return null; // wait 1 frame
        GoToTitleMenu();
    }

    public void GoToTitleMenu()
    {
        if (!isTitleScene)
        {
            Debug.LogWarning("GoToTitleMenu called while in Game Scene. Ignoring.");
            return;
        }
        currentTitleMenu = 0;
        if (escapeKeyScript == null)
            escapeKeyScript = universalEscapeKey.GetComponent<EscMenuBehaviour>();
        escapeKeyScript.onTitleMainMenu = true;
        navigation.buttonList.Clear();
        if (PlayerPrefs.GetInt("SavedGameExists") == 1)
        {
            continueButton.SetActive(true);
            navigation.buttonList.Add(continueButton);
            navigation.buttonList.Add(startButton);
            navigation.buttonList.Add(titleSettingsButton);
            navigation.buttonList.Add(titleCreditsButton);
            navigation.buttonList.Add(titleExitGameButton);
        }
        else
        {
            continueButton.SetActive(false);
            navigation.buttonList.Add(startButton);
            navigation.buttonList.Add(titleSettingsButton);
            navigation.buttonList.Add(titleCreditsButton);
            navigation.buttonList.Add(titleExitGameButton);
        }
        titleMainMenu.SetActive(true);
        creditsMenu.SetActive(false);
        rollingCreditsController = creditsMenu.GetComponent<RollingCreditsController>();
        rollingCreditsController.enabled = false;
        titleSettingsMenu.SetActive(false);

        navigation.RestartSelection(0);
        SelectFirstButtonSafe();
    }

    public void GoToSettingstTitleMenu()
    {
        escapeKeyScript.onTitleMainMenu = false;

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
        SelectFirstButtonSafe();
    }

    public void GoToTitleSoundBoard()
    {
        escapeKeyScript.onTitleMainMenu = false;

        navigation.buttonList.Clear();
        navigation.buttonList.Add(universalMusicButton);
        navigation.buttonList.Add(universalSfxButton);

        titleSettingsMenu?.SetActive(false);
        universalSoundPanelPause.SetActive(true);
        titleSoundButton.SetActive(true);
        universalSfxButton.SetActive(true);
        currentTitleMenu = 2;

        navigation.RestartSelection(0);
        SelectFirstButtonSafe();
    }

    public void GoToTitleControls()
    {
        escapeKeyScript.onTitleMainMenu = false;

        navigation.buttonList.Clear();

        titleSettingsMenu?.SetActive(false);
        universalControlLayoutTitle.SetActive(true);
        currentTitleMenu = 3;

        navigation.RestartSelection(0);
        SelectFirstButtonSafe();
    }

    public void GoToCredits()
    {
        escapeKeyScript.onTitleMainMenu = false;

        navigation.buttonList.Clear();

        titleMainMenu?.SetActive(false);

        creditsMenu.SetActive(true);
        rollingCreditsController = creditsMenu.GetComponent<RollingCreditsController>();
        rollingCreditsController.StopAllCoroutines();
        rollingCreditsController.enabled = true;
        rollingCreditsController.PlayCredits();
        creditsMenu.GetComponent<RollingCreditsController>().PlayCredits();

        currentTitleMenu = 1;

        navigation.RestartSelection(0);
        SelectFirstButtonSafe();
    }

    public void GoBackTitleMenus()
    {
        switch (currentTitleMenu)
        {
            case 4:
                GoToTitleMenu();
                break;
            case 3:
                GoToSettingstTitleMenu();
                break;
            case 2:
                GoToSettingstTitleMenu();
                break;
            case 1:
                GoToTitleMenu();
                StartCoroutine(SetTitleMainMenuTrueNextFrame());
                break;
        }
    }

    private IEnumerator SetTitleMainMenuTrueNextFrame()
    {
        yield return null; // Wait 1 frame
        escapeKeyScript.onTitleMainMenu = true;
    }

    public void ChangeSceneToInGame()
    {
        PlayerPrefs.DeleteAll();
        fadeToSceneScript = fadeImageObjectMainCanvas.GetComponent<FadeToBlack>();
        if (fadeToSceneScript != null)
        {
            fadeToSceneScript.FadeToScene(1);
        }
        else
        {
            Debug.LogWarning("FadeImageObjectMainCanvas missing on reload.");
        }
    }

    public void ShowPauseMenu()
    {
        if (isTitleScene)
        {
            Debug.LogWarning("ShowPauseMenu called while in Title Scene. Ignoring.");
            return;
        }
        currentPauseMenu = 0;
        if (escapeKeyScript == null)
            escapeKeyScript = universalEscapeKey.GetComponent<EscMenuBehaviour>();

        if (isFirstTimeOpeningPause)
        {
            escapeKeyScript.onPauseMainMenu = true;
            isFirstTimeOpeningPause = false;
        }
        escapeKeyScript.onPauseMainMenu = true;
        navigation.buttonList.Clear();

        navigation.buttonList.Add(pauseContinueButton);
        navigation.buttonList.Add(pauseSettingsButton);
        navigation.buttonList.Add(pauseReturnTitleButton);
        navigation.buttonList.Add(pauseExitGameButton);

        pauseMainMenu.SetActive(true);
        pauseSettingsMenu.SetActive(false);

        navigation.RestartSelection(0);
        SelectFirstButtonSafe();
    }

    public void GoToPauseSettingsMenu()
    {
        escapeKeyScript.onPauseMainMenu = false;

        navigation.buttonList.Clear();
        navigation.buttonList.Add(pauseControlsButton);
        navigation.buttonList.Add(pauseSoundButton);

        pauseMainMenu.SetActive(false);
        pauseSettingsMenu.SetActive(true);
        universalSoundPanelPause.SetActive(false);
        pauseControlsButton.SetActive(true);
        pauseSoundButton.SetActive(true);
        universalSfxButton.SetActive(false);
        universalControlLayoutTitle.SetActive(false);

        currentPauseMenu = 1;

        navigation.RestartSelection(0);
        SelectFirstButtonSafe();
    }

    public void GoToPauseSoundBoard()
    {
        escapeKeyScript.onPauseMainMenu = false;

        navigation.buttonList.Clear();
        navigation.buttonList.Add(universalMusicButton);
        navigation.buttonList.Add(universalSfxButton);

        pauseSettingsMenu?.SetActive(false);
        universalSoundPanelPause.SetActive(true);
        pauseSoundButton.SetActive(true);
        universalSfxButton.SetActive(true);
        currentPauseMenu = 2;

        navigation.RestartSelection(0);
        SelectFirstButtonSafe();
    }

    public void GoToPauseControls()
    {
        navigation.buttonList.Clear();

        pauseSettingsMenu?.SetActive(false);
        universalControlLayoutTitle.SetActive(true);
        currentPauseMenu = 3;

        navigation.RestartSelection(0);
        SelectFirstButtonSafe();
    }

    public void GoBackPauseMenus()
    {
        switch (currentPauseMenu)
        {
            case 3:
                GoToPauseSettingsMenu();
                break;
            case 2:
                GoToPauseSettingsMenu();
                break;
            case 1:
                ShowPauseMenu();
                break;
        }
    }

    private IEnumerator SetPauseMainMenuTrueNextFrame()
    {
        yield return null; // Wait 1 frame
        escapeKeyScript.onPauseMainMenu = true;
    }

    public void ChangeSceneToTitle()
    {

        fadeToSceneScript = fadeImageObjectInGameCanvas.GetComponent<FadeToBlack>();
        if (fadeToSceneScript != null)
        {
            fadeToSceneScript.FadeToScene(0, () =>
            {
                pauseMainMenu.SetActive(false);
                pauseSettingsMenu.SetActive(false);
                universalSoundPanelPause.SetActive(false);
                universalControlLayoutTitle.SetActive(false);
            });
        }
        else
        {
            Debug.LogWarning("FadeToBlack missing on reload.");
        }
    }

    public void GoBack()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            GoBackTitleMenus();
        else
            GoBackPauseMenus();
    }

    public void GameOver()
    {
        navigation.buttonList.Clear();
        navigation.buttonList.Add(returnFromGameOver);
        navigation.buttonList.Add(exitFromGameOver);

        navigation.RestartSelection(0);
        SelectFirstButtonSafe();
    }

    public void GoToEndCredits()
    {
        navigation.buttonList.Clear();
        navigation.buttonList.Add(returnFromCredits);
        navigation.buttonList.Add(exitFromCredits);

        navigation.RestartSelection(0);
        SelectFirstButtonSafe();
    }

    public void ChangeSceneByIndex(int scene)
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;

        fadeToSceneScript = fadeImageObjectInGameCanvas.GetComponent<FadeToBlack>();
        if (fadeToSceneScript != null)
        {
            fadeToSceneScript.FadeToScene(scene);
        }
        else
        {
            Debug.LogWarning("FadeImageObjectMainCanvas missing on reload.");
        }
    }

    public void ChangeSceneById(string scene)
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;

        fadeToSceneScript = fadeImageObjectInGameCanvas.GetComponent<FadeToBlack>();
        if (fadeToSceneScript != null)
        {
            fadeToSceneScript.FadeToScene(scene);
        }
        else
        {
            Debug.LogWarning("FadeImageObjectMainCanvas missing on reload.");
        }
    }


    // Helper method para selección segura del primer botón en la lista
    private void SelectFirstButtonSafe()
    {
        if (navigation.buttonList.Count == 0)
        {
            Debug.LogWarning("[ButtonListManager] buttonList está VACÍA, no se puede seleccionar ningún botón.");
            EventSystem.current.SetSelectedGameObject(null);
            return;
        }

        EventSystem.current.SetSelectedGameObject(navigation.buttonList[0]);
    }
}
