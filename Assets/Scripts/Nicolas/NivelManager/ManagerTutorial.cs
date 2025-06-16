using UnityEngine;

public class ManagerTutorial : MonoBehaviour
{
    private PlayerState playerState;

    [SerializeField] CharacterData characterData; // Asigna desde el editor
    [TextArea(2, 5)]
    [SerializeField] private string[] lineasDialogo; // Las líneas que dirá el personaje

    public void PrimeraPureza()
{
    if (DialogueManager.Instance != null)
    {
        DialogueManager.Instance.UpdateDialogue(characterData); // imagen y nombre
        DialogueManager.Instance.StartDialogue(lineasDialogo, true); // true = bloquear movimiento
    }
    else
    {
        Debug.LogWarning("No se encontró el DialogueManager.");
    }
}

}
