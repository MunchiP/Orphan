using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class GrappleLiana : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    private bool swingPressedThisFrame;
    private float horizontalMoveInput;

    [Header("Grapple (Liana)")]
    public float detectionRadius = 5f;
    public LayerMask grappleLayer;
    public LineRenderer lineRenderer;

    [Header("Swing Boost")]
    public float swingBoostForce = 8f;

    Rigidbody2D rb;
    DistanceJoint2D swingJoint;
    public Vector2 grapplePoint;
    public bool isAttached;

    public GameObject boquita;
    public GameObject cabeza;
    public Quaternion rotationSave;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lineRenderer.positionCount = 0;

        if (boquita == null) boquita = GameObject.Find("Boquita");
        if (cabeza == null) cabeza = GameObject.Find("Cabeza");

        if (boquita == null)
            Debug.LogWarning("GrappleLiana: No se encontró el GameObject 'Boquita'.");

        inputActions = new InputSystem_Actions();

        // Suscripción a eventos en Awake para poder desuscribirlos después
        inputActions.Player.Swing.performed += OnSwingPerformed;
        inputActions.Player.Move.performed += OnMovePerformed;
        inputActions.Player.Move.canceled += OnMoveCanceled;
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();

        // Desuscribir eventos para evitar memory leaks o errores
        inputActions.Player.Swing.performed -= OnSwingPerformed;
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Move.canceled -= OnMoveCanceled;
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        anim.SetBool("swing", isAttached);

        if (swingPressedThisFrame)
        {
            if (swingJoint == null)
            {
                rotationSave = transform.rotation;
                Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius, grappleLayer);

                Transform bestValidTarget = null;
                float bestValidDist = float.MaxValue;

                foreach (var c in hits)
                {
                    if (boquita != null && boquita.transform.position.y < c.transform.position.y)
                    {
                        float d = Vector2.Distance(transform.position, c.transform.position);
                        if (d < bestValidDist)
                        {
                            bestValidDist = d;
                            bestValidTarget = c.transform;
                        }
                    }
                }

                if (bestValidTarget != null)
                {
                    isAttached = true;
                    grapplePoint = bestValidTarget.position;
                    CreateSwingJoint();
                }
            }
            else
            {
                RemoveSwingJoint();
            }
        }

        swingPressedThisFrame = false;

        if (swingJoint != null && lineRenderer.positionCount >= 2 && boquita != null)
        {
            lineRenderer.SetPosition(0, boquita.transform.position);
            lineRenderer.SetPosition(1, grapplePoint);
        }
    }

    private void FixedUpdate()
    {
        if (swingJoint != null)
        {
            if (Mathf.Abs(horizontalMoveInput) > 0.1f)
                rb.AddForce(new Vector2(horizontalMoveInput * swingBoostForce, 0));
        }
    }

    private void CreateSwingJoint()
    {
        swingJoint = gameObject.AddComponent<DistanceJoint2D>();
        swingJoint.connectedAnchor = grapplePoint;
        swingJoint.autoConfigureDistance = false;
        swingJoint.distance = Vector2.Distance(transform.position, grapplePoint);
        swingJoint.enableCollision = false;

        lineRenderer.positionCount = 2;
    }

    public void RemoveSwingJoint()
    {
        isAttached = false;
        Destroy(swingJoint);
        swingJoint = null;
        lineRenderer.positionCount = 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    // Métodos para los callbacks del Input System

    private void OnSwingPerformed(InputAction.CallbackContext context)
    {
        swingPressedThisFrame = true;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        horizontalMoveInput = context.ReadValue<Vector2>().x;
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        horizontalMoveInput = 0f;
    }
}
