using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PoisonGround : MonoBehaviour
{
    private float timeInside = 0f;
    private bool isInside = false;
    private bool canTakeDamage = true;
    private Coroutine damageCoroutine;
    private PlayerState playerState;
    private Animator anim;

    private void Start()
    {
        playerState = FindAnyObjectByType<PlayerState>();
        anim = playerState.gameObject.GetComponentInChildren<Animator>();
}
    private void Update()
    {
        if (isInside && canTakeDamage)
        {
            timeInside += Time.deltaTime;

            if (timeInside >= 1f)
            {
                damageCoroutine = StartCoroutine(DoDamageAndImmunity());
                timeInside = 0f; // Reiniciar por si se queda
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInside = true;
            timeInside = 0f; // Comienza a contar desde 0
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInside = false;
            timeInside = 0f;
        }
    }

    private IEnumerator DoDamageAndImmunity()
    {
        playerState.QuitarVida(20);
        anim.SetTrigger("knockout"); 
        canTakeDamage = false;

        yield return new WaitForSeconds(4f); // Tiempo de inmunidad

        canTakeDamage = true;
    }
}
