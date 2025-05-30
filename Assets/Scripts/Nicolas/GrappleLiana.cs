using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GrappleLiana : MonoBehaviour
{
    [Header("Grapple (Liana)")]
    public float detectionRadius = 5f;
    public LayerMask grappleLayer;
    public LineRenderer lineRenderer;

    [Header("Swing Boost")]
    public float swingBoostForce = 8f;

    Rigidbody2D rb;
    DistanceJoint2D swingJoint;
    public Vector2 grapplePoint;
    float hInput;
    public bool isAttached;
    private GameObject boquita;
    private GameObject cabeza;
    public Quaternion rotationSave;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lineRenderer.positionCount = 0;
        boquita = GameObject.Find("Boquita");
        cabeza = GameObject.Find("Cabeza");
    }

    void Update()
    {
        hInput = Input.GetAxisRaw("Horizontal");

        // --- Intento de enganchar liana ---
        if (Input.GetKeyDown(KeyCode.E) && swingJoint == null)
        {
            rotationSave = transform.rotation;
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius, grappleLayer);
            if (hits.Length > 0)
            {
                isAttached = true;
                Transform best = hits[0].transform;
                float bestDist = Vector2.Distance(transform.position, best.position);
                foreach (var c in hits)
                {
                    float d = Vector2.Distance(transform.position, c.transform.position);
                    if (d < bestDist)
                    {
                        bestDist = d;
                        best = c.transform;
                    }
                }
                grapplePoint = best.position;
                CreateSwingJoint();
            }
        }

        // --- Soltar liana ---
        if (Input.GetKeyDown(KeyCode.Space) && swingJoint != null)
        {
            RemoveSwingJoint();
        }

        // --- Dibuja la cuerda ---
        if (swingJoint != null && lineRenderer.positionCount >= 2)
        {
            lineRenderer.SetPosition(0, boquita.transform.position);
            lineRenderer.SetPosition(1, grapplePoint);
        }
    }

    void FixedUpdate()
    {
        if (swingJoint != null)
        {
            // Empuje lateral para balanceo
            if (Mathf.Abs(hInput) > 0.1f)
                rb.AddForce(new Vector2(hInput * swingBoostForce, 0));
        }
    }

    void CreateSwingJoint()
    {
        swingJoint = gameObject.AddComponent<DistanceJoint2D>();
        swingJoint.connectedAnchor = grapplePoint;
        swingJoint.autoConfigureDistance = false;
        swingJoint.distance = Vector2.Distance(transform.position, grapplePoint);
        swingJoint.enableCollision = false;

        lineRenderer.positionCount = 2;
    }

    void RemoveSwingJoint()
    {
        isAttached = false;
        Destroy(swingJoint);
        swingJoint = null;
        lineRenderer.positionCount = 0;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
