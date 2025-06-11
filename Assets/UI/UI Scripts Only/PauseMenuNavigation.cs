using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenuNavigation : MonoBehaviour, InputSystem_Actions.IUIActions
{
    private InputSystem_Actions controlsUI;
    public List<GameObject> buttonList = new List<GameObject>();
    public int buttonToMoveOnto;
    private GameObject currentButton;
    public int lastButtonIndex = -1;
    private int mouseHoveringButtonIndex = -1;
    private bool isMouseHoveringButton;
    private bool isMouseControlling = false;

    // Nueva lógica para bloquear temporalmente el mouse
    private bool blockMouseTemporarily = false;
    private float mouseBlockTimer = 0f;
    private float mouseBlockDuration = 2f;

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

    void Start()
    {
        lastButtonIndex = -1;
        buttonToMoveOnto = -1;
    }

    public void RestartSelection(int startIndex = 0)
    {
        if (buttonList.Count == 0) return;

        buttonToMoveOnto = Mathf.Clamp(startIndex, 0, buttonList.Count - 1);
        currentButton = buttonList[buttonToMoveOnto];

        if (currentButton != null)
        {
            currentButton.GetComponent<Button>().Select();
        }

        lastButtonIndex = buttonToMoveOnto;
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
        // Control de tiempo para bloquear mouse
        if (blockMouseTemporarily)
        {
            mouseBlockTimer -= Time.unscaledDeltaTime;
            if (mouseBlockTimer <= 0f)
            {
                blockMouseTemporarily = false;
                isMouseControlling = false;
            }
        }

        if (buttonList == null || buttonToMoveOnto < 0 || buttonToMoveOnto >= buttonList.Count)
            return;

        // Si el mouse está controlando, no hagas nada
        if (isMouseHoveringButton && !blockMouseTemporarily)
            return;

        if (!isMouseHoveringButton && buttonToMoveOnto != lastButtonIndex)
        {
            currentButton = buttonList[buttonToMoveOnto];
            currentButton.GetComponent<Button>().Select();
            lastButtonIndex = buttonToMoveOnto;
        }
        else if (isMouseHoveringButton && mouseHoveringButtonIndex != lastButtonIndex && mouseHoveringButtonIndex >= 0 && !blockMouseTemporarily)
        {
            currentButton = buttonList[mouseHoveringButtonIndex];
            currentButton.GetComponent<Button>().Select();
            buttonToMoveOnto = mouseHoveringButtonIndex;
            lastButtonIndex = mouseHoveringButtonIndex;
        }
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        if (buttonList == null || buttonList.Count == 0)
            return;

        if (buttonList.Count == 1)
        {
            buttonToMoveOnto = 0;
            lastButtonIndex = 0;
            buttonList[0].GetComponent<Button>().Select();
            return;
        }

        if (context.performed)
        {
            Vector2 direction = context.ReadValue<Vector2>();

            if (Mathf.Abs(direction.y) > 0.5f)
            {
                blockMouseTemporarily = true;
                mouseBlockTimer = mouseBlockDuration;

                if (direction.y > 0)
                {
                    buttonToMoveOnto = (buttonToMoveOnto - 1 + buttonList.Count) % buttonList.Count;
                }
                else
                {
                    buttonToMoveOnto = (buttonToMoveOnto + 1) % buttonList.Count;
                }
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
                else
                {
                    Debug.LogWarning("Selected GameObject is not a Button or is not interactable.");
                }
            }
            else
            {
                Debug.LogWarning("No UI element is currently selected.");
            }
        }
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        if ((context.performed || context.started) && !blockMouseTemporarily)
        {
            isMouseControlling = true;
        }
    }

    // Métodos requeridos por la interfaz pero no usados actualmente
    public void OnCancel(InputAction.CallbackContext context) { }
    public void OnClick(InputAction.CallbackContext context) { }
    public void OnMiddleClick(InputAction.CallbackContext context) { }
    public void OnRightClick(InputAction.CallbackContext context) { }
    public void OnScrollWheel(InputAction.CallbackContext context) { }
    public void OnTrackedDeviceOrientation(InputAction.CallbackContext context) { }
    public void OnTrackedDevicePosition(InputAction.CallbackContext context) { }
    public void OnInventory(InputAction.CallbackContext context) { }
}
