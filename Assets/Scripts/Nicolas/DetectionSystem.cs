using UnityEngine;

public class DetectionSystem : MonoBehaviour
{
    public Transform player;
    public float distanciaDeteccion = 10f;
    public LineRenderer lenguaRenderer; // Usamos LineRenderer para dibujar la lengua
    private DistanceJoint2D joint;
    private Vector2 attachPoint;

    void Start()
    {
        joint = player.GetComponent<DistanceJoint2D>();
        joint.enabled = false;

        if (lenguaRenderer != null)
        {
            lenguaRenderer.positionCount = 2;
            lenguaRenderer.enabled = false;
        }
    }

    void Update()
    {
        Vector2 distancia = (Vector2)transform.position - (Vector2)player.position;
        float distanciaActual = distancia.magnitude;

        if (distanciaActual < distanciaDeteccion)
        {
            RaycastHit2D hit = Physics2D.Raycast(player.position, distancia.normalized, distanciaActual);
            if (hit.collider != null && hit.collider.CompareTag("Plataforma"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    attachPoint = hit.point;

                    joint.enabled = true;
                    joint.connectedAnchor = attachPoint;
                    joint.autoConfigureDistance = false;
                    joint.distance = Vector2.Distance(player.position, attachPoint);

                    if (lenguaRenderer != null)
                    {
                        lenguaRenderer.enabled = true;
                    }
                }
            }
        }

        // Mientras mantienes presionada la E, sigue enganchado
        if (joint.enabled && Input.GetKey(KeyCode.E))
        {
            if (lenguaRenderer != null)
            {
                lenguaRenderer.SetPosition(0, player.position);
                lenguaRenderer.SetPosition(1, attachPoint);
            }
        }
        else if (joint.enabled && Input.GetKeyUp(KeyCode.E))
        {
            joint.enabled = false;

            if (lenguaRenderer != null)
            {
                lenguaRenderer.enabled = false;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (player == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(player.position, transform.position);
    }
}
