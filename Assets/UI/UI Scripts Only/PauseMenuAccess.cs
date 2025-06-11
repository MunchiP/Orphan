using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuAccess : MonoBehaviour, InputSystem_Actions.IUIActions
{
    public GameObject escapeScreen;
    public GameObject settingScreen;
    public GameObject controlLayout;
    public GameObject soundUi;

    private bool isGameOnPauseMenu;
    private InputSystem_Actions controls;
    private InventoryAccess inventoryAccessScript;
    private MenuButtonListManager menuButtonListManager;
    private PauseMenuNavigation pauseMenuNavigationScript;

    void Awake()
    {
        controls = new InputSystem_Actions();
        controls.UI.SetCallbacks(this);
        inventoryAccessScript = gameObject.GetComponent<InventoryAccess>();
        menuButtonListManager = gameObject.GetComponent<MenuButtonListManager>();
        pauseMenuNavigationScript = gameObject.GetComponent<PauseMenuNavigation>();
        
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
        escapeScreen.SetActive(false);
        isGameOnPauseMenu = false;

        escapeScreen.SetActive(true);

        menuButtonListManager.ShowPauseMenu();
        pauseMenuNavigationScript.RestartSelection(0);

        escapeScreen.SetActive(false);
        Time.timeScale = 1f;
        

       

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
                isGameOnPauseMenu = true;
                inventoryAccessScript.enabled = false;
                menuButtonListManager.ShowPauseMenu();
                if (escapeScreen != null)
                {
                    inventoryAccessScript.enabled = false;
                    escapeScreen.SetActive(true);

                    Time.timeScale = 0f;
                }
                else if (escapeScreen == null)
                {
                    Debug.Log("Either one or both Pause Main Layers are missing on the scene");
                }
            }
            else if (isGameOnPauseMenu)
            {
                isGameOnPauseMenu = false;
                if (escapeScreen != null)
                {
                    inventoryAccessScript.enabled = true;
                    pauseMenuNavigationScript.RestartSelection();
                    escapeScreen.SetActive(false);
                    settingScreen.SetActive(false);
                    controlLayout.SetActive(false);
                    soundUi.SetActive(false);
                    Time.timeScale = 1f;
                }
                else if (escapeScreen == null)
                {
                    Debug.Log("Either one or both Pause Main Layers are missing on the scene");
                }
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
