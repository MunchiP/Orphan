using UnityEngine;
using System.Collections;

public class BossOneTacle : MonoBehaviour
{
    public float fuerzaTacleo = 10f;
    private Rigidbody2D rb;
    private Transform jugador;
    private BossOneBehaviour bossOneBehaviour;

    private Coroutine finTacleoCoroutine;
    private Coroutine delayCoroutine;
    private Animator anim;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        jugador = GameObject.FindGameObjectWithTag("Player")?.transform;
        bossOneBehaviour = GetComponent<BossOneBehaviour>();
        anim = GetComponent<Animator>();

        anim.SetBool("tacle", true);
        delayCoroutine = StartCoroutine(DelayAndStartTacle());

        // Orientación inicial hacia el jugador
        OrientarHaciaJugador();
    }

    void OnDisable()
    {
        StopAllCoroutines();
        rb.linearVelocity = Vector2.zero;
    }

    void Update()
    {
        if (jugador != null && rb.linearVelocity.magnitude > 0f)
        {
            // Mientras se mueve, sigue mirando hacia el jugador
            OrientarHaciaJugador();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && this.enabled == true)
        {
            Debug.Log("¡Tacleó al jugador!");

            PlayerKnockback knockback = collision.gameObject.GetComponent<PlayerKnockback>();
            if (knockback != null)
            {
                Vector2 puntoContacto = collision.contacts[0].point;
                knockback.ApplyKnockback(puntoContacto, this.transform, true, true);
            }
            StopAllCoroutines();  // Para todas las corrutinas
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
            anim.SetBool("tacle", false);
            bossOneBehaviour.enabled = true;
            this.enabled = false;
        }
    }

    private IEnumerator DelayAndStartTacle()
    {
        yield return new WaitForSeconds(3f);

        if (jugador != null)
        {
            Vector2 direccion = jugador.position - transform.position;
            direccion.y = 0f;
            direccion.Normalize();
            BossOneAudioEvents bossOneAudioEvents = GetComponent<BossOneAudioEvents>();
            bossOneAudioEvents.PlayAttack22();
            rb.linearVelocity = direccion * fuerzaTacleo;

            // Orientar en el momento que empieza el tacle
            OrientarHaciaJugador();

            // Después de iniciar el tacle, esperar 2 segundos para finalizar
            finTacleoCoroutine = StartCoroutine(FinTacleoDespuesDe2Seg());
        }
    }

    private IEnumerator FinTacleoDespuesDe2Seg()
    {
        yield return new WaitForSeconds(2f);
        anim.SetBool("tacle", false);
        rb.linearVelocity = Vector2.zero;
        bossOneBehaviour.enabled = true;
        this.enabled = false;
    }

    private void OrientarHaciaJugador()
    {
        if (jugador == null)
            return;

        float direccionX = jugador.position.x - transform.position.x;

        if (direccionX > 0)
            transform.localScale = new Vector3(-1, 1, 1); // Mirando a la derecha
        else if (direccionX < 0)
            transform.localScale = new Vector3(1, 1, 1);  // Mirando a la izquierda
    }
}
