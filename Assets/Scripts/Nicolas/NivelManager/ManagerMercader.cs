using UnityEngine;
using UnityEngine.Playables;

public class ManagerMercader : MonoBehaviour, IInteractable
{
    [SerializeField] CharacterData characterData;
    [SerializeField] private GameObject exclamation;
    [TextArea] private string[] lines;
    [SerializeField] private bool stopPlayerMovement = true;
    private PlayerChangeClothes playerChangeClothes;

    private PlayerState playerState;
    private HabilitiesActivatorSwitch habilitiesActivatorSwitch;


    void Start()
    {

    }

    public void Interact()
    {
        // Si es la primera vez
        if (PlayerPrefs.GetInt("hability1", 0) == 0)
        {
            if (playerState.purezaActual >= 400)
            {
                lines = new string[]
            {
            "¿400 de pureza? Con eso me compras fácil...",
            "Toma esta habilidad... te va a abrir nuevas posibilidades.",
            "Escala, salta… y si te falta práctica, ya sabes dónde encontrarme."
            };
                playerState.QuitarPureza(400);
                playerState.ActualizarHUD();
                // Guardar que ya no es la primera vez
                PlayerPrefs.SetInt("hability1", 1);
                PlayerPrefs.Save();
                habilitiesActivatorSwitch.ActivarHabilidad1();
            }
            else
            {
                lines = new string[]
           {
            "Mmm... así no me convences.",
            "Vuelve cuando tengas los 400... de pureza, claro.",
            "Y no te tardes, me gusta cuando insisten."
           };
            }
        }
        else if (PlayerPrefs.GetInt("hability1") == 1)
        {
            lines = new string[]
            {
            "¿Ya te gustó venir a verme, eh?",
            "Lástima que ya te di lo que querías...",
            "Ahora ve, sigue subiendo... y sudando."
            };
        }





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

    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            exclamation.SetActive(true);
            playerState = collision.GetComponent<PlayerState>();
            habilitiesActivatorSwitch = collision.GetComponent<HabilitiesActivatorSwitch>();
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
