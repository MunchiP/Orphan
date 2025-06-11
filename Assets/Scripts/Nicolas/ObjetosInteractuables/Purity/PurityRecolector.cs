using UnityEngine;

public class PurityRecolector : MonoBehaviour
{
    private PlayerState playerState;
    private PlayerAudioEvents playerAudioEvents;
    public AudioClip audioPureza;

    void Start()
    {
        playerState = FindAnyObjectByType<PlayerState>();
        playerAudioEvents = FindAnyObjectByType<PlayerAudioEvents>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerAudioEvents.Play(audioPureza, 1f);
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
