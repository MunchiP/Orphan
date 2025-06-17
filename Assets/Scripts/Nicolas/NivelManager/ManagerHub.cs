using UnityEngine;
using UnityEngine.Playables;

public class ManagerHub : MonoBehaviour, IInteractable
{
    [SerializeField] CharacterData characterData;
    [SerializeField] private GameObject exclamation;
    [TextArea] public string[] lines;
    [SerializeField] private bool stopPlayerMovement = true;
    private PlayerChangeClothes playerChangeClothes;
    private PlayableDirector cinematicaPango;

    private PlayerState playerState;


    void Start()
    {
        if (PlayerPrefs.GetInt("clothes", 0) > 0)
        {
            cinematicaPango = FindAnyObjectByType<PlayableDirector>();
            if (cinematicaPango != null && cinematicaPango.gameObject.activeInHierarchy)
            {
                cinematicaPango.gameObject.SetActive(false);
            }
            lines = new string[]
            {
            "No deben estar muy lejos...",
            "Cuento contigo, Lilith."
            };
        }

    }

    void OnEnable()
    {
        if (PlayerPrefs.GetInt("clothes", 0) > 0)
        {
            cinematicaPango = FindAnyObjectByType<PlayableDirector>();
            if (cinematicaPango != null && cinematicaPango.gameObject.activeInHierarchy)
            {
                cinematicaPango.gameObject.SetActive(false);
            }
        }
    }

    public void Interact()
    {

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

        DialogueManager.Instance.StartDialogue(lines, stopPlayerMovement, OnDialogueFinished);
    }

    private void OnDialogueFinished()
    {
        // Si es la primera vez
        if (PlayerPrefs.GetInt("clothes", 0) == 0)
        {
            playerChangeClothes = FindAnyObjectByType<PlayerChangeClothes>();
            if (playerChangeClothes != null)
            {
                playerChangeClothes.Change();
            }

            // Guardar que ya no es la primera vez
            PlayerPrefs.SetInt("clothes", 1);
            PlayerPrefs.Save();

            // Cambiar líneas para las siguientes interacciones
            lines = new string[]
            {
            "No deben estar muy lejos...",
            "Cuento contigo, Lilith."
            };

        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            exclamation.SetActive(true);
            playerState = collision.GetComponent<PlayerState>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            exclamation.SetActive(false);
            DialogueManager.Instance.EndDialogue();
            playerState = null;
        }
    }
}
