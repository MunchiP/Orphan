using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButtonListManager : MonoBehaviour
{
    public PauseMenuNavigation navigation;
    public GameObject continueButton;
    public GameObject settingsButton;
    public GameObject returnTitleButton;
    public GameObject exitGameButton;
    public GameObject pauseMenu;
    public GameObject controlsButton;
    public GameObject soundButton;
    public GameObject goBackButton;
    public GameObject mainPauseMenu;
    public GameObject settingsPauseMenu;
    public GameObject controlLayoutPause;
    public GameObject soundPanelPause;
    public GameObject musicButton;
    public GameObject sfxButton;
    private int currentPauseMenu;
    public static MenuButtonListManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ShowPauseMenu()
    {
        navigation.SetMouseHoverState(false);
        navigation.buttonList.Clear();
        navigation.buttonList.Add(continueButton);
        navigation.buttonList.Add(settingsButton);
        navigation.buttonList.Add(returnTitleButton);
        navigation.buttonList.Add(exitGameButton);

        mainPauseMenu.SetActive(true);
        settingsPauseMenu.SetActive(false);
        goBackButton.SetActive(false);
        controlLayoutPause.SetActive(false);
        soundPanelPause.SetActive(false);

        navigation.RestartSelection(0);
    }

    public void GoToSettingsMenu()
    {
        navigation.SetMouseHoverState(false);
        navigation.buttonList.Clear();
        navigation.buttonList.Add(controlsButton);
        navigation.buttonList.Add(soundButton);
        navigation.buttonList.Add(goBackButton);

        mainPauseMenu.SetActive(false);
        settingsPauseMenu.SetActive(true);
        soundPanelPause.SetActive(false);
        controlLayoutPause.SetActive(false);
        soundButton.SetActive(true);
        goBackButton.SetActive(true);

        currentPauseMenu = 1;
        navigation.RestartSelection(0);
    }

    public void GoToSoundBoard()
    {
        navigation.SetMouseHoverState(false);
        navigation.buttonList.Clear();
        navigation.buttonList.Add(musicButton);
        navigation.buttonList.Add(sfxButton);
        navigation.buttonList.Add(goBackButton);

        settingsPauseMenu?.SetActive(false);
        soundPanelPause.SetActive(true);
        soundButton.SetActive(true);
        sfxButton.SetActive(true);

        currentPauseMenu = 2;
        navigation.RestartSelection(0);
    }

    public void GoToControls()
    {
        navigation.SetMouseHoverState(false);
        navigation.buttonList.Clear();
        navigation.buttonList.Add(goBackButton);

        mainPauseMenu.SetActive(false);
        settingsPauseMenu?.SetActive(false);
        soundPanelPause?.SetActive(false);
        controlLayoutPause.SetActive(true);
        goBackButton.SetActive(true);

        currentPauseMenu = 3;
        navigation.RestartSelection(0);
    }

    public void GoContinue()
    {
        if (mainPauseMenu.activeInHierarchy)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void GoToTittle()
    {
        TitleSceneAndButtonFunction title = FindAnyObjectByType<TitleSceneAndButtonFunction>();
        title.dataPersistanceTestNumber = 0;
        pauseMenu.SetActive(false);
        SceneManager.LoadScene(0);
    }

    public void GoBack()
    {
        switch (currentPauseMenu)
        {
            case 3:
            case 2:
                GoToSettingsMenu();
                break;
            case 1:
                ShowPauseMenu();
                break;
        }
    }
}
