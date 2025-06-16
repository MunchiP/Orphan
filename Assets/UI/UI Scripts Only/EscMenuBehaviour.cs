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
    public bool isCreditsActive = false;
    public bool isGameOverActive = false;

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

    public void OnPause(InputAction.CallbackContext context)
    {
        // Puedes dejarlo vacío si no necesitas que haga nada
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[EscMenuBehaviour] OnSceneLoaded called for: {scene.name}");
        int index = scene.buildIndex;

        // Intentar asignar GameManager si está null
        if (GameManager == null)
        {
            GameManager = GameObject.FindWithTag("GameManager"); // O busca por nombre con Find("GameManager") o como prefieras
            if (GameManager == null)
            {
                Debug.LogWarning("[EscMenuBehaviour] No GameManager found on scene load");
                return;
            }
            else
            {
                Debug.Log("[EscMenuBehaviour] GameManager found dynamically on scene load");
            }
        }

        if (index == 0) // Escena título
        {
            onTitleMainMenu = true;
            onPauseMainMenu = false;

            // Asegurar que la pausa esté desactivada en título
            if (pauseScript != null)
                pauseScript.enabled = false;

            Time.timeScale = 1f; // Por si venimos de pausa
        }
        else // Escena de juego
        {
            onTitleMainMenu = false;
            onPauseMainMenu = true;

            if (GameManager != null)
            {
                var pauseAccess = GameManager.GetComponent<PauseMenuAccess>();
                if (pauseAccess != null)
                {
                    pauseAccess.enabled = true;
                }
                else
                {
                    Debug.LogWarning("[EscMenuBehaviour] PauseMenuAccess component not found on GameManager");
                }
            }
            else
            {
                Debug.LogWarning("[EscMenuBehaviour] GameManager is null on scene load");
            }
        }
    }

    void Start()
    {
        if (GameManager == null)
        {
            GameManager = GameObject.FindWithTag("GameManager");
            if (GameManager == null)
            {
                Debug.LogWarning("[EscMenuBehaviour] GameManager is null in Start()");
                return;
            }
        }

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
        else
        {
            SetAlpha(1f);
        }
    }

    void SetAlpha(float alpha)
    {
        if (escapeTMP == null) return;

        Color color = escapeTMP.color;
        color.a = alpha;
        escapeTMP.color = color;
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (!context.performed || isCreditsActive || isGameOverActive)
        {
            Debug.Log("Blocking Esc key due to context or active credits/gameover.");
            return;
        }

        if (pauseScript == null || universalButtonListManager == null)
        {
            Debug.LogWarning("[EscMenuBehaviour] pauseScript or universalButtonListManager is null on OnCancel");
            return;
        }

        if (context.performed && !pauseScript.enabled)
        {
            // Solo en título
            universalButtonListManager.GoBack();
        }
        else if (context.performed && pauseScript.enabled)
        {
            if (!pauseScript.isGameOnPauseMenu)
            {
                pauseScript.PauseGame();
            }
            else if (pauseScript.isGameOnPauseMenu && onPauseMainMenu)
            {
                pauseScript.UnpauseGame();
            }
            else if (pauseScript.isGameOnPauseMenu && !onPauseMainMenu)
            {
                universalButtonListManager.GoBack();
            }
        }
    }

    public void OnClick(InputAction.CallbackContext context) { }
    public void OnInventory(InputAction.CallbackContext context) { }
    public void OnMiddleClick(InputAction.CallbackContext context) { }
    public void OnNavigate(InputAction.CallbackContext context) { }
    public void OnPoint(InputAction.CallbackContext context) { }
    public void OnRightClick(InputAction.CallbackContext context) { }
    public void OnScrollWheel(InputAction.CallbackContext context) { }
    public void OnSubmit(InputAction.CallbackContext context) { }
    public void OnTrackedDeviceOrientation(InputAction.CallbackContext context) { }
    public void OnTrackedDevicePosition(InputAction.CallbackContext context) { }
}
