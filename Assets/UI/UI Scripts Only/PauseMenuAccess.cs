using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class PauseMenuAccess : MonoBehaviour, InputSystem_Actions.IUIActions
{
    public GameObject escapeScreen;
    public GameObject escapeScreenBackground;
    public GameObject escapeKey;

    private bool isGameOnPauseMenu;
    private InputSystem_Actions controls;
    private InventoryAccess inventoryAccessScript;
    private MenuButtonListManager menuButtonListManager;
    private PauseMenuNavigation pauseMenuNavigationScript;
    private EscMenuBehaviour escapeKeyScript;

    void Awake()
    {
        controls = new InputSystem_Actions();
        controls.UI.SetCallbacks(this);
        inventoryAccessScript = gameObject.GetComponent<InventoryAccess>();
        menuButtonListManager = gameObject.GetComponent<MenuButtonListManager>();
        pauseMenuNavigationScript = gameObject.GetComponent<PauseMenuNavigation>();
        escapeKeyScript = escapeKey.GetComponent<EscMenuBehaviour>();

    }

    void OnEnable()
    {
        controls.Enable();
        isGameOnPauseMenu = false;

    }

    void OnDisable()
    {
        controls.Disable();
    }

    void Start()
    {
        // Initialize button layout once at startup, then hide pause UI
        escapeScreen.SetActive(false);
        isGameOnPauseMenu = false;

        escapeScreen.SetActive(true);

        menuButtonListManager.ShowPauseMenu();
        pauseMenuNavigationScript.RestartSelection(0);



        escapeScreen.SetActive(false);
        Time.timeScale = 1f;


    }

    private void PauseGame()
    {
        isGameOnPauseMenu = true;
        inventoryAccessScript.enabled = false;
        menuButtonListManager.ShowPauseMenu();

        if (escapeScreen != null)
        {
            escapeScreenBackground.SetActive(true);
            escapeScreen.SetActive(true);
            Time.timeScale = 0f;

            StartCoroutine(UnblockSubmitNextFrame());
        }
        else
        {
            Debug.LogWarning("Pause UI layers are missing in the scene.");
        }
    }

    private void UnpauseGame()
    {
        isGameOnPauseMenu = false;
        inventoryAccessScript.enabled = true;
        escapeScreenBackground.SetActive(false);
        escapeScreen.SetActive(false);
        Time.timeScale = 1f;
    }

    private IEnumerator UnblockSubmitNextFrame()
    {
        yield return null; // wait 1 frame
        pauseMenuNavigationScript.UnblockSubmit();
    }

    public void PressingContinue()
    {
       
            UnpauseGame();
       
    }

    void Update()
    {

    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!isGameOnPauseMenu)
            {
                PauseGame();
            }
            else if (isGameOnPauseMenu && escapeKeyScript.onPauseMainMenu)
            {
                UnpauseGame();
            }
        }
    }
    public void OnClick(InputAction.CallbackContext context)
    {
    }

    public void OnMiddleClick(InputAction.CallbackContext context)
    {
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
    }

    public void OnScrollWheel(InputAction.CallbackContext context)
    {
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
    }

    public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
    {
    }

    public void OnTrackedDevicePosition(InputAction.CallbackContext context)
    {
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
       
    }
}
