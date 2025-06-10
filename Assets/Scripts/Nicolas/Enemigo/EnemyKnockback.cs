using UnityEngine;
using System.Collections;

public class EnemyKnockback : MonoBehaviour
{
    public EnemyState enemyState;
    public float knockbackForce = 3f;
    public float knockbackDuration = 0.15f;
    public float damageCooldown = 0.3f; // Tiempo entre golpes permitidos

    private Rigidbody2D rb;
    private Animator anim;
    private bool isKnockedBack = false;
    private bool canBeHit = true;

    private EnemigoVoladorIA enemigoVoladorIA;
    private EnemigoPatrulla enemigoPatrulla;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyState = GetComponent<EnemyState>();
        anim = GetComponent<Animator>();
        enemigoVoladorIA = GetComponent<EnemigoVoladorIA>();
        enemigoPatrulla = GetComponent<EnemigoPatrulla>();

        if (anim == null)
        {
            anim = GetComponentInChildren<Animator>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Arma") && !isKnockedBack && canBeHit)
        {
            anim.SetTrigger("hurt");

            Vector2 knockDirection = new Vector2(transform.position.x - other.bounds.center.x, 0).normalized;
            StartCoroutine(ApplyKnockback(knockDirection));

            enemyState.TomarDano(25);

            if (enemigoVoladorIA != null)
            {
                enemigoVoladorIA.PausarTrasGolpear();
            }

            if (enemigoPatrulla != null)
            {
                enemigoPatrulla.PausarTrasGolpear(0.3f);
            }

            StartCoroutine(DamageCooldown());
        }
    }

    private IEnumerator ApplyKnockback(Vector2 direction)
    {
        isKnockedBack = true;

        float timer = 0f;
        float speed = knockbackForce;
        Vector2 start = rb.position;

        while (timer < knockbackDuration)
        {
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        isKnockedBack = false;
    }

    private IEnumerator DamageCooldown()
    {
        canBeHit = false;
        yield return new WaitForSeconds(damageCooldown);
        canBeHit = true;
    }
}
