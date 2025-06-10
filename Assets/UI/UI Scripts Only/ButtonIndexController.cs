using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ButtonIndexController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
   
    
    private VerticalOnlyNavigation keyboardController;
    
    public int thisButtonIndex;

    public void SetKeyboardController(VerticalOnlyNavigation controller)
    {
        keyboardController = controller;

    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        if (keyboardController == null)
            keyboardController = FindFirstObjectByType<VerticalOnlyNavigation>();
        //Debug.Log($"[ButtonIndexController] OnPointerEnter: '{gameObject.name}', index {thisButtonIndex}");

        if (CompareTag("GoBackButtonUI"))
        {
            if (keyboardController.buttonList.Contains(gameObject))
            {
                
                thisButtonIndex = keyboardController.buttonList.IndexOf(gameObject);
                Debug.Log($"[ButtonIndexController] GoBackButton detected, overriding index to {thisButtonIndex}");
            }
            else
            {
                thisButtonIndex = -1;
            }
        }

        keyboardController?.SetMouseHoverButtonIndex(thisButtonIndex);
        EventSystem.current.SetSelectedGameObject(gameObject);
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (CompareTag("GoBackButtonUI"))
        {

            int idx = keyboardController.buttonList.IndexOf(gameObject);
            if (idx >= 0)
            {
                thisButtonIndex = idx;


                keyboardController.buttonToMoveOnto = idx;
                keyboardController.lastButtonIndex = idx;

                EventSystem.current.SetSelectedGameObject(gameObject);
            }
            else
            {
                Debug.LogWarning("GoBackButton not found in controller list!");
            }


            keyboardController.SetMouseHoverButtonIndex(-1);
        }
        else
        {
            keyboardController?.SetMouseHoverButtonIndex(-1);
            EventSystem.current.SetSelectedGameObject(null);
            //Debug.Log($"[ButtonIndexController] OnPointerExit: '{gameObject.name}', index {thisButtonIndex}");
        }

    }





}

