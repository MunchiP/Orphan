using UnityEngine;

public class MonolithOne : MonoBehaviour, IInteractable
{

    [SerializeField] private GameObject exclamation;

    [TextArea] public string[] lines; // Almaceno las lineas de código que mostraré
                                      // private bool playerInRange = false; //Asegura que el dialogo se ejecute una sola vez cuando el jugador está cerca
                                      // private InputSystem_Actions inputActions;

    // private void Awake()
    // {
    //     inputActions = new InputSystem_Actions();
    // }

    // private void Update()
    // {
    //     // if (playerInRange && Input.GetKeyDown("Swing"))
    //     //     if (playerInRange && Input.GetKeyDown(KeyCode.C))
    //     //     {

    //     //         playerInRange = false;
    //     //         //   Debug.Log("Update del límite collider");
    //     //     }
    // }

    // void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         playerInRange = true;
    //         exclamation.SetActive(true);
    //         // Debug.Log("REconoce el límite collider");
    //     }
    // }
    // void OnTriggerExit2D(Collider2D other)
    // {
    //     if (other.gameObject.CompareTag("Player"))
    //         playerInRange = false;
    //     exclamation.SetActive(false);
    //     // Debug.Log("Sale del límite collider");
    // }


    public void Interact()
    {
        DialogueManager.Instance.StartDialogue(lines);
    }
    
      void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            exclamation.SetActive(true);
            // Debug.Log("REconoce el límite collider");
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



    // void OnTriggerEnter2D(Collider2D other)
    // {
    //             if (!playerInRange && other.CompareTag("Player")) // Si el dialogo no se ha activado !  - y si el objeto que se acerca es un Player
    //     {
    //         playerInRange = true; // el dialogo se activa
    //         DialogueManager.Instance.StartDialogue(lines); // Llama el método en el SialogueManager renglón por renglón, línea por línea
    //     }
    // }