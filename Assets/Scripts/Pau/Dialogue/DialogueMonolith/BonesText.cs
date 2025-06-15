using UnityEngine;

public class BonesText : MonoBehaviour
{
    [SerializeField] private GameObject hinText;

    void Start()
    {
        if (hinText != null)
        {
            hinText.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && hinText != null)
        {
            hinText.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && hinText != null)
        {
            hinText.SetActive(false);
        }
    }
}
