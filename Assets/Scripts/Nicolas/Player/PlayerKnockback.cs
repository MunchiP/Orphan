using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    public float knockbackForce = 10f;
    public float knockbackDuration = 0.2f;
    public float knockbackCooldown = 1f;
    public int dañoPorGolpe = 20;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isKnockedBack = false;
    private bool canKnockback = true;
    private PlayerController playerController;
    private PlayerState playerState;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        playerController = GetComponent<PlayerController>();
        playerState = GetComponent<PlayerState>();

        if (rb == null)
            Debug.LogError("No se encontró Rigidbody2D en el jugador.", this);
        if (anim == null)
            Debug.LogError("No se encontró Animator en el jugador.", this);
    }

    // Método knockback que recibe posición de partícula
    public void ApplyKnockback(Vector2 sourceParticlePosition)
    {
        if (isKnockedBack || !canKnockback) return;

        Debug.Log("Knockback aplicado desde posición de partícula: " + sourceParticlePosition);

        anim.SetTrigger("knockout");
        isKnockedBack = true;
        canKnockback = false;

        if (playerController != null)
            playerController.isKnockedBack = true;

        Vector2 direction = (transform.position.x > sourceParticlePosition.x) ? Vector2.right : Vector2.left;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);

        if (playerState != null)
            playerState.QuitarVida(dañoPorGolpe);

        Invoke(nameof(ResetKnockback), knockbackDuration);
        Invoke(nameof(ResetCooldown), knockbackCooldown);
    }

    // Método knockback que recibe contacto y transform enemigo (tu versión original)
    public void ApplyKnockback(Vector2 contactPoint, Transform enemigoTransform, bool knockbackConImpulsoVertical = false, bool esTacleo = false)
    {
        if (isKnockedBack || !canKnockback) return;

        Debug.Log("Knockback aplicado desde contacto con enemigo: " + enemigoTransform.name);

        anim.SetTrigger("knockout");
        isKnockedBack = true;
        canKnockback = false;

        if (playerController != null)
            playerController.isKnockedBack = true;

        Vector2 direction = (transform.position.x > enemigoTransform.position.x) ? Vector2.right : Vector2.left;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);

        if (playerState != null)
            playerState.QuitarVida(dañoPorGolpe);

        Invoke(nameof(ResetKnockback), knockbackDuration);
        Invoke(nameof(ResetCooldown), knockbackCooldown);
    }

    void ResetKnockback()
    {
        isKnockedBack = false;
        if (playerController != null)
            playerController.isKnockedBack = false;
    }

    void ResetCooldown()
    {
        canKnockback = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemigo"))
        {
            Vector2 contacto = other.ClosestPoint(transform.position);
            ApplyKnockback(contacto, other.transform);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Estalagmita"))
        {
            ContactPoint2D contacto = collision.GetContact(0);
            ApplyKnockback(contacto.point);
            Destroy(collision.gameObject);
        }
    }
}
