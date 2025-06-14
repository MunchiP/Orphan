using System;
using NUnit.Framework;
//using UnityEditor.ShaderGraph;
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
    private PauseMenuNavigation buttonIndexController;
    public int lastButtonIndex = -1;
    private bool submitBlocked = false;
    private float submitBlockTime = 0.1f; // 100ms, tweak as needed
    private float lastSubmitTime = -1f;

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

  


    

   

    void Update()
    {



        if (buttonList == null || buttonList.Count == 0)
            return;

        if (buttonToMoveOnto < 0 || buttonToMoveOnto >= buttonList.Count)
            return;

        if (buttonToMoveOnto != lastButtonIndex)
        {
            currentButton = buttonList[buttonToMoveOnto];
            currentButton.GetComponent<Button>().Select();
            lastButtonIndex = buttonToMoveOnto;
        }
        //Debug.Log(buttonToMoveOnto);
    }

    public void BlockSubmit()
    {
        submitBlocked = true;
        lastSubmitTime = Time.unscaledTime;
    }

    public void UnblockSubmit()
    {
        submitBlocked = false;
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
        if (buttonList == null || buttonList.Count == 0 || !context.performed)
            return;

        Vector2 direction = context.ReadValue<Vector2>();

        if (direction.y > 0.5f)
        {
            if (buttonToMoveOnto < 0)
                buttonToMoveOnto = 0;
            else
                buttonToMoveOnto = (buttonToMoveOnto - 1 + buttonList.Count) % buttonList.Count;
        }
        else if (direction.y < -0.5f)
        {
            if (buttonToMoveOnto < 0)
                buttonToMoveOnto = 0;
            else
                buttonToMoveOnto = (buttonToMoveOnto + 1) % buttonList.Count;
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
            if (submitBlocked && Time.unscaledTime - lastSubmitTime < submitBlockTime)
            {
                Debug.Log("[PauseMenuNavigation] Submit blocked due to cooldown.");
                return;
            }

            GameObject selected = EventSystem.current.currentSelectedGameObject;

            if (selected != null)
            {
                Button button = selected.GetComponent<Button>();
                if (button != null && button.interactable)
                {
                    button.onClick.Invoke();
                    lastSubmitTime = Time.unscaledTime;
                    submitBlocked = true;
                }
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

