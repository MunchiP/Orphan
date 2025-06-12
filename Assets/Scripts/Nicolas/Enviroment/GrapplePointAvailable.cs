using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GrapplePointAvailable : MonoBehaviour
{
    private Transform player;               // Referencia al jugador
    private float distance;               
    public Sprite avaliable;              // Sprite cuando el punto está disponible
    public Sprite offAvaliable;           // Sprite cuando el punto no está disponible

    private SpriteRenderer spriteRenderer; // Referencia al componente SpriteRenderer
    private Light2D luz;

    void Start()
    {
        player = GameObject.Find("PlayerF2").transform;
        // Obtener el SpriteRenderer del objeto
        spriteRenderer = GetComponent<SpriteRenderer>();
        luz = GetComponent<Light2D>();
        luz.intensity = 0f;
    }

    void Update()
    {
        if (player == null || spriteRenderer == null) return;

        // Calcular la distancia en 2D (X y Y)
        Vector2 distanceVector = new Vector2(
            transform.position.x - player.position.x,
            transform.position.y - player.position.y
        );

        distance = distanceVector.magnitude;

        // Cambiar el sprite dependiendo de la distancia
        if (distance < 5f)
        {
            spriteRenderer.sprite = avaliable;
            luz.intensity = 1f;
        }
        else
        {
            spriteRenderer.sprite = offAvaliable;
            luz.intensity = 0f;
        }
    }
}
