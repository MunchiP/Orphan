using UnityEngine;
using System.Collections;

public class EnemyKnockbackBossOne : MonoBehaviour
{
    public EnemyStateBossOne enemyState;
    public float knockbackForce = 3f;
    public float knockbackDuration = 0.15f;
    public float damageCooldown = 0.5f;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isKnockedBack = false;
    private bool canBeHit = true;

    private EnemigoVoladorIA enemigoVoladorIA;
    private EnemigoPatrulla enemigoPatrulla;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyState = GetComponent<EnemyStateBossOne>();
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
        if (!canBeHit || isKnockedBack) return;

        if (other.CompareTag("Arma"))
        {
            canBeHit = false;

            // Solo activa la animación si no está ya en "hurt"
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("hurt"))
            {
                anim.SetTrigger("hurt");
            }

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
        yield return new WaitForSeconds(damageCooldown);
        canBeHit = true;
    }
}
