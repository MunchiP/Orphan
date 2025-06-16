using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class EscMenuBehaviour : MonoBehaviour
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
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnEnable()
    {
        controlsUI.Enable();
        controlsUI.Player.Pause.performed += OnPausePerformed;
    }

    void OnDisable()
    {
        controlsUI.Player.Pause.performed -= OnPausePerformed;
        controlsUI.Disable();
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        if (!context.performed || isCreditsActive || isGameOverActive)
        {
            Debug.Log("[EscMenuBehaviour] Input bloqueado (créditos/gameover activos o no performed)");
            return;
        }

        if (pauseScript == null || universalButtonListManager == null)
        {
            Debug.LogWarning("[EscMenuBehaviour] pauseScript o universalButtonListManager son null");
            return;
        }

        if (!pauseScript.enabled)
        {
            universalButtonListManager.GoBack();
        }
        else if (!pauseScript.isGameOnPauseMenu)
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

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[EscMenuBehaviour] OnSceneLoaded called for: {scene.name}");
        int index = scene.buildIndex;

        if (GameManager == null)
        {
            GameManager = GameObject.FindWithTag("GameManager");
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

        if (index == 0)
        {
            onTitleMainMenu = true;
            onPauseMainMenu = false;

            if (pauseScript != null)
                pauseScript.enabled = false;

            Time.timeScale = 1f;
        }
        else
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
}
