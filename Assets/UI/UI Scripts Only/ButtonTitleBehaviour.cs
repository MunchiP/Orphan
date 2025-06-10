using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ButtonTitleBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Image spriteToChange;
    public Sprite defaultSprite;
    public Sprite hoverSprite;
    
    
    
    
    private void Start()
    {
       
     
    }
  

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        
            
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
       if (spriteToChange != null)
        {
            spriteToChange.sprite = defaultSprite;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
      
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
     
        
    }

    
}
