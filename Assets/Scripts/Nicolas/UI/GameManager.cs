using System;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para SceneManager
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton Pattern (recomendado para GameManagers)

    [Header("Referencias de UI")]
    [Tooltip("El Canvas que se activará en la escena del menú (índice 0).")]
    public GameObject canvasMenu;
    [Tooltip("El Canvas que se activará en las escenas de juego (índice > 0).")]
    public GameObject canvasInGame;

    private void Awake()
    {
        // Implementación del patrón Singleton para asegurar que solo haya un GameManager
        if (Instance == null)
        {
            Instance = this;
            // Opcional: Si quieres que el GameManager persista entre escenas
            // DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Debug.LogWarning("Se intentó crear una segunda instancia de GameManager. Destruyendo este GameObject.", this);
            Destroy(gameObject);
            return; // Salir para evitar errores en la instancia duplicada
        }

        // Verificar que los canvases estén asignados en el Inspector
        if (canvasMenu == null)
        {
            Debug.LogError("CanvasMenu no asignado en el Inspector de GameManager.", this);
        }
        if (canvasInGame == null)
        {
            Debug.LogError("CanvasInGame no asignado en el Inspector de GameManager.", this);
        }
    }

    void Start()
    {
        // Llamar a la lógica de actualización al inicio para la escena actual
        // Esto es necesario porque 'SceneManager.sceneLoaded' no se dispara para la primera escena
        // si el GameManager ya está activo en ella.
        UpdateCanvasVisibility(SceneManager.GetActiveScene());
    }

    private void OnEnable()
    {
        // Suscribirse al evento cuando el objeto está activo
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Desuscribirse del evento cuando el objeto se desactiva o destruye
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Se llama automáticamente cada vez que una nueva escena se carga.
    /// </summary>
    /// <param name="scene">La escena que acaba de cargarse.</param>
    /// <param name="mode">El modo de carga (Additive o Single).</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Escena cargada: {scene.name} (Índice: {scene.buildIndex})");
        UpdateCanvasVisibility(scene);
    }

    /// <summary>
    /// Actualiza la visibilidad de los canvases y el estado del cursor según el índice de la escena.
    /// </summary>
    /// <param name="scene">La escena actual.</param>
    private void UpdateCanvasVisibility(Scene scene)
    {
        if (canvasMenu == null || canvasInGame == null)
        {
            Debug.LogError("Uno o ambos canvases no están asignados en GameManager. No se puede actualizar la visibilidad.", this);
            return;
        }

        if (scene.buildIndex == 0) // Escena del menú
        {
            canvasMenu.SetActive(true);
            canvasInGame.SetActive(false);

            // Configuración del cursor para el menú
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Debug.Log("Configuración para escena de Menú: Canvas Menú ON, Canvas InGame OFF, Cursor visible y desbloqueado.");
        }
        else // Cualquier otra escena (escena de juego)
        {
            canvasMenu.SetActive(false);
            canvasInGame.SetActive(true);

            // Configuración del cursor para el juego
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Debug.Log("Configuración para escena de Juego: Canvas Menú OFF, Canvas InGame ON, Cursor oculto y bloqueado.");
        }
    }

    /// <summary>
    /// Método para salir del juego (funciona en Editor y en Build).
    /// </summary>
    public void ExitGame()
    {
#if UNITY_EDITOR
        // Si está en el editor, salir del modo Play
        EditorApplication.isPlaying = false;
        Debug.Log("Saliendo del modo Play en Unity Editor.");
#else
        // Si está en una build, cerrar la aplicación
        Application.Quit();
        Debug.Log("Saliendo de la aplicación.");
#endif
    }

    // Opcional: Métodos para activar/desactivar el cursor manualmente, por ejemplo, para un menú de pausa.
    public void ShowCursorAndUnlock()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Debug.Log("Cursor visible y desbloqueado.");
    }

    public void HideCursorAndLock()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Debug.Log("Cursor oculto y bloqueado.");
    }
}