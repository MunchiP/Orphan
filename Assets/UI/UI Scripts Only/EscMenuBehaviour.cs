using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class EscMenuBehaviour : MonoBehaviour, InputSystem_Actions.IUIActions
{
    public GameObject GameManager;
    public GameObject inGamePauseCanvas;
    private InputSystem_Actions controlsUI;
    private TitleSceneAndButtonFunction titlenavigationscript;
    private MenuButtonListManager menuButtonListManager;
    public bool onTitleMainMenu = false;
    public bool onPauseMainMenu = false;
    private TextMeshProUGUI escapeTMP;

    void Awake()
    {
        controlsUI = new InputSystem_Actions();
        controlsUI.UI.SetCallbacks(this);
    }

    public void OnEnable()
    {
        controlsUI.Enable();
        SceneManager.sceneLoaded += OnSceneLoaded;

    }
    public void OnDisable()
    {
        controlsUI.Disable();
        SceneManager.sceneLoaded -= OnSceneLoaded;

    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            onTitleMainMenu = false;
            onPauseMainMenu = true;
        }
        else if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            Debug.Log("im on title scene");
            onTitleMainMenu = true;
            onPauseMainMenu = false;
        }
    }

    void Start()
    {
        titlenavigationscript = GameManager.GetComponent<TitleSceneAndButtonFunction>();
        menuButtonListManager = inGamePauseCanvas.GetComponent<MenuButtonListManager>();
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
        if(context.performed)
        {
            if(!onPauseMainMenu)
            {
                menuButtonListManager.GoBack();
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
