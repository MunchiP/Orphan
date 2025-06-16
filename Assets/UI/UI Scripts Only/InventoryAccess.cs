using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryAccess : MonoBehaviour, InputSystem_Actions.IUIActions
{
    public GameObject tabScreen;
    
    public bool isGameOnInventory;
    private InputSystem_Actions controls;
    private InventoryPanelRotation menuRotationScript;
    private PauseMenuAccess pauseMenuAccessScript;


    void Awake()
    {
        controls = new InputSystem_Actions();
        controls.UI.SetCallbacks(this);
        pauseMenuAccessScript = gameObject.GetComponent<PauseMenuAccess>();
    }

    void OnEnable()
    {
        controls.Enable();
        isGameOnInventory = false;
    }

    void OnDisable()
    {
        controls.Disable();
    }    

    void Start()
    {
        menuRotationScript = GetComponent<InventoryPanelRotation>();
        menuRotationScript.enabled = false;
        tabScreen.SetActive(false);
        isGameOnInventory = false;
    }

    
    void Update()
    {
        
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        
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
        if (context.performed)
        {
            if (!isGameOnInventory)
            {
                isGameOnInventory = true;
                pauseMenuAccessScript.enabled = false;
                if (tabScreen != null)
                {
                    menuRotationScript.enabled = true;
                    tabScreen.SetActive(true);

                    Time.timeScale = 0f;
                }
                else if (tabScreen == null)
                {
                    Debug.Log("Either one or both Pause Main Layers are missing on the scene");
                }
            }
            else if (isGameOnInventory && !menuRotationScript.isMenuRotating)
            {
                isGameOnInventory = false;
                pauseMenuAccessScript.enabled = true;
                if (tabScreen != null)
                {
                    menuRotationScript.enabled = false;
                    tabScreen.SetActive(false);

                    Time.timeScale = 1f;
                }
                else if (tabScreen == null)
                {
                    Debug.Log("Either one or both Pause Main Layers are missing on the scene");
                }
            }

        }
    }
}
