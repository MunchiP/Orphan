using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseButtonIndexController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private PauseMenuNavigation keyboardController;

    public int thisButtonIndex;

    public void Start()
    {
        if (keyboardController == null)
            keyboardController = FindFirstObjectByType<PauseMenuNavigation>();
    }

   
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (keyboardController == null)
            keyboardController = FindFirstObjectByType<PauseMenuNavigation>();
        //Debug.Log($"[PointerEnter] Hovering index {thisButtonIndex}, controller null? {keyboardController == null}");
        Debug.Log($"[ButtonIndexController] OnPointerEnter: '{gameObject.name}', index {thisButtonIndex}");

        if (CompareTag("GoBackButtonUI"))
        {
            if (keyboardController.buttonList.Contains(gameObject))
            {
                thisButtonIndex = keyboardController.buttonList.Count - 1;
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
            int idx = keyboardController.buttonList.Count - 1;
            thisButtonIndex = idx;

            keyboardController.buttonToMoveOnto = idx;
            keyboardController.lastButtonIndex = idx;

            EventSystem.current.SetSelectedGameObject(gameObject);
            keyboardController.SetMouseHoverButtonIndex(-1);
        }
        else
        {
            keyboardController?.SetMouseHoverButtonIndex(-1);
            EventSystem.current.SetSelectedGameObject(null);
        }

    }


}
