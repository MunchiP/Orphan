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

        keyboardController.SetMouseHoverState(true);
        keyboardController.SetMouseHoverButtonIndex(thisButtonIndex);

        // 🔥 Limpiar selección previa y seleccionar este botón
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (keyboardController == null)
            keyboardController = FindFirstObjectByType<VerticalOnlyNavigation>();

        keyboardController.SetMouseHoverState(false);
        keyboardController.SetMouseHoverButtonIndex(-1);

        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
