using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    public EnemyState enemyState;
    private float knockbackForce = 5f;
    private Rigidbody2D rb;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyState = GetComponent<EnemyState>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Arma"))
        {
            anim.SetTrigger("Hurt");
            Vector3 knockDirection = (transform.position - other.transform.position).normalized;
            rb.AddForce(knockDirection * knockbackForce, ForceMode2D.Impulse);
            enemyState.TomarDano(25);
        }
    }
}