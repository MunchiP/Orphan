using UnityEngine;

public class PurityRecolectorDrop : MonoBehaviour
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
            playerAudioEvents.Play(audioPureza);
            playerState.AgregarPureza(5);
            Destroy(this.gameObject);
        }
    }
}
