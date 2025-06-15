using UnityEngine;
using System.Collections;

public class BossOneBehaviour : MonoBehaviour
{
    [Header("Configuración")]
    public float chaseSpeed = 3f;
    public float attackDistance = 1.5f;
    public float stopChaseDistance = 2.5f;
    public string attackTriggerName = "attack";
    public float attackCooldown = 1.5f;

    [Header("Switch Behavior")]
    public float checkInterval = 5f;
    [Range(0, 1)] public float switchProbability = 0.2f;

    [Header("Referencias")]
    public Transform specialPositionTarget;
    public GameObject teleportEffectPrefab; // <-- NUEVO PREFAB

    private BossOneTacle bossOneTacle;
    private BossOneJumpAttack bossOneJumpAttack;
    private FixedPositionSwordSpawner swordSpawner;
    private VerticalSwordGroupSpawner verticalSwordGroupSpawner;
    private BossOneAudioEvents bossOneAudioEvents;

    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;

    private bool canAttack = true;
    private bool isWaitingToAttack = false;
    private bool isDoingSpecialAction = false;
    private bool initialDelayDone = false;

    private float timer = 0f;

    private Coroutine specialCoroutine;
    private Coroutine waitAndAttackCoroutine;

    private void OnEnable()
    {
        timer = 0f;
        canAttack = true;
        isWaitingToAttack = false;
        isDoingSpecialAction = false;
        initialDelayDone = false;

        if (anim != null)
            anim.SetBool("special1", false);

        StartCoroutine(InitialDelayBeforeSpecials());
    }

    private IEnumerator InitialDelayBeforeSpecials()
    {
        yield return new WaitForSeconds(checkInterval);
        initialDelayDone = true;
    }

    private void Start()
    {
        bossOneAudioEvents = GetComponent<BossOneAudioEvents>();
        swordSpawner = GetComponentInChildren<FixedPositionSwordSpawner>();
        bossOneTacle = GetComponent<BossOneTacle>();
        bossOneJumpAttack = GetComponentInChildren<BossOneJumpAttack>();
        verticalSwordGroupSpawner = GetComponentInChildren<VerticalSwordGroupSpawner>();

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
            Debug.LogError("No se encontró un GameObject con tag 'Player'");

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void OnDisable()
    {
        if (specialCoroutine != null)
        {
            StopCoroutine(specialCoroutine);
            specialCoroutine = null;
        }
        if (waitAndAttackCoroutine != null)
        {
            StopCoroutine(waitAndAttackCoroutine);
            waitAndAttackCoroutine = null;
        }

        isDoingSpecialAction = false;
        canAttack = true;
        isWaitingToAttack = false;

        rb.linearVelocity = Vector2.zero;
    }

    void Update()
    {
        anim.SetFloat("move", Mathf.Abs(rb.linearVelocityX));
    }

    private void FixedUpdate()
    {
        if (isDoingSpecialAction)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (initialDelayDone)
        {
            timer += Time.fixedDeltaTime;

            if (timer >= checkInterval)
            {
                if (Random.value <= switchProbability)
                {
                    if (waitAndAttackCoroutine != null)
                    {
                        StopCoroutine(waitAndAttackCoroutine);
                        waitAndAttackCoroutine = null;
                        isWaitingToAttack = false;
                    }

                    specialCoroutine = StartCoroutine(ActivarComportamientoEspecial());
                    return;
                }
                else
                {
                    timer = 0f;
                }
            }
        }

        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > stopChaseDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(direction.x * chaseSpeed, rb.linearVelocityY);

            if (direction.x > 0)
                transform.localScale = new Vector3(-1, 1, 1);
            else if (direction.x < 0)
                transform.localScale = new Vector3(1, 1, 1);

            anim.ResetTrigger(attackTriggerName);
            isWaitingToAttack = false;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;

            if (distanceToPlayer <= attackDistance && !isWaitingToAttack && canAttack)
            {
                waitAndAttackCoroutine = StartCoroutine(WaitAndAttack());
            }
        }
    }

    private IEnumerator WaitAndAttack()
    {
        isWaitingToAttack = true;
        yield return new WaitForSeconds(0.13f);

        if (player != null && Vector2.Distance(transform.position, player.position) <= attackDistance && canAttack)
        {
            anim.SetTrigger(attackTriggerName);
            StartCoroutine(AttackCooldown());
        }

        isWaitingToAttack = false;
        waitAndAttackCoroutine = null;
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private IEnumerator ActivarComportamientoEspecial()
    {
        isDoingSpecialAction = true;
        rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(1.5f);

        int opcion = Random.Range(0, 4); // 0-3

        if (waitAndAttackCoroutine != null)
        {
            StopCoroutine(waitAndAttackCoroutine);
            waitAndAttackCoroutine = null;
            isWaitingToAttack = false;
        }

        if (opcion == 0 && bossOneTacle != null)
        {
            Debug.Log("Activando EnemyTacle");
            bossOneTacle.enabled = true;
            this.enabled = false;
        }
        else if (opcion == 1 && bossOneJumpAttack != null)
        {
            Debug.Log("Activando BossOneJumpAttack");
            bossOneJumpAttack.enabled = true;
            this.enabled = false;
        }
        else if (opcion == 2 && swordSpawner != null)
        {
            Debug.Log("Activando FixedPositionSwordSpawner");

            if (specialPositionTarget != null && teleportEffectPrefab != null)
                Instantiate(teleportEffectPrefab, transform.position, Quaternion.identity);
            bossOneAudioEvents.PlayTeleportAudio();

            transform.position = specialPositionTarget.position;
            transform.localScale = new Vector3(1, 1, 1);
            rb.linearVelocity = Vector2.zero;
            anim.SetBool("special1", true);
            this.enabled = false;
        }
        else if (opcion == 3 && verticalSwordGroupSpawner != null)
        {
            Debug.Log("Activando VerticalSwordGroupSpawner");

            if (specialPositionTarget != null && teleportEffectPrefab != null)
                Instantiate(teleportEffectPrefab, transform.position, Quaternion.identity);
            bossOneAudioEvents.PlayTeleportAudio();
            transform.position = specialPositionTarget.position;
            transform.localScale = new Vector3(1, 1, 1);
            rb.linearVelocity = Vector2.zero;
            anim.SetBool("special2", true);
            verticalSwordGroupSpawner.enabled = true;
            this.enabled = false;
        }
        else
        {
            Debug.LogWarning("No se pudo activar comportamiento especial.");
            isDoingSpecialAction = false;
        }

        specialCoroutine = null;
    }
}
