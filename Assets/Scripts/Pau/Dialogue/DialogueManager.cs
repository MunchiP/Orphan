using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject dialogueBox2;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private Image characterImage;

    public HelperMonolithData helperMonolithData;
    public MonoBehaviour playerControllerScript;

    private Queue<string> dialogueLines;
    private Coroutine typingCoroutine;
    private string currentLine;
    private bool shouldStopPlayer;

    private Action onDialogueEndCallback; // 🔸 CALLBACK

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
            playerControllerScript = player.GetComponent<MonoBehaviour>();
        }
    }

    public void StartDialogue(string[] lines, bool stopPlayer = false, Action onDialogueEnd = null)
    {
        shouldStopPlayer = stopPlayer;
        onDialogueEndCallback = onDialogueEnd;

        if (!dialogueBox.activeInHierarchy)
        {
            if (shouldStopPlayer)
            {
                PlayerController player = FindAnyObjectByType<PlayerController>();
                Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                rb.linearVelocity = Vector2.zero;
                Animator anim = player.GetComponentInChildren<Animator>();
                anim.SetFloat("walk", 0);
                if (player != null)
                    player.enabled = false;
            }

            dialogueBox.SetActive(true);
            dialogueLines.Clear();

            foreach (string line in lines)
                dialogueLines.Enqueue(line);

            DisplayNextLine();
        }
        else
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }

            dialogueLines.Clear();
            dialogueBox.SetActive(false);

            if (playerControllerScript != null)
                playerControllerScript.enabled = true;
        }
    }

    void DisplayNextLine()
    {
        if (dialogueLines.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentLine = dialogueLines.Dequeue();

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine(currentLine));
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

        dialogueLines.Clear();
        dialogueBox.SetActive(false);

        if (shouldStopPlayer)
        {
            PlayerController player = FindAnyObjectByType<PlayerController>();
            if (player != null)
                player.enabled = true;
        }

        onDialogueEndCallback?.Invoke();  // 🔸 EJECUTA CALLBACK
        onDialogueEndCallback = null;
    }

    public void UpdateDialogue(CharacterData characterData, string monolithKey = "")
    {
        if (characterData == null || characterName == null) return;

        characterName.text = characterData.CharacterName;
        characterName.color = characterData.NameColor;

        if (characterData.IsMonolith && !string.IsNullOrEmpty(monolithKey))
        {
            Sprite monolithSpecificImage = helperMonolithData.GetImageForMonolith(monolithKey);
            characterImage.sprite = monolithSpecificImage != null ? monolithSpecificImage : characterData.Portrait;
        }
        else
        {
            characterImage.sprite = characterData.Portrait;
        }
    }

    public bool IsDialogueActive => dialogueBox.activeInHierarchy;

    public void AdvanceDialogue()
    {
        if (!IsDialogueActive) return;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
            dialogueText.text = currentLine;
            return;
        }

        DisplayNextLine();
    }
}
