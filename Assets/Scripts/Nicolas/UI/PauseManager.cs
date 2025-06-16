using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuCanvas;
    public MenuStackManager menuStackManager;

    private InputAction pauseAction;

    private void OnEnable()
    {
        var playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            pauseAction = playerInput.actions["Pause"];
            pauseAction.performed += OnPausePerformed;
            pauseAction.Enable();
        }
    }

    private void OnDisable()
    {
        if (pauseAction != null)
        {
            pauseAction.performed -= OnPausePerformed;
            pauseAction.Disable();
        }
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        // No abrir pausa si estamos en la escena 0
        if (SceneManager.GetActiveScene().buildIndex == 0)
            return;

        // Si el menú de pausa no está activo, lo abrimos y pausamos el juego
        if (!pauseMenuCanvas.activeSelf)
        {
            menuStackManager.OpenMenu(pauseMenuCanvas);
            Time.timeScale = 0f;
        }
    }
}
