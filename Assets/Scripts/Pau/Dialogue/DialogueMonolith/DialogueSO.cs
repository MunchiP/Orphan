using UnityEngine;

public class DialogueSO : MonoBehaviour , IInteractable
{

    // Este es un buen lugar para recolectar pureza.
    [SerializeField] CharacterData characterData;

    [SerializeField] private GameObject exclamation;

    [TextArea] public string[] lines; // Almaceno las lineas de código que mostraré
                                      // private bool playerInRange = false; //Asegura que el dialogo se ejecute una sola vez cuando el jugador está cerca
                                      // private InputSystem_Actions inputActions;



    // void OnMouseDown()
    // {
    //     DialogueManager.Instance.UpdateDialogue(characterData);
    // }


        public void Interact()
    {
        DialogueManager.Instance.StartDialogue(lines);
    }
    
      void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //  DialogueManager.Instance.UpdateDialogue(characterData); // cambiado para el dicccccc
            // DialogueManager.Instance.UpdateDialogue(characterData, gameObject.name);
            SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
            if (sr != null && sr.sprite != null)
            {
                string spriteName = sr.sprite.name;
                DialogueManager.Instance.UpdateDialogue(characterData, spriteName); // no es exactamente el Monolith sino el hijo el que tiene le nombre
            }
            exclamation.SetActive(true);
            // Debug.Log("REconoce el límite collider");
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            exclamation.SetActive(false);
            DialogueManager.Instance.EndDialogue();
        }
    }
}
