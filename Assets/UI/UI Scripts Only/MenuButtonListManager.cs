using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.UI;

public class MenuButtonListManager : MonoBehaviour
{
    public PauseMenuNavigation navigation;
    public GameObject continueButton;
    public GameObject settingsButton;
    public GameObject returnTitleButton;
    public GameObject exitGameButton;
    public GameObject controlsButton;
    public GameObject soundButton;
    public GameObject goBackButton;
    public GameObject mainPauseMenu;
    public GameObject settingsPauseMenu;
    public GameObject controlLayoutPause;
    public GameObject soundPanelPause;
    public GameObject musicButton;
    public GameObject sfxButton;
    public GameObject escapeKey;
    private EscMenuBehaviour escapeKeyScript;
    private int currentPauseMenu;

    public void Start()
    {
        escapeKeyScript = escapeKey.GetComponent<EscMenuBehaviour>();
    }

    public void ShowPauseMenu()
    {
        escapeKeyScript.onPauseMainMenu = true;
        navigation.buttonList.Clear();


        navigation.buttonList.Add(continueButton);
        navigation.buttonList.Add(settingsButton);
        navigation.buttonList.Add(returnTitleButton);
        navigation.buttonList.Add(exitGameButton);


        mainPauseMenu.SetActive(true);
        settingsPauseMenu.SetActive(false);
        goBackButton.SetActive(false);


        navigation.RestartSelection(0);
    }

    public void GoToSettingsMenu()
    {
        escapeKeyScript.onPauseMainMenu = false;
        navigation.buttonList.Clear();

        navigation.buttonList.Add(controlsButton);
        navigation.buttonList.Add(soundButton);
        navigation.buttonList.Add(goBackButton);

        mainPauseMenu.SetActive(false);
        settingsPauseMenu.SetActive(true);
        soundPanelPause.SetActive(false);
        soundButton.SetActive(true);
        sfxButton.SetActive(false);
        controlLayoutPause.SetActive(false);


        goBackButton.SetActive(true);
        currentPauseMenu = 1;

        navigation.RestartSelection(0);

    }

    public void GoToSoundBoard()
    {
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
        navigation.buttonList.Clear();

        
        navigation.buttonList.Add(goBackButton);

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
                    break ;
                }
        }    
    }
}
