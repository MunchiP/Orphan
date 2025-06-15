using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;


    // Relaciono los mismos elementos que deben estar en el CharacterData que son los del ScriptableObject
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject dialogueBox2;
    // [SerializeField] private Image dialogueBoxImg;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI characterName;
    // [SerializeField] private TextMeshProUGUI characterNameColor;
    [SerializeField] private Image characterImage;

    private Queue<string> dialogueLines;
    private Coroutine typingCoroutine;

    // diccionario de imagenes de los monolithssss
    public HelperMonolithData helperMonolithData;

    // Para no tocar el script del jugador lo referencio aca para bloquearlo mientras habla con el pangolin
    public MonoBehaviour playerControllerScript;


    private bool shouldStopPlayer; // detener al jugador



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        dialogueLines = new Queue<string>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerControllerScript = player.GetComponent<MonoBehaviour>(); // hago referencia al script del jugador
        }
    }

    // public void StartDialogue(string[] lines) // modifico el parametro para agregar la validacion de quien detiene al jugador y quien no
    public void StartDialogue(string[] lines , bool stopPlayer = false) // parametro que se complementa en DialogueSO
    {
        if (!dialogueBox.activeInHierarchy)
        {

            if (shouldStopPlayer)
            {
                PlayerController player = FindAnyObjectByType<PlayerController>();

                if (player != null)
                {
                    player.enabled = false; // detener al jugador incluso aunque siga caminando
                }
            }
            dialogueBox.SetActive(true);
            dialogueLines.Clear();

            foreach (string line in lines)
            {
                dialogueLines.Enqueue(line);
            }
            DisplayNextLine();
        }
        else
        {
            if (typingCoroutine != null)// Detener el tipeo si está activo
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }

            dialogueLines.Clear();
            dialogueBox.SetActive(false);

            if (playerControllerScript != null) // refuerzo de reactivar el movimiento jugador
            {
                playerControllerScript.enabled = true;
            }
        }
    }

    void DisplayNextLine()
    {
        if (dialogueLines.Count == 0)
        {
            EndDialogue();
            return;
        }

        string line = dialogueLines.Dequeue();

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeLine(line));
    }

    private IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";
        foreach (char item in line.ToCharArray())
        {
            dialogueText.text += item;
            yield return new WaitForSeconds(0.04f);
        }

        typingCoroutine = null;
    }

    public void EndDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        dialogueLines.Clear(); // Asegura que siempre se limpie
        dialogueBox.SetActive(false);

        if (shouldStopPlayer)
        {
            PlayerController player = FindAnyObjectByType<PlayerController>();
            if (player != null)
            {
                player.enabled = true;
            }
        }
    }




    // Código utilizado porque ahora utilizo ScriptableObject en los diálogos

    public void UpdateDialogue(CharacterData characterData, string monolithKey = "")
    {
        characterName.text = characterData.CharacterName;
        characterName.color = characterData.NameColor;
        Debug.Log($"Color del nombre: {characterData.NameColor} (alpha: {characterData.NameColor.a})");


        if (characterData.IsMonolith && !string.IsNullOrEmpty(monolithKey))
        {
            Sprite monolithSpecificImage = helperMonolithData.GetImageForMonolith(monolithKey);
            characterImage.sprite = monolithSpecificImage != null ? monolithSpecificImage : characterData.Portrait;
        }
        else
        {
            characterImage.sprite = characterData.Portrait;

        }
        Debug.Log($"Nombre: {characterData.CharacterName}, ¿Monolith?: {characterData.IsMonolith}, Imagen: {(characterData.Portrait != null ? characterData.Portrait.name : "NULL")}");

    }
}
