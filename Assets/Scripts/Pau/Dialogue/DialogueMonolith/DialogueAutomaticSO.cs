using UnityEngine;

public class DialogueAutomaticSO : MonoBehaviour
{

    [SerializeField] CharacterData characterData;
    [TextArea] public string[] lines;
    

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite != null)
            {
                string spriteName = sr.sprite.name;
                DialogueManager.Instance.UpdateDialogue(characterData, spriteName);
            }
            DialogueManager.Instance.StartDialogue(lines);
        }
        else
        {
            DialogueManager.Instance.UpdateDialogue(characterData);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DialogueManager.Instance.EndDialogue();
        }
    }
}
