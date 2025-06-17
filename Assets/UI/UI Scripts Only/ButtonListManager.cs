using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class ButtonListManager : MonoBehaviour
{
    public PauseMenuNavigation navigation;

    // Title Buttons and Menus
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

    // InGame Buttons and Menus
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

    // Scene changes with fades
    public GameObject fadeImageObjectMainCanvas;
    public GameObject fadeImageObjectInGameCanvas;
    private FadeToBlack fadeToSceneScript;

    // Shared Buttons and Menus
    public GameObject universalControlLayoutTitle;
    public GameObject universalSoundPanelPause;
    public GameObject universalMusicButton;
    public GameObject universalSfxButton;
    public GameObject universalEscapeKey;
    private EscMenuBehaviour escapeKeyScript;

    // GameOver Buttons
    public GameObject returnFromGameOver;
    public GameObject exitFromGameOver;

    // EndCredits Buttons
    public GameObject returnFromCredits;
    public GameObject exitFromCredits;

    public bool isTitleScene;

    void Awake() { }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        fadeToSceneScript = fadeImageObjectInGameCanvas.GetComponent<FadeToBlack>();
        escapeKeyScript = universalEscapeKey.GetComponent<EscMenuBehaviour>();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        EventSystem.current.SetSelectedGameObject(null); // 👈 Fix selección fantasma
        isTitleScene = scene.buildIndex == 0;

        if (isTitleScene)
        {
            StartCoroutine(DelayedGoToTitleMenu());
            ResetPauseUI();
        }
        else
        {
            ResetPauseUI();
        }
    }

    public void QuitApplication()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public bool IsOnSubmenu =>
        SceneManager.GetActiveScene().buildIndex == 0 ? currentTitleMenu != 0 : currentPauseMenu != 0;

    public void ResetPauseUI()
    {
        EventSystem.current.SetSelectedGameObject(null); // 👈

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
        yield return null;
        GoToTitleMenu();
    }

    public void GoToTitleMenu()
    {
        EventSystem.current.SetSelectedGameObject(null); // 👈
        if (!isTitleScene) return;

        currentTitleMenu = 0;
        escapeKeyScript.onTitleMainMenu = true;
        navigation.buttonList.Clear();

        if (PlayerPrefs.GetInt("SavedGameExists") == 1)
        {
            continueButton.SetActive(true);
            navigation.buttonList.Add(continueButton);
        }
        else
        {
            continueButton.SetActive(false);
        }

        navigation.buttonList.Add(startButton);
        navigation.buttonList.Add(titleSettingsButton);
        navigation.buttonList.Add(titleCreditsButton);
        navigation.buttonList.Add(titleExitGameButton);

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
        EventSystem.current.SetSelectedGameObject(null); // 👈

        escapeKeyScript.onTitleMainMenu = false;
        currentTitleMenu = 1;
        navigation.buttonList.Clear();
        navigation.buttonList.Add(titleControlsButton);
        navigation.buttonList.Add(titleSoundButton);

        titleMainMenu.SetActive(false);
        titleSettingsMenu.SetActive(true);
        universalSoundPanelPause.SetActive(false);
        titleSoundButton.SetActive(true);
        universalSfxButton.SetActive(false);
        universalControlLayoutTitle.SetActive(false);

        navigation.RestartSelection(0);
        SelectFirstButtonSafe();
    }

    public void GoToTitleSoundBoard()
    {
        EventSystem.current.SetSelectedGameObject(null); // 👈

        escapeKeyScript.onTitleMainMenu = false;
        currentTitleMenu = 2;
        navigation.buttonList.Clear();
        navigation.buttonList.Add(universalMusicButton);
        navigation.buttonList.Add(universalSfxButton);

        titleSettingsMenu?.SetActive(false);
        universalSoundPanelPause.SetActive(true);
        titleSoundButton.SetActive(true);
        universalSfxButton.SetActive(true);

        navigation.RestartSelection(0);
        SelectFirstButtonSafe();
    }

    public void GoToTitleControls()
    {
        EventSystem.current.SetSelectedGameObject(null); // 👈

        escapeKeyScript.onTitleMainMenu = false;
        currentTitleMenu = 3;
        navigation.buttonList.Clear();

        titleSettingsMenu?.SetActive(false);
        universalControlLayoutTitle.SetActive(true);

        navigation.RestartSelection(0);
        SelectFirstButtonSafe();
    }

    public void GoToCredits()
    {
        EventSystem.current.SetSelectedGameObject(null); // 👈

        escapeKeyScript.onTitleMainMenu = false;
        currentTitleMenu = 1;
        navigation.buttonList.Clear();

        titleMainMenu?.SetActive(false);
        creditsMenu.SetActive(true);
        rollingCreditsController = creditsMenu.GetComponent<RollingCreditsController>();
        rollingCreditsController.StopAllCoroutines();
        rollingCreditsController.enabled = true;
        rollingCreditsController.PlayCredits();

        navigation.RestartSelection(0);
        SelectFirstButtonSafe();
    }

    public void GoBackTitleMenus()
    {
        EventSystem.current.SetSelectedGameObject(null); // 👈

        switch (currentTitleMenu)
        {
            case 4:
            case 1:
                GoToTitleMenu();
                StartCoroutine(SetTitleMainMenuTrueNextFrame());
                break;
            case 3:
            case 2:
                GoToSettingstTitleMenu();
                break;
        }
    }

    IEnumerator SetTitleMainMenuTrueNextFrame()
    {
        yield return null;
        escapeKeyScript.onTitleMainMenu = true;
    }

    public void ChangeSceneToInGame()
    {
        PlayerPrefs.DeleteAll();
        fadeToSceneScript = fadeImageObjectMainCanvas.GetComponent<FadeToBlack>();

        if (fadeToSceneScript != null)
        {
            EventSystem.current.SetSelectedGameObject(null); // 👈
            fadeToSceneScript.FadeToScene(1);
        }
        else
        {
            Debug.LogWarning("FadeImageObjectMainCanvas missing on reload.");
        }
    }

    public void ShowPauseMenu()
    {
        EventSystem.current.SetSelectedGameObject(null); // 👈

        if (isTitleScene) return;

        currentPauseMenu = 0;

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
        EventSystem.current.SetSelectedGameObject(null); // 👈

        escapeKeyScript.onPauseMainMenu = false;
        currentPauseMenu = 1;
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

        navigation.RestartSelection(0);
        SelectFirstButtonSafe();
    }

    public void GoToPauseSoundBoard()
    {
        EventSystem.current.SetSelectedGameObject(null); // 👈

        escapeKeyScript.onPauseMainMenu = false;
        currentPauseMenu = 2;
        navigation.buttonList.Clear();
        navigation.buttonList.Add(universalMusicButton);
        navigation.buttonList.Add(universalSfxButton);

        pauseSettingsMenu?.SetActive(false);
        universalSoundPanelPause.SetActive(true);
        pauseSoundButton.SetActive(true);
        universalSfxButton.SetActive(true);

        navigation.RestartSelection(0);
        SelectFirstButtonSafe();
    }

    public void GoToPauseControls()
    {
        EventSystem.current.SetSelectedGameObject(null); // 👈

        currentPauseMenu = 3;
        navigation.buttonList.Clear();

        pauseSettingsMenu?.SetActive(false);
        universalControlLayoutTitle.SetActive(true);

        navigation.RestartSelection(0);
        SelectFirstButtonSafe();
    }

    public void GoBackPauseMenus()
    {
        EventSystem.current.SetSelectedGameObject(null); // 👈

        switch (currentPauseMenu)
        {
            case 3:
            case 2:
                GoToPauseSettingsMenu();
                break;
            case 1:
                ShowPauseMenu();
                break;
        }
    }

    public void ChangeSceneToTitle()
    {
        if (Time.timeScale < 1)
            Time.timeScale = 1f;

        fadeToSceneScript = fadeImageObjectInGameCanvas.GetComponent<FadeToBlack>();
        if (fadeToSceneScript != null)
        {
            EventSystem.current.SetSelectedGameObject(null); // 👈
            fadeToSceneScript.FadeToScene(0, () =>
            {
                ResetPauseUI();
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
        EventSystem.current.SetSelectedGameObject(null); // 👈

        navigation.buttonList.Clear();
        navigation.buttonList.Add(returnFromGameOver);
        navigation.buttonList.Add(exitFromGameOver);

        navigation.RestartSelection(0);
        SelectFirstButtonSafe();
    }

    public void GoToEndCredits()
    {
        EventSystem.current.SetSelectedGameObject(null); // 👈

        navigation.buttonList.Clear();
        navigation.buttonList.Add(returnFromCredits);
        navigation.buttonList.Add(exitFromCredits);

        navigation.RestartSelection(0);
        SelectFirstButtonSafe();
    }

    public void ChangeSceneByIndex(int scene)
    {
        fadeToSceneScript = fadeImageObjectInGameCanvas.GetComponent<FadeToBlack>();
        if (fadeToSceneScript != null)
        {
            EventSystem.current.SetSelectedGameObject(null); // 👈
            fadeToSceneScript.FadeToScene(scene);
        }
        else
        {
            Debug.LogWarning("FadeImageObjectMainCanvas missing on reload.");
        }
    }

    public void ChangeSceneById(string scene)
    {
        fadeToSceneScript = fadeImageObjectInGameCanvas.GetComponent<FadeToBlack>();
        if (fadeToSceneScript != null)
        {
            EventSystem.current.SetSelectedGameObject(null); // 👈
            fadeToSceneScript.FadeToScene(scene);
        }
        else
        {
            Debug.LogWarning("FadeImageObjectMainCanvas missing on reload.");
        }
    }

    private void SelectFirstButtonSafe()
    {
        if (navigation.buttonList.Count == 0)
        {
            EventSystem.current.SetSelectedGameObject(null);
            Debug.LogWarning("[ButtonListManager] buttonList está VACÍA.");
            return;
        }

        EventSystem.current.SetSelectedGameObject(navigation.buttonList[0]);
    }
}
