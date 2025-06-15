using System;
using NUnit.Framework;
//using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class PauseMenuNavigation : MonoBehaviour, InputSystem_Actions.IUIActions
{

    private InputSystem_Actions controlsUI;
    public List<GameObject> buttonList = new List<GameObject>();
    public int buttonToMoveOnto;
    private GameObject currentButton;
    
    public int lastButtonIndex = -1;
    private bool submitBlocked = false;
    private float submitBlockTime = 0.3f; // 100ms, tweak as needed
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

    public void UnblockSubmitAfterDelay(float delay)
    {
        Debug.Log($"[PauseMenuNavigation] Starting unblock coroutine with delay: {delay}");
        StartCoroutine(UnblockAfterDelayCoroutine(delay));
    }

    private IEnumerator UnblockAfterDelayCoroutine(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        submitBlocked = false;
        Debug.Log("[PauseMenuNavigation] Submit unblocked after delay.");
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
        if (!context.performed || submitBlocked) return;

        GameObject selected = EventSystem.current.currentSelectedGameObject;

        if (selected == null)
        {
            Debug.LogWarning("[PauseNavigation] Submit pressed, but no GameObject is selected.");
            return;
        }

        Debug.Log($"[PauseNavigation] Submit pressed on: {selected.name}");

        Button button = selected.GetComponent<Button>();

        if (button == null)
        {
            Debug.LogWarning($"[PauseNavigation] Selected object '{selected.name}' has no Button component.");
            return;
        }

        if (!button.interactable)
        {
            Debug.LogWarning($"[PauseNavigation] Button '{button.name}' is not interactable.");
            return;
        }

        Debug.Log($"[PauseNavigation] Invoking button '{button.name}'");
        button.onClick.Invoke();
        Debug.Log("[PauseNavigation] User is pressing Enter");

        BlockSubmit();
        UnblockSubmitAfterDelay(0.3f);
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

