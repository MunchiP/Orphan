using UnityEngine;

public class DialogueSO : MonoBehaviour, IInteractable
{

    // Este es un buen lugar para recolectar pureza.
    [SerializeField] CharacterData characterData;
    [SerializeField] private GameObject exclamation;
    [TextArea] public string[] lines; // Almaceno las lineas de código que mostraré

    [SerializeField] private bool stopPlayerMovement = false;



    public void Interact()
    {
        // Asegurar que se actualice el personaje en pantalla justo antes de mostrar el diálogo
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null && sr.sprite != null)
        {
            string spriteName = sr.sprite.name;
            DialogueManager.Instance.UpdateDialogue(characterData, spriteName);
        }
        else
        {
            DialogueManager.Instance.UpdateDialogue(characterData);
        }

        DialogueManager.Instance.StartDialogue(lines, stopPlayerMovement);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            exclamation.SetActive(true);
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
