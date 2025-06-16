using UnityEngine;
using System.Collections;

public class BossOneJumpAttack : MonoBehaviour
{
    [Header("Configuración de salto parabólico")]
    public float jumpHeight = 5f;
    public float minHorizontalDistance = 3f;
    public float maxHorizontalDistance = 7f;
    public float jumpDuration = 0.8f;

    [Header("Detección de suelo")]
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;
    public Transform groundCheckPoint;

    [Header("Detección de paredes")]
    public float wallCheckDistance = 0.5f;
    public LayerMask wallLayer;

    [Header("Partículas al aterrizar")]
    public GameObject landingParticles;

    private Animator anim;
    private Transform player;
    private Transform parentTransform;

    private bool isJumping = false;

    private void Awake()
    {
        anim = GetComponentInParent<Animator>();
        parentTransform = transform.parent;

        if (parentTransform == null)
            Debug.LogWarning("BossOneJumpAttack: No se encontró el padre inmediato.");

        if (groundCheckPoint == null)
            groundCheckPoint = parentTransform;

        Debug.Log("BossOneJumpAttack: Awake - Componentes inicializados. Padre: " + parentTransform.name);
    }

    private void Update()
    {
        if (player != null && parentTransform != null)
        {
            Vector3 scale = parentTransform.localScale;
            scale.x = player.position.x < parentTransform.position.x ? 1 : -1;
            parentTransform.localScale = scale;
        }
    }

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player != null && !isJumping && parentTransform != null)
        {
            isJumping = true;
            anim.SetBool("isJumping", true);
            StartCoroutine(JumpRoutine());
        }
        else
        {
            Debug.LogWarning("BossOneJumpAttack: Jugador no encontrado, salto en progreso o padre nulo.");
        }
    }

    private IEnumerator JumpRoutine()
    {
        yield return new WaitForSeconds(0.5f);

        Vector2 dirToPlayer = (player.position - parentTransform.position).normalized;
        dirToPlayer = AdjustDirectionIfWall(dirToPlayer);

        // Primer salto hacia el jugador (o dirección ajustada)
        yield return JumpInParabola(dirToPlayer);

        // Segundo salto en la dirección contraria
        yield return JumpInParabola(-dirToPlayer);

        // Tercer salto vuelve a la dirección original
        yield return JumpInParabola(dirToPlayer);

        // Espera 2 segundos ANTES de reactivar BossOneBehaviour
        yield return new WaitForSeconds(2f);

        anim.SetBool("isJumping", false);
        isJumping = false;

        // Habilita el BossOneBehaviour en el padre
        BossOneBehaviour bossBehaviour = parentTransform.GetComponent<BossOneBehaviour>();
        if (bossBehaviour != null)
        {
            bossBehaviour.enabled = true;
            Debug.Log("BossOneBehaviour habilitado después del salto.");
        }
        else
        {
            Debug.LogWarning("No se encontró BossOneBehaviour en el padre.");
        }

        // Desactiva este script
        this.enabled = false;
    }

    private IEnumerator JumpInParabola(Vector2 direction)
    {
        float horizontalDistance = UnityEngine.Random.Range(minHorizontalDistance, maxHorizontalDistance);
        float elapsedTime = 0f;

        Vector3 startPos = parentTransform.position;
        float dirX = Mathf.Sign(direction.x);
        Vector3 proposedEndPos = startPos + new Vector3(dirX * horizontalDistance, 0, 0);

        // Verificar si hay una pared entre origen y destino
        RaycastHit2D wallBetween = Physics2D.Raycast(startPos, Vector2.right * dirX, horizontalDistance, wallLayer);

        // Verificar si hay una pared justo en el destino
        RaycastHit2D wallAtEnd = Physics2D.Raycast(proposedEndPos, Vector2.right * dirX, wallCheckDistance, wallLayer);

        if (wallBetween.collider != null || wallAtEnd.collider != null)
        {
            Debug.Log("Pared detectada entre el origen y el destino o en el destino. Invirtiendo dirección.");
            dirX *= -1;
            proposedEndPos = startPos + new Vector3(dirX * horizontalDistance, 0, 0);
        }

        Vector3 endPos = proposedEndPos;
        Debug.Log($"Saltando de {startPos} a {endPos}");

        while (elapsedTime < jumpDuration)
        {
            anim.SetBool("jump", true);
            float normalizedTime = elapsedTime / jumpDuration;
            float t;

            if (normalizedTime < 0.5f)
            {
                t = normalizedTime;
            }
            else
            {
                t = 0.5f + (normalizedTime - 0.5f) * 1.5f;
                if (t > 1f) t = 1f;
            }

            float x = Mathf.Lerp(startPos.x, endPos.x, t);
            float y = startPos.y + jumpHeight * 4 * t * (1 - t);
            parentTransform.position = new Vector3(x, y, startPos.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        anim.SetBool("jump", false);
        parentTransform.position = endPos;

        BossOneAudioEvents bossOneAudioEvents = FindAnyObjectByType<BossOneAudioEvents>();
        bossOneAudioEvents.PlayJumpAttack2();

        if (landingParticles != null)
            Instantiate(landingParticles, groundCheckPoint.position, Quaternion.identity);

        yield return new WaitForSeconds(1f);
    }


    private Vector2 AdjustDirectionIfWall(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(parentTransform.position, Vector2.right * Mathf.Sign(direction.x), wallCheckDistance, wallLayer);
        if (hit.collider != null)
        {
            Debug.Log("AdjustDirectionIfWall: Pared detectada, invirtiendo dirección.");
            return new Vector2(-1 * Mathf.Sign(direction.x), 0).normalized;
        }
        return direction;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * wallCheckDistance);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * wallCheckDistance);
    }
}
