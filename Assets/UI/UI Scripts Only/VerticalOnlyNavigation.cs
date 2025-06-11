using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class VerticalOnlyNavigation : MonoBehaviour, InputSystem_Actions.IUIActions
{
    private InputSystem_Actions controlsUI;
    public List<GameObject> buttonList = new List<GameObject>();
    public int buttonToMoveOnto;
    private GameObject currentButton;
    public int lastButtonIndex = -1;
    private int mouseHoveringButtonIndex = -1;
    private bool isMouseHoveringButton;
    private bool isMouseControlling = false;

    void Awake()
    {
        controlsUI = new InputSystem_Actions();
        controlsUI.UI.SetCallbacks(this);
    }

    void OnEnable()
    {
        controlsUI.Enable();
        lastButtonIndex = -1;
        buttonToMoveOnto = -1;
        EventSystem.current.SetSelectedGameObject(null);
    }

    void OnDisable()
    {
        controlsUI.Disable();
        lastButtonIndex = -1;
        buttonToMoveOnto = -1;
    }

    public void SetMouseHoverState(bool isMouseOnButton)
    {
        isMouseHoveringButton = isMouseOnButton;
        isMouseControlling = isMouseOnButton;

        if (!isMouseOnButton)
        {
            mouseHoveringButtonIndex = -1;
        }
    }

    public void SetMouseHoverButtonIndex(int index)
    {
        mouseHoveringButtonIndex = index;
        isMouseHoveringButton = index >= 0;
    }

    void Update()
    {
        if (buttonList == null || buttonList.Count == 0)
            return;

        if (!isMouseHoveringButton && buttonToMoveOnto != lastButtonIndex && IsIndexValid(buttonToMoveOnto))
        {
            SelectButton(buttonToMoveOnto);
        }
        else if (isMouseHoveringButton && mouseHoveringButtonIndex != lastButtonIndex && IsIndexValid(mouseHoveringButtonIndex))
        {
            SelectButton(mouseHoveringButtonIndex);
        }
    }

    private bool IsIndexValid(int index)
    {
        return index >= 0 && index < buttonList.Count;
    }

    private void SelectButton(int index)
    {
        if (!IsIndexValid(index))
            return;

        GameObject button = buttonList[index];

        // 🔥 Desactivar el botón anterior visualmente
        if (lastButtonIndex != -1 && IsIndexValid(lastButtonIndex))
        {
            GameObject lastButton = buttonList[lastButtonIndex];
            var btnComponent = lastButton.GetComponent<Selectable>();
            if (btnComponent != null)
                btnComponent.OnDeselect(null);
        }

        // 🔥 Limpiar selección previa en el sistema de eventos
        EventSystem.current.SetSelectedGameObject(null);

        // 🔥 Forzar seleccionar nuevo botón
        EventSystem.current.SetSelectedGameObject(button);
        button.GetComponent<Button>()?.Select();

        currentButton = button;
        lastButtonIndex = index;
        buttonToMoveOnto = index;
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        if (buttonList == null || buttonList.Count == 0 || isMouseControlling)
            return;

        if (context.performed)
        {
            Vector2 direction = context.ReadValue<Vector2>();

            if (direction.y > 0.5f)
            {
                buttonToMoveOnto = buttonToMoveOnto < 0 ? 0 : (buttonToMoveOnto - 1 + buttonList.Count) % buttonList.Count;
            }
            else if (direction.y < -0.5f)
            {
                buttonToMoveOnto = buttonToMoveOnto < 0 ? 0 : (buttonToMoveOnto + 1) % buttonList.Count;
            }
        }
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameObject selected = EventSystem.current.currentSelectedGameObject;
            if (selected != null)
            {
                Button button = selected.GetComponent<Button>();
                if (button != null && button.interactable)
                {
                    button.onClick.Invoke();
                }
            }
        }
    }

    // Métodos vacíos necesarios por la interfaz
    public void OnCancel(InputAction.CallbackContext context) { }
    public void OnClick(InputAction.CallbackContext context) { }
    public void OnMiddleClick(InputAction.CallbackContext context) { }
    public void OnPoint(InputAction.CallbackContext context) { }
    public void OnRightClick(InputAction.CallbackContext context) { }
    public void OnScrollWheel(InputAction.CallbackContext context) { }
    public void OnTrackedDeviceOrientation(InputAction.CallbackContext context) { }
    public void OnTrackedDevicePosition(InputAction.CallbackContext context) { }
    public void OnInventory(InputAction.CallbackContext context) { }
}
