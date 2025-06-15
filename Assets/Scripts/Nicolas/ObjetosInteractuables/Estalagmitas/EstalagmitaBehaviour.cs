using Unity.VisualScripting;
using UnityEngine;

public class EstalagmitaBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    public float valorGravedad = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            rb.gravityScale = valorGravedad;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Destroy(this.gameObject);
    }
}
