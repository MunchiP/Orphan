using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HorizontalOnlyNavigation : MonoBehaviour, InputSystem_Actions.IUIActions
{

    private InputSystem_Actions controlsUI;
    public List<GameObject> buttonList = new List<GameObject>();
    private int buttonToMoveOnto;
    private GameObject currentButton;
    private ButtonIndexController buttonIndexController;
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
    void Start()
    {

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
            lastButtonIndex = -1;
        }
    }

    public void SetMouseHoverButtonIndex(int index)
    {
        mouseHoveringButtonIndex = index;
        isMouseHoveringButton = index >= 0;
    }

    void Update()
    {
        if (buttonToMoveOnto < 0 || buttonToMoveOnto >= buttonList.Count)
            return;

        if (isMouseHoveringButton)
            return;

        if (!isMouseHoveringButton && buttonToMoveOnto != lastButtonIndex)
        {

            if (lastButtonIndex >= 0)
            {
                var previousButton = buttonList[lastButtonIndex];
                var previousPawCall = previousButton.GetComponent<ButtonSidePawController>();
                if (previousPawCall != null) previousPawCall.OnPointerExit();
            }


            currentButton = buttonList[buttonToMoveOnto];
            currentButton.GetComponent<UnityEngine.UI.Button>().Select();
            var currentPawCall = currentButton.GetComponent<ButtonSidePawController>();
            if (currentPawCall != null) currentPawCall.OnPointerEnter();


            lastButtonIndex = buttonToMoveOnto;
        }

        else if (isMouseHoveringButton)
        {

            if (mouseHoveringButtonIndex != lastButtonIndex && mouseHoveringButtonIndex >= 0)
            {
                if (lastButtonIndex >= 0)
                {
                    var previousButton = buttonList[lastButtonIndex];
                    previousButton.GetComponent<ButtonSidePawController>()?.OnPointerExit();
                }

                currentButton = buttonList[mouseHoveringButtonIndex];
                currentButton.GetComponent<UnityEngine.UI.Button>().Select();
                currentButton.GetComponent<ButtonSidePawController>()?.OnPointerEnter();

                buttonToMoveOnto = mouseHoveringButtonIndex;
                lastButtonIndex = mouseHoveringButtonIndex;
            }
        }

        //Debug.Log(buttonToMoveOnto);
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
    }

    public void OnClick(InputAction.CallbackContext context)
    {
    }

    public void OnMiddleClick(InputAction.CallbackContext context)
    {
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        if (isMouseControlling) return;
        if (!isMouseHoveringButton && context.performed)
        {
            Vector2 direction = context.ReadValue<Vector2>();


            if (direction.x > 0.5f)
            {
                if (buttonToMoveOnto < 0)
                {
                    buttonToMoveOnto = 0;
                    return;
                }
                buttonToMoveOnto = (buttonToMoveOnto - 1 + buttonList.Count) % buttonList.Count;


            }
            else if (direction.x < -0.5f)
            {
                if (buttonToMoveOnto < 0)
                {
                    buttonToMoveOnto = 0;
                    return;
                }
                buttonToMoveOnto = (buttonToMoveOnto + 1) % buttonList.Count;
            }
        }
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

    public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
    {
    }

    public void OnTrackedDevicePosition(InputAction.CallbackContext context)
    {
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
    }
}
