using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    private IInteractable currentInteractable;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Swing.performed += OnSwingPerformed;
    }

    private void OnDisable()
    {
        inputActions.Player.Swing.performed -= OnSwingPerformed;
        inputActions.Disable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            currentInteractable = interactable;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable) && interactable == currentInteractable)
        {
            currentInteractable = null;
        }
    }

    private void OnSwingPerformed(InputAction.CallbackContext context)
    {
        // Si hay diálogo activo, se avanza el diálogo y NO se interactúa
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive)
        {
            DialogueManager.Instance.AdvanceDialogue();
            return;
        }

        // Si no hay diálogo, se interactúa normalmente
        currentInteractable?.Interact();
    }
}
