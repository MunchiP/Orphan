using UnityEngine;

public class EventAnimatorSounds : MonoBehaviour
{
    EnemyAudio enemyAudio;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyAudio = GetComponentInParent<EnemyAudio>();
    }

    // Update is called once per frame
    void SonidoHurt()
    {
        enemyAudio.PlayHurtSound();
    }
}
