using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class TitleSceneAndButtonFunction : MonoBehaviour
{

    private FadeToBlack fadeController;
    private GameObject startButton;
    private GameObject quitButton;
    private GameObject creditsButton;
    public GameObject goBackButton;
    public GameObject settingsButton;
    public GameObject leftPaw;
    public GameObject rightPaw;
    public GameObject UINavigationManager;
    public GameObject settingsUINavigationManager;
    public GameObject soundPanelUINavigationManager;
    public GameObject oneButtonNavigation;
    public GameObject titleMenu;
    public GameObject creditRoller;
    public GameObject settingsMenu;
    public GameObject controlLayout;
    public GameObject controlsButton;
    public GameObject soundMenu;
    public GameObject soundButton;
    public int dataPersistanceTestNumber;
    private string currentMenu;
    private bool isGoBackTransitioning = false;
    private HorizontalOnlyNavigation horizontalNavigationScript;
    private VerticalOnlyNavigation titleMenuVerticalNavigationScript;
    private VerticalOnlyNavigation settingsMenuVerticalNavigationScript;
    private VerticalOnlyNavigation soundPanelVerticalNavigationScript;
    private VerticalOnlyNavigation oneButtonVerticalNavigationScript;
    private TitleSceneAndButtonFunction titleAndButtonScript;




    public void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (SceneManager.GetActiveScene().buildIndex != 0) return;
        titleMenuVerticalNavigationScript = UINavigationManager.GetComponent<VerticalOnlyNavigation>();
        horizontalNavigationScript = UINavigationManager.GetComponent<HorizontalOnlyNavigation>();
        settingsMenuVerticalNavigationScript = settingsUINavigationManager.GetComponent<VerticalOnlyNavigation>();
        soundPanelVerticalNavigationScript = soundPanelUINavigationManager.GetComponent<VerticalOnlyNavigation>();
        oneButtonVerticalNavigationScript = oneButtonNavigation.GetComponent<VerticalOnlyNavigation>();
    }

    public void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != 0) return;

        else if (scene.buildIndex == 0)
        {
            fadeController = GetComponent<FadeToBlack>();
            startButton = GameObject.FindGameObjectWithTag("StartButtonUI");
            quitButton = GameObject.FindGameObjectWithTag("ExitButtonUI");
            creditsButton = GameObject.FindGameObjectWithTag("CreditsButtonUI");
            titleMenuVerticalNavigationScript = UINavigationManager.GetComponent<VerticalOnlyNavigation>();
            horizontalNavigationScript = UINavigationManager.GetComponent<HorizontalOnlyNavigation>();
            settingsMenuVerticalNavigationScript = settingsUINavigationManager.GetComponent<VerticalOnlyNavigation>();
            soundPanelVerticalNavigationScript = soundPanelUINavigationManager.GetComponent<VerticalOnlyNavigation>();
            oneButtonVerticalNavigationScript = oneButtonNavigation.GetComponent<VerticalOnlyNavigation>();
            titleMenu.SetActive(true);
            var buttonScripts = titleMenu.GetComponentsInChildren<ButtonIndexController>(true);
            foreach (var script in buttonScripts)
            {
                script.SetKeyboardController(titleMenuVerticalNavigationScript);
            }
            creditRoller.SetActive(false);
            settingsMenu.SetActive(false);
            goBackButton.SetActive(false);
            controlLayout.SetActive(false);
            soundMenu.SetActive(false);
            soundPanelVerticalNavigationScript.enabled = false;
            AudioManager.instance.PlayMusic("MainTheme");

            if (startButton != null)
            {
                Button start = startButton.GetComponent<Button>();
                start.onClick.RemoveAllListeners();
                start.onClick.AddListener(() => ChangeSceneToNewGame());
            }

            if (startButton == null)
            {
                Debug.Log("startButton not found in current scene");
            }

            if (quitButton != null)
            {
                Button quit = quitButton.GetComponent<Button>();
                quit.onClick.RemoveAllListeners();
                quit.onClick.AddListener(() => QuitApplication());
            }

            if (creditsButton != null)
            {
                Button credits = creditsButton.GetComponent<Button>();
                credits.onClick.RemoveAllListeners();
                credits.onClick.AddListener(() => GoToCredits());
            }

            if (goBackButton != null)
            {
                Button goBack = goBackButton.GetComponent<Button>();
                goBack.onClick.RemoveAllListeners();
                goBack.onClick.AddListener(() => GoBackToTitle());
            }

            if (settingsButton != null)
            {
                Button settings = settingsButton.GetComponent<Button>();
                settings.onClick.RemoveAllListeners();
                settings.onClick.AddListener(() => GoToSettings());
            }

            if (controlsButton != null)
            {
                Button controls = controlsButton.GetComponent<Button>();
                controls.onClick.RemoveAllListeners();
                controls.onClick.AddListener(() => GoToControlLayout());
            }

            if (soundButton != null)
            {
                Button sound = soundButton.GetComponent<Button>();
                sound.onClick.RemoveAllListeners();
                sound.onClick.AddListener(() => GoToSoundPanel());
            }
        }



    }

    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0) return;
        AudioManager.instance.PlayMusic("MainTheme");
        dataPersistanceTestNumber = 0;
        fadeController = GetComponent<FadeToBlack>();
        goBackButton.SetActive(false);
        controlLayout.SetActive(false);
        titleMenuVerticalNavigationScript = UINavigationManager.GetComponent<VerticalOnlyNavigation>();
        horizontalNavigationScript = UINavigationManager.GetComponent<HorizontalOnlyNavigation>();
        settingsMenuVerticalNavigationScript = settingsUINavigationManager.GetComponent<VerticalOnlyNavigation>();
        soundPanelVerticalNavigationScript = soundPanelUINavigationManager.GetComponent<VerticalOnlyNavigation>();
        oneButtonVerticalNavigationScript = oneButtonNavigation.GetComponent<VerticalOnlyNavigation>();
        soundPanelVerticalNavigationScript.enabled = false;
        soundMenu.SetActive(false);
    }

    public void GoToSettings()
    {
        currentMenu = "SettingsMenu";

        var buttonScripts = settingsMenu.GetComponentsInChildren<ButtonIndexController>(true);
        foreach (var script in buttonScripts)
        {
            script.SetKeyboardController(settingsMenuVerticalNavigationScript);
        }
        ButtonIndexController buttonIndexController = goBackButton.GetComponent<ButtonIndexController>();
        buttonIndexController.SetKeyboardController(settingsMenuVerticalNavigationScript);

        if (!settingsMenuVerticalNavigationScript.buttonList.Contains(goBackButton))
        {
            settingsMenuVerticalNavigationScript.buttonList.Add(goBackButton);
            soundPanelVerticalNavigationScript.buttonList.Remove(goBackButton);
            oneButtonVerticalNavigationScript.buttonList.Remove(goBackButton);
        }

        titleMenu.SetActive(false);
        settingsMenu.SetActive(true);
        goBackButton.SetActive(true);
        titleMenuVerticalNavigationScript.enabled = false;
        settingsMenuVerticalNavigationScript.enabled = true;
        oneButtonVerticalNavigationScript.enabled = false;
        soundPanelVerticalNavigationScript.enabled = false;




    }

    public void GoToControlLayout()
    {
        currentMenu = "SettingsControlSubMenu";

        ButtonIndexController buttonIndexController = goBackButton.GetComponent<ButtonIndexController>();
        buttonIndexController.SetKeyboardController(oneButtonVerticalNavigationScript);
        if (!oneButtonVerticalNavigationScript.buttonList.Contains(goBackButton))
        {
            oneButtonVerticalNavigationScript.buttonList.Add(goBackButton);
            settingsMenuVerticalNavigationScript.buttonList.Remove(goBackButton);
        }

        settingsMenuVerticalNavigationScript.enabled = false;
        oneButtonVerticalNavigationScript.enabled = true;
        settingsMenu.SetActive(false);
        controlLayout.SetActive(true);
    }

    public void GoToSoundPanel()
    {
        currentMenu = "SettingsSoundSubMenu";

        var buttonScripts = soundMenu.GetComponentsInChildren<ButtonIndexController>(true);
        foreach (var script in buttonScripts)
        {
            script.SetKeyboardController(soundPanelVerticalNavigationScript);
        }

        ButtonIndexController buttonIndexController = goBackButton.GetComponent<ButtonIndexController>();
        buttonIndexController.SetKeyboardController(soundPanelVerticalNavigationScript);
        if (!soundPanelVerticalNavigationScript.buttonList.Contains(goBackButton))
        {
            soundPanelVerticalNavigationScript.buttonList.Add(goBackButton);
            settingsMenuVerticalNavigationScript.buttonList.Remove(goBackButton);
        }

        settingsMenu.SetActive(false);
        settingsMenuVerticalNavigationScript.enabled = false;
        soundPanelVerticalNavigationScript.enabled = true;
        soundMenu.SetActive(true);
    }
    public void GoToCredits()
    {
        currentMenu = "CreditsMenu";

        ButtonIndexController buttonIndexController = goBackButton.GetComponent<ButtonIndexController>();
        buttonIndexController.SetKeyboardController(oneButtonVerticalNavigationScript);
        if (!oneButtonVerticalNavigationScript.buttonList.Contains(goBackButton))
        {
            oneButtonVerticalNavigationScript.buttonList.Add(goBackButton);
        }

        titleMenu.SetActive(false);
        creditRoller.SetActive(true);
        goBackButton.SetActive(true);
        titleMenuVerticalNavigationScript.enabled = false;
        oneButtonVerticalNavigationScript.enabled = true;


    }

    public void GoBackToTitle()
    {
        if (isGoBackTransitioning)
        {
            Debug.Log("GoBackToTitle blocked by isTransitioning.");
            return;
        }
        Debug.Log("GoBackToTitle starting.");
        isGoBackTransitioning = true;
        Debug.Log("GoBackToTitle - currentMenu: " + currentMenu);
        titleMenuVerticalNavigationScript = UINavigationManager.GetComponent<VerticalOnlyNavigation>();
        horizontalNavigationScript = UINavigationManager.GetComponent<HorizontalOnlyNavigation>();
        switch (currentMenu)
        {
            case "SettingsMenu":
                {
                    titleMenu.SetActive(true);

                    var buttonScripts = titleMenu.GetComponentsInChildren<ButtonIndexController>(true);
                    foreach (var script in buttonScripts)
                    {
                        script.SetKeyboardController(titleMenuVerticalNavigationScript);
                    }

                    if (!settingsMenuVerticalNavigationScript.buttonList.Contains(goBackButton))
                    {
                        settingsMenuVerticalNavigationScript.buttonList.Remove(goBackButton);
                    }

                    goBackButton.SetActive(false);
                    creditRoller.SetActive(false);
                    settingsMenu.SetActive(false);
                    titleMenuVerticalNavigationScript.enabled = true;

                    settingsMenuVerticalNavigationScript.enabled = false;
                    currentMenu = "TitleMainMenu";
                    StartCoroutine(ResetTransition());
                    return;
                }


            case "CreditsMenu":
                {
                    titleMenu.SetActive(true);

                    var buttonScripts = titleMenu.GetComponentsInChildren<ButtonIndexController>(true);
                    foreach (var script in buttonScripts)
                    {
                        script.SetKeyboardController(titleMenuVerticalNavigationScript);
                    }

                    if (!oneButtonVerticalNavigationScript.buttonList.Contains(goBackButton))
                    {
                        oneButtonVerticalNavigationScript.buttonList.Remove(goBackButton);
                    }
                    goBackButton.SetActive(false);
                    creditRoller.SetActive(false);
                    settingsMenu.SetActive(false);
                    titleMenuVerticalNavigationScript.enabled = true;

                    currentMenu = "TitleMainMenu";
                    StartCoroutine(ResetTransition());
                    return;
                }

            case "SettingsControlSubMenu":
                {
                    GoToSettings();
                    controlLayout.SetActive(false);
                    currentMenu = "SettingsMenu";
                    StartCoroutine(ResetTransition());
                    return;
                }

            case "SettingsSoundSubMenu":
                {
                    GoToSettings();
                    soundMenu.SetActive(false);
                    currentMenu = "SettingsMenu";
                    StartCoroutine(ResetTransition());
                    return;
                }


        }




    }

    private IEnumerator ResetTransition()
    {
        Debug.Log("ResetTransitionFlag started.");
        yield return null;
        isGoBackTransitioning = false;
        Debug.Log("ResetTransitionFlag ended. isTransitioning reset.");
    }

    public void ChangeSceneToNewGame()
    {
        titleAndButtonScript = gameObject.GetComponent<TitleSceneAndButtonFunction>();
        dataPersistanceTestNumber++;
        fadeController = GetComponent<FadeToBlack>();

        // Desactiva todos los VerticalOnlyNavigation menos el de titleMenu
        titleMenuVerticalNavigationScript.enabled = true;
        settingsMenuVerticalNavigationScript.enabled = false;
        soundPanelVerticalNavigationScript.enabled = false;
        oneButtonVerticalNavigationScript.enabled = false;

        if (fadeController != null)
        {
            fadeController.FadeToSceneByIndex(1);
            titleAndButtonScript.enabled = false;
        }
        else
        {
            Debug.LogWarning("FadeToBlack missing on reload.");
        }
    }

    public void Update()
    {
        //Debug.Log(currentMenu);
    }

    public void QuitApplication()
    {

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif


    }
}
