using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance; // Patrón Singletosn que me permite que otros scripts acceden facilmente a mi DialogoManager

    [SerializeField] private TextMeshProUGUI dialogueText; // Referencia al objeto de UI donde se muestra el texto del diálogo
    [SerializeField] private GameObject dialogueBox; // Caja de diálogo completa 
    private Queue<string> dialogueLines; // Cola de líneas de texto que se irán mostrando en orden


    private void Awake()
    {
        if (Instance == null)  // Verifico que sólo haya un singletón  y si hay otro se destruye para usar thissss
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // No se destruye al cambio de scena
        }
        else
        {
            Destroy(gameObject); // Si existe el otro destruye el duplicado
        }
        dialogueLines = new Queue<string>();  // Inicia la lista de dialogos vacía
    }

    public void StartDialogue(string[] lines)
    {
        dialogueBox.SetActive(true);  // Mostrar mi caja de dialogo en la pantalla
        dialogueLines.Clear();  //Elimina cualquier dialogo anterior
        foreach (string line in lines) // Recorre cada una de las lineas guardadas y ....
        {
            dialogueLines.Enqueue(line); // Añade cada línea a la cola
        }
        DisplayNextLine(); // Comienza mostrando la primera linea
    }

    void DisplayNextLine()
    {
        if (dialogueLines.Count == 0)
        {
            EndDialogue(); // Si ya no queda lineas, termina el diálogo.
            return;
        }

        string line = dialogueLines.Dequeue();  // Toma la siguiente linea para agregarla a la cola
        //StopAllCoroutines(); // Detiene cualquier corrutina en proceso 
        StartCoroutine(TypeLine(line)); // Inicia el efecto de tipeo con la línea actual
    }

    private IEnumerator TypeLine(string line)
    {
        dialogueText.text = ""; // Limpia el texto actual
        foreach (char item in line.ToCharArray())
        {
            dialogueText.text += item; // Agrega letra por letra al texto
            yield return new WaitForSeconds(0.04f); // Espera un poco entre las letrras para hacer el efecto de tipeo
        }
    }

    void EndDialogue()
    {
        dialogueBox.SetActive(false); // Se oculta la caja cuando terminan los textos.
    }

}

