using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSidePawController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject leftPaw;
    public GameObject rightPaw;
    public float offsetPositiveX;
    public float offsetNegativeX;
    private Vector3 buttonPosition;
    private VerticalOnlyNavigation keyboardController;
    public int thisButtonIndex;

    void Start()
    {
        leftPaw.SetActive(false);
        rightPaw.SetActive(false);
        buttonPosition = GetComponent<RectTransform>().localPosition;
        keyboardController = FindFirstObjectByType<VerticalOnlyNavigation>();
        
    }

    

    public void OnPointerEnter(PointerEventData eventData)
    {
        keyboardController?.SetMouseHoverState(true);
        keyboardController?.SetMouseHoverButtonIndex(thisButtonIndex);
        EventSystem.current.SetSelectedGameObject(gameObject);
        HandlerToEnter();        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        keyboardController?.SetMouseHoverState(false);
        keyboardController?.SetMouseHoverButtonIndex(-1);
        EventSystem.current.SetSelectedGameObject(null);
        HandlerToExit();       
    }

    public void OnPointerEnter()
    {
        HandlerToEnter();
    }

    public void OnPointerExit()
    {
        HandlerToExit();
    }

    public void HandlerToEnter()
    {
        leftPaw.GetComponent<RectTransform>().localPosition = new Vector3(buttonPosition.x + offsetNegativeX, buttonPosition.y, buttonPosition.z);
        leftPaw.SetActive(true);
        rightPaw.GetComponent<RectTransform>().localPosition = new Vector3(buttonPosition.x + offsetPositiveX, buttonPosition.y, buttonPosition.z);
        rightPaw.SetActive(true);
    }

    public void HandlerToExit()
    {
        leftPaw.SetActive(false);
        rightPaw.SetActive(false);
    }
  
}
