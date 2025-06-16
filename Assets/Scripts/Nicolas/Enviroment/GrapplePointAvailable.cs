using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GrapplePointAvailable : MonoBehaviour
{
    private Transform player;               // Referencia al jugador
    private float distance;               
    private Light2D luz;

    void Start()
    {
        player = GameObject.Find("PlayerF2").transform;
        // Obtener el SpriteRenderer del objeto
        luz = GetComponentInChildren<Light2D>();
        luz.intensity = 0f;
    }

    void Update()
    {
        if (player == null) return;

        // Calcular la distancia en 2D (X y Y)
        Vector2 distanceVector = new Vector2(
            transform.position.x - player.position.x,
            transform.position.y - player.position.y
        );

        distance = distanceVector.magnitude;

        // Cambiar el sprite dependiendo de la distancia
        if (distance < 4.6f)
        {
            luz.intensity = 1f;
        }
        else
        {
            luz.intensity = 0f;
        }
    }
}
