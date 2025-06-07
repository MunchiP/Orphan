using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    [Header("Configuración de Knockback")]
    public float knockbackForce = 10f;
    public float knockbackDuration = 0.2f;
    public float knockbackCooldown = 1f;
    public int dañoPorGolpe = 20;

    private Rigidbody2D rb;
    private PlayerController playerController;
    private PlayerState playerState;
    private bool isKnockedBack = false;
    private bool canKnockback = true;
    private Animator anim;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
        playerState = GetComponent<PlayerState>();

        if (rb == null)
            Debug.LogError("No se encontró Rigidbody2D en el jugador.", this);
        if (playerController == null)
            Debug.LogError("No se encontró PlayerController en el jugador.", this);
        if (playerState == null)
            Debug.LogError("No se encontró PlayerState en el jugador.", this);
    }
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canKnockback || isKnockedBack) return;

        if (other.CompareTag("Enemigo"))
        {
            Vector2 contacto = other.ClosestPoint(transform.position);

            // Verifica si es un enemigo volador
            EnemigoVoladorIA enemigoIA = other.GetComponent<EnemigoVoladorIA>();
            if (enemigoIA != null)
            {
                if (!enemigoIA.enCooldownPostAtaque)
                {
                    ApplyKnockback(contacto);
                    enemigoIA.PausarTrasGolpear();
                }
            }
            else
            {
                ApplyKnockback(contacto);
            }
        }
    }

    public void ApplyKnockback(Vector2 contactPoint)
    {
        if (isKnockedBack || !canKnockback) return;
        anim.SetTrigger("knockout");
        isKnockedBack = true;
        canKnockback = false;

        if (playerController != null)
            playerController.isKnockedBack = true;

        Vector2 direction = (Vector2)transform.position - contactPoint;
        direction.y = 0f;
        direction.Normalize();

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);

        // 🔴 Aplica daño al jugador
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
}
