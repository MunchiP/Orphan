using System;
using NUnit.Framework;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenuNavigation : MonoBehaviour, InputSystem_Actions.IUIActions
{

    private InputSystem_Actions controlsUI;
    public List<GameObject> buttonList = new List<GameObject>();
    public int buttonToMoveOnto;
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

    public void RestartSelection(int startIndex = 0)
    {
        if (buttonList.Count == 0) return;
        buttonToMoveOnto = Mathf.Clamp(startIndex, 0, buttonList.Count - 1);
        buttonList[buttonToMoveOnto].GetComponent<Button>().Select();
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
        if (buttonList == null)
            return;
        if (buttonToMoveOnto < 0 || buttonToMoveOnto > buttonList.Count)
            return;

        if (isMouseHoveringButton)
            return;

        if (!isMouseHoveringButton && buttonToMoveOnto != lastButtonIndex)
        {

            if (lastButtonIndex >= 0)
            {
                var previousButton = buttonList[lastButtonIndex];

            }


            currentButton = buttonList[buttonToMoveOnto];
            currentButton.GetComponent<UnityEngine.UI.Button>().Select();

            Debug.Log("lastindex should be same as buttontomoveonto");

            lastButtonIndex = buttonToMoveOnto;
        }

        else if (isMouseHoveringButton)
        {

            if (mouseHoveringButtonIndex != lastButtonIndex && mouseHoveringButtonIndex >= 0)
            {
                if (lastButtonIndex >= 0)
                {
                    var previousButton = buttonList[lastButtonIndex];

                }

                currentButton = buttonList[mouseHoveringButtonIndex];
                currentButton.GetComponent<UnityEngine.UI.Button>().Select();


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
        if (buttonList == null || buttonList.Count == 0)
            return;
        if (isMouseControlling) return;
        if (!isMouseHoveringButton && context.performed)
        {
            Vector2 direction = context.ReadValue<Vector2>();


            if (direction.y > 0.5f)
            {
                if (buttonToMoveOnto < 0)
                {
                    buttonToMoveOnto = 0;
                    return;
                }
                buttonToMoveOnto = (buttonToMoveOnto - 1 + buttonList.Count) % buttonList.Count;


            }
            else if (direction.y < -0.5f)
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

