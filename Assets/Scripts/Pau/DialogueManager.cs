using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject dialogueBox;

    private Queue<string> dialogueLines;
    private Coroutine typingCoroutine;

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
    }

    public void StartDialogue(string[] lines)
    {
        if (!dialogueBox.activeInHierarchy)
        {
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
            // Detener el tipeo si está activo
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }

            dialogueLines.Clear();
            dialogueBox.SetActive(false);
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
    }
}
