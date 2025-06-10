using Unity.VisualScripting;
using UnityEngine;

public class UnderElevator : MonoBehaviour
{
    public bool isUnder = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isUnder = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isUnder = false;
        }
    }
}
