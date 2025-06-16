using Unity.VisualScripting;
using UnityEngine;

public class EstalagmitaBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    public float valorGravedad = 1f;
    private bool jugadorEntro = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            rb.gravityScale = valorGravedad;
            jugadorEntro = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (jugadorEntro)
        {
            Destroy(this.gameObject);
        }
        
    }
}
