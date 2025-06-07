using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class InventoryPanelRotation : MonoBehaviour
{
    public List<GameObject> menuScreensList = new List<GameObject>();
    private RectTransform screenPosition1;
    private RectTransform screenPosition2;
    private RectTransform screenPosition3;
    private Vector2 leftPosition;
    private Vector2 centerPosition;
    private Vector2 rightPosition;
    private int currentRotation;

    public Button rightPawArrow;
    public Button leftPawArrow;

    private bool wasItMoveToLeftBefore = false;
    private bool wasItMoveToRightBefore = false;
    public bool isMenuRotating;
    public float transitionSpeed;
    
    void Start()
    {
        
    }

    private void OnEnable()
    {
        menuScreensList[0].SetActive(false);
        menuScreensList[1].SetActive(true);
        menuScreensList[2].SetActive(false);

        currentRotation = -1;

        screenPosition1 = menuScreensList[0].GetComponent<RectTransform>();
        screenPosition2 = menuScreensList[1].GetComponent<RectTransform>();
        screenPosition3 = menuScreensList[2].GetComponent<RectTransform>();

        leftPosition = screenPosition1.anchoredPosition;
        centerPosition = screenPosition2.anchoredPosition;
        rightPosition = screenPosition3.anchoredPosition;
    }

    private void OnDisable()
    {       
      
        screenPosition1.anchoredPosition = leftPosition;
        screenPosition2.anchoredPosition = centerPosition;
        screenPosition3.anchoredPosition = rightPosition;
        
        
    }


    void Update()
    {
        Debug.Log(currentRotation);
    }

    public void MovePauseScreenToLeft()
    {
        if (currentRotation == -1 && !wasItMoveToRightBefore)
        {
            currentRotation = menuScreensList.Count - 1;
        }
        else if (currentRotation == 2 && wasItMoveToRightBefore)
        {
            currentRotation = 2;
        }

        else if (currentRotation == 0 && wasItMoveToRightBefore)
        {
            currentRotation = 0;
        }
        else if (currentRotation == 1 && wasItMoveToRightBefore)
        {
            currentRotation = 1;
        }
        else
        {
            currentRotation = (currentRotation - 1 + menuScreensList.Count) % menuScreensList.Count;
        }

        switch (currentRotation)
        {
            case 2:
                {
                    menuScreensList[0].SetActive(true);
                    menuScreensList[2].SetActive(false);
                    StartCoroutine(SlideScreenToPosition(screenPosition1, centerPosition, transitionSpeed));
                    StartCoroutine(SlideScreenToPosition(screenPosition2, rightPosition, transitionSpeed));
                    StartCoroutine(SlideScreenToPosition(screenPosition3, leftPosition, transitionSpeed));
                    break;
                }
            case 1:
                {
                    menuScreensList[2].SetActive(true);
                    menuScreensList[1].SetActive(false);
                    StartCoroutine(SlideScreenToPosition(screenPosition1, rightPosition, transitionSpeed));
                    StartCoroutine(SlideScreenToPosition(screenPosition2, leftPosition, transitionSpeed));
                    StartCoroutine(SlideScreenToPosition(screenPosition3, centerPosition, transitionSpeed));

                    break;
                }
            case 0:
                {
                    menuScreensList[0].SetActive(false);
                    menuScreensList[1].SetActive(true);
                    menuScreensList[2].SetActive(true);
                    StartCoroutine(SlideScreenToPosition(screenPosition1, leftPosition, transitionSpeed));
                    StartCoroutine(SlideScreenToPosition(screenPosition2, centerPosition, transitionSpeed));
                    StartCoroutine(SlideScreenToPosition(screenPosition3, rightPosition, transitionSpeed));

                    break;
                }
        }
        
        wasItMoveToLeftBefore = true;
        wasItMoveToRightBefore = false;

    }

    public void MovePauseScreenToRight()
    {
        if (currentRotation == -1 && !wasItMoveToLeftBefore)
        {
            currentRotation = 0;
        }
        else if (currentRotation == 2 && wasItMoveToLeftBefore)
        {
            currentRotation = 2;
        }
        else if (currentRotation == 0 && wasItMoveToLeftBefore)
        {
            currentRotation = 0;
        }
        else if (currentRotation == 1 && wasItMoveToLeftBefore)
        {
            currentRotation = 1;
        }
        else
        {
            currentRotation = (currentRotation + 1) % menuScreensList.Count;
        }   

        switch (currentRotation)
        {
            case 0:
                {
                    menuScreensList[0].SetActive(false);
                    menuScreensList[2].SetActive(true);
                    StartCoroutine(SlideScreenToPosition(screenPosition1, rightPosition, transitionSpeed));
                    StartCoroutine(SlideScreenToPosition(screenPosition2, leftPosition, transitionSpeed));
                    StartCoroutine(SlideScreenToPosition(screenPosition3, centerPosition, transitionSpeed));
                    break;
                }
            case 1:
                {
                    menuScreensList[0].SetActive(true);
                    menuScreensList[1].SetActive(false);
                    StartCoroutine(SlideScreenToPosition(screenPosition1, centerPosition, transitionSpeed));
                    StartCoroutine(SlideScreenToPosition(screenPosition2, rightPosition, transitionSpeed));
                    StartCoroutine(SlideScreenToPosition(screenPosition3, leftPosition, transitionSpeed));

                    break;
                }
            case 2:
                {
                    menuScreensList[2].SetActive(false);
                    menuScreensList[0].SetActive(true);
                    menuScreensList[1].SetActive(true);
                    StartCoroutine(SlideScreenToPosition(screenPosition1, leftPosition, transitionSpeed));
                    StartCoroutine(SlideScreenToPosition(screenPosition2, centerPosition, transitionSpeed));
                    StartCoroutine(SlideScreenToPosition(screenPosition3, rightPosition, transitionSpeed));

                    break;
                }
        }
        wasItMoveToRightBefore = true;
        wasItMoveToLeftBefore = false;
    }

    public IEnumerator SlideScreenToPosition(RectTransform rect, Vector2 positionToReach, float duration)
    {
        isMenuRotating = true;
        rightPawArrow.interactable = false;
        leftPawArrow.interactable = false;
        Vector2 start = rect.anchoredPosition;

        float t = 0f;

        while (t < 1)
        {
            t += Time.unscaledDeltaTime / duration;
            rect.anchoredPosition = Vector2.Lerp(start, positionToReach, t);
            yield return null;
        }
        yield return new WaitForSecondsRealtime(0.4f);
        isMenuRotating = false;
        rightPawArrow.interactable = true;
        leftPawArrow. interactable = true;
        rect.anchoredPosition = positionToReach;
    }
}
