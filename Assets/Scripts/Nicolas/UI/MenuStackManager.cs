using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuStackManager : MonoBehaviour
{
    public List<GameObject> allMenus;
    public GameObject defaultMenuInScene0;
    public GameObject pauseMenuCanvas;

    private Stack<GameObject> menuStack = new Stack<GameObject>();

    public PlayerInput playerInput;
    private InputAction pauseAction;

    private void Awake()
    {
        // Ya no abres aquí el menú default porque lo harás en OnSceneLoaded
        Time.timeScale = 1f;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (playerInput != null)
        {
            pauseAction = playerInput.actions["Pause"];
            pauseAction.performed += OnPausePerformed;
            pauseAction.Enable();
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (pauseAction != null)
        {
            pauseAction.performed -= OnPausePerformed;
            pauseAction.Disable();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            Debug.Log("Escena 0 cargada, reseteando menú principal.");
            ResetMenusToDefault();
        }
    }

    private void ResetMenusToDefault()
    {
        CloseAllMenus();
        if (defaultMenuInScene0 != null)
        {
            OpenMenu(defaultMenuInScene0);
        }
        Time.timeScale = 1f;
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        if (menuStack.Count == 0 && SceneManager.GetActiveScene().buildIndex != 0)
        {
            Debug.Log("No hay menús abiertos, abriendo menú pausa.");
            OpenMenu(pauseMenuCanvas);
        }
        else
        {
            Debug.Log("Cerrando menú superior.");
            CloseTopMenu();
        }
    }

    public void OpenMenu(GameObject menu)
    {
        Debug.Log("OpenMenu llamado para: " + menu.name);

        if (menuStack.Count > 0)
        {
            Debug.Log("Ocultando menú: " + menuStack.Peek().name);
            menuStack.Peek().SetActive(false);
        }

        if (!menuStack.Contains(menu))
        {
            menu.SetActive(true);
            menuStack.Push(menu);
            Debug.Log("Menú abierto: " + menu.name);
        }
        else
        {
            Debug.LogWarning("El menú ya está en el stack: " + menu.name);
        }
    }

    public void CloseTopMenu()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0 && menuStack.Count <= 1)
        {
            Debug.Log("En escena 0 y solo hay un menú. No se cierra.");
            return;
        }

        if (menuStack.Count > 0)
        {
            GameObject top = menuStack.Pop();
            top.SetActive(false);
            Debug.Log("Cerrando menú: " + top.name);
        }

        if (menuStack.Count > 0)
        {
            GameObject next = menuStack.Peek();
            next.SetActive(true);
            Debug.Log("Mostrando menú anterior: " + next.name);
        }
        else
        {
            Debug.Log("No quedan menús abiertos. Reanudando juego.");
            Time.timeScale = 1f;
        }
    }

    public void CloseAllMenus()
    {
        while (menuStack.Count > 0)
        {
            GameObject menu = menuStack.Pop();
            menu.SetActive(false);
        }

        Debug.Log("Todos los menús cerrados.");
        Time.timeScale = 1f;
    }
}
