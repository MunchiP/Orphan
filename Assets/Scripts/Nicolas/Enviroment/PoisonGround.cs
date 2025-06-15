using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PoisonGround : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerState playerState = other.GetComponent<PlayerState>();
            playerState.QuitarVida(100);
        }
    }
}
