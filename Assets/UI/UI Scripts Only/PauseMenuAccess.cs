using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class PauseMenuAccess : MonoBehaviour
{
    public GameObject escapeScreen;
    public GameObject escapeScreenBackground;
    public GameObject escapeKey;

    public bool isGameOnPauseMenu;
    
    private InventoryAccess inventoryAccessScript;
    private ButtonListManager universalButtonListManager;
    private PauseMenuNavigation pauseMenuNavigationScript;
    private EscMenuBehaviour escapeKeyScript;


    void Awake()
    {
        

        inventoryAccessScript = GetComponent<InventoryAccess>();
        universalButtonListManager = GetComponent<ButtonListManager>();
        pauseMenuNavigationScript = GetComponent<PauseMenuNavigation>();
    }

    void OnEnable()
    {
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.buildIndex);

        if (scene.buildIndex == 1) // In-Game scene
        {
            Debug.Log("Enabling PauseMenuAccess for In-Game scene");
            this.enabled = true;

            isGameOnPauseMenu = false;

            // Ensure no paused state lingers
            ResetPauseMenuVisuals();

            universalButtonListManager.isFirstTimeOpeningPause = true;
            InitializePauseMenu();
        }
        else
        {
            Debug.Log("Disabling PauseMenuAccess for Title scene");

            // Reset pause UI visuals in case scene transition occurred mid-pause
            ResetPauseMenuVisuals();

            this.enabled = false;
        }
    }

    public void TogglePause()
    {
        if (!isGameOnPauseMenu)
            PauseGame();
        else
            UnpauseGame();
    }
    private void ResetPauseMenuVisuals()
    {
        if (escapeScreen != null)
            escapeScreen.SetActive(false);

        if (escapeScreenBackground != null)
            escapeScreenBackground.SetActive(false);

        isGameOnPauseMenu = false;
        Time.timeScale = 1f;

        Debug.Log("[PauseMenuAccess] Reset pause menu visuals.");
    }
    private void InitializePauseMenu()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            this.enabled = false; // Disable self on Title
            return;
        }

        // Initialize required references (if not already assigned)
        inventoryAccessScript = GetComponent<InventoryAccess>();
        universalButtonListManager = GetComponent<ButtonListManager>();
        pauseMenuNavigationScript = GetComponent<PauseMenuNavigation>();
        escapeKeyScript = escapeKey.GetComponent<EscMenuBehaviour>();

        // Reset pause menu state
        escapeScreen.SetActive(false);
        escapeScreenBackground.SetActive(false);
        isGameOnPauseMenu = false;

        // Prepare layout (temporary activation for layout to be ready)
        escapeScreen.SetActive(true);
        universalButtonListManager.ShowPauseMenu();
        pauseMenuNavigationScript.RestartSelection(0);
        escapeScreen.SetActive(false);

        // Ensure game is running
        Time.timeScale = 1f;
    }

    public bool IsGamePaused()
    {
        return isGameOnPauseMenu;
    }

    public void PauseGame()
    {
        if (isGameOnPauseMenu) return;

        isGameOnPauseMenu = true;
        Time.timeScale = 0f;

        inventoryAccessScript.enabled = false;
        escapeScreen.SetActive(true);
        escapeScreenBackground.SetActive(true);

        universalButtonListManager.ShowPauseMenu();
        pauseMenuNavigationScript.RestartSelection(0);
        StartCoroutine(UnblockSubmitNextFrame());
    }

    public void UnpauseGame()
    {
        if (!isGameOnPauseMenu) return;

        isGameOnPauseMenu = false;
        Time.timeScale = 1f;

        inventoryAccessScript.enabled = true;
        escapeScreen.SetActive(false);
        escapeScreenBackground.SetActive(false);
    }

    private IEnumerator UnblockSubmitNextFrame()
    {
        yield return null;
        pauseMenuNavigationScript.UnblockSubmit();
    }

    public void PressingContinue()
    {
        UnpauseGame();
    }

      
}
