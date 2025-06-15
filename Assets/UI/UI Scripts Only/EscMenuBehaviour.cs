using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class EscMenuBehaviour : MonoBehaviour, InputSystem_Actions.IUIActions
{
    public GameObject GameManager;
    
    private InputSystem_Actions controlsUI;
    private PauseMenuNavigation universalNavigationScript;
    private ButtonListManager universalButtonListManager;
    public PauseMenuAccess pauseScript;
    public bool onTitleMainMenu = false;
    public bool onPauseMainMenu = false;
    private TextMeshProUGUI escapeTMP;

    void Awake()
    {
        Debug.Log("[EscMenuBehaviour] Awake - Subscribing to sceneLoaded");
        controlsUI = new InputSystem_Actions();
        controlsUI.UI.SetCallbacks(this);
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnEnable()
    {
        controlsUI.Enable();
        

    }
    public void OnDisable()
    {
        controlsUI.Disable();
        

    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[EscMenuBehaviour] OnSceneLoaded called for: {scene.name}");
        int index = scene.buildIndex;

        // Title Scene
        if (index == 0)
        {
            Debug.Log("making ontitlemainmenu true and onpausemainmenu false");
            onTitleMainMenu = true;
            onPauseMainMenu = false;

            // Ensure pause-related systems are disabled in title
            //GameManager.GetComponent<PauseMenuAccess>().enabled = false;
            Time.timeScale = 1f; // Just in case we came from paused state
        }
        // In-Game Scene
        else if (index == 1)
        {
            Debug.Log("making ontitlemainmenu false and onpausemainmenu true");
            onTitleMainMenu = false;
            onPauseMainMenu = true;

            // Enable pause logic for gameplay
            GameManager.GetComponent<PauseMenuAccess>().enabled = true;
        }
    }

    void Start()
    {
        universalNavigationScript = GameManager.GetComponent<PauseMenuNavigation>();
        universalButtonListManager = GameManager.GetComponent<ButtonListManager>();
        escapeTMP = gameObject.GetComponent<TextMeshProUGUI>();
    }




    void Update()
    {

        
        if (onPauseMainMenu || onTitleMainMenu)
        {
            SetAlpha(0f);
        }
        if (!onPauseMainMenu && !onTitleMainMenu)
        {
            SetAlpha(1f);
        }
    }

    void SetAlpha(float alpha)
    {
        Color color = escapeTMP.color;
        color.a = alpha;
        escapeTMP.color = color;
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if(context.performed && !pauseScript.enabled)
        {
            // We're on the title — only use GoBack()
            universalButtonListManager.GoBack();
        }
        else if(context.performed && pauseScript.enabled)
        {
            if (!pauseScript.isGameOnPauseMenu)
            {
                pauseScript.PauseGame();
            }
            else if(pauseScript.isGameOnPauseMenu && onPauseMainMenu)
            {
                pauseScript.UnpauseGame();
            }
            else if (pauseScript.isGameOnPauseMenu && !onPauseMainMenu)
            {
                universalButtonListManager.GoBack();
                
            }
        }
        
    }

    public void OnClick(InputAction.CallbackContext context)
    {
    }

    public void OnInventory(InputAction.CallbackContext context)
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
}
