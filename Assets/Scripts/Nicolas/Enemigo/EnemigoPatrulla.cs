using UnityEngine;
using System.Collections;

public class EnemigoPatrulla : MonoBehaviour
{
    public float velocidad = 2f;
    public float tiempoEsperaGiro = 1f;

    public Transform detectorSuelo;
    public Transform detectorPared;
    public LayerMask capaSuelo;
    public LayerMask capaPared;
    public float distanciaDeteccionSuelo = 0.5f;
    public float distanciaDeteccionPared = 0.2f;

    private bool moviendoDerecha = true;
    private bool girando = false;

    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        if (!girando)
        {
            float movimiento = (moviendoDerecha ? -1 : 1) * velocidad;
            rb.linearVelocity = new Vector2(movimiento, rb.linearVelocity.y);
            animator.SetBool("velocity", true);

            bool sinSuelo = !Physics2D.Raycast(detectorSuelo.position, Vector2.down, distanciaDeteccionSuelo, capaSuelo);
            bool hayPared = Physics2D.Raycast(detectorPared.position, moviendoDerecha ? Vector2.right : Vector2.left, distanciaDeteccionPared, capaPared);

            if (sinSuelo || hayPared)
            {
                StartCoroutine(EsperarYGirar());
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            animator.SetBool("velocity", false);
        }
    }

    IEnumerator EsperarYGirar()
    {
        girando = true;
        yield return new WaitForSeconds(tiempoEsperaGiro);
        moviendoDerecha = !moviendoDerecha;

        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;

        girando = false;
    }

    public void PausarTrasGolpear(float duracion = 0.3f)
    {
        StartCoroutine(PausaMovimiento(duracion));
    }

    private IEnumerator PausaMovimiento(float duracion)
    {
        girando = true;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        animator.SetBool("velocity", false);

        yield return new WaitForSeconds(duracion);

        girando = false;
    }

    void OnDrawGizmos()
    {
        if (detectorSuelo != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(detectorSuelo.position, detectorSuelo.position + Vector3.down * distanciaDeteccionSuelo);
        }

        if (detectorPared != null)
        {
            Gizmos.color = Color.blue;
            Vector3 dir = moviendoDerecha ? Vector3.right : Vector3.left;
            Gizmos.DrawLine(detectorPared.position, detectorPared.position + dir * distanciaDeteccionPared);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerKnockback knockback = collision.gameObject.GetComponent<PlayerKnockback>();
            if (knockback != null)
            {
                Vector2 contactPoint = collision.contacts[0].point;
                knockback.ApplyKnockback(contactPoint);
            }
        }
    }
}
