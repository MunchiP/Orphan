using UnityEngine;

public class PurityRecolector : MonoBehaviour
{
    private PlayerState playerState;

    void Start()
    {
        playerState = FindAnyObjectByType<PlayerState>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerState.AgregarPureza(10);
            if (playerState.vidaActual < 100)
            {
                int aleatorio = Random.Range(0, 10);
                if (aleatorio < 4)
                {
                    playerState.AgregarVida(20);
                }
            }
            Destroy(this.gameObject);
        }
    }
}
