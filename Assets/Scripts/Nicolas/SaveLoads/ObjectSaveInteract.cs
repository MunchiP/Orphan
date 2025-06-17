using UnityEngine;

public class ObjectSaveInteract : MonoBehaviour, IInteractable
{
    [SerializeField] CharacterData characterData;
    [SerializeField] private GameObject exclamation;
    [TextArea] public string[] lines;
    [SerializeField] private bool stopPlayerMovement = false;

    private PlayerState playerState; // 🔹 Se almacena aquí al entrar el jugador

    public void Interact()
    {
        // Mostrar diálogo
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

        // Guardar partida con referencia al jugador
        if (playerState != null)
        {
            playerState.ActualizarHUD();
            playerState.AgregarVida(100);
            GameManagerSave.Instance.GuardarPartida(playerState);
            Debug.Log("[ObjectSaveInteract] Partida guardada correctamente");
        }
        else
        {
            Debug.LogWarning("[ObjectSaveInteract] No se encontró el PlayerState para guardar");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            exclamation.SetActive(true);
            playerState = collision.GetComponent<PlayerState>(); // 🔹 Asignamos al colisionar
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            exclamation.SetActive(false);
            DialogueManager.Instance.EndDialogue();
            playerState = null; // 🔹 Limpiamos por si acaso
        }
    }
}
