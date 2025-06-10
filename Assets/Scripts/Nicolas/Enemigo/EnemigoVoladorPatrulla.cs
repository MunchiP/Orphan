using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemigoVoladorIA : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidadMovimiento = 4f;
    public float tiempoEntreActualizacionesPath = 0.5f;
    public float radioPatrulla = 2f;
    public float tiempoEntrePuntosPatrulla = 2f;

    [Header("Distancias")]
    public float rangoDeteccionJugador = 8f;
    public float distanciaParaAtacar = 0.5f;

    private Transform jugador;
    private Rigidbody2D rb;
    private Seeker seeker;
    private Animator anim;

    private Path path;
    private int currentWaypoint = 0;

    public bool persiguiendoJugador = false;
    public bool enCooldownPostAtaque = false;

    private Vector2 posicionInicial;

    void Awake()
    {
        GameObject jugadorGO = GameObject.FindGameObjectWithTag("Player");
        if (jugadorGO != null)
        {
            jugador = jugadorGO.transform;
        }
        else
        {
            Debug.LogWarning("Jugador no encontrado al iniciar. Se volverá a intentar más tarde.");
        }

        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();

        if (seeker == null)
            Debug.LogError("No se encontró Seeker en el enemigo");

        if (rb == null)
            Debug.LogError("No se encontró Rigidbody2D en el enemigo");

        posicionInicial = transform.position;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        InvokeRepeating(nameof(ActualizarPath), 0f, tiempoEntreActualizacionesPath);
        StartCoroutine(PatrullarSiNoDetectaJugador());
    }

    void ActualizarPath()
    {
        if (jugador == null)
        {
            GameObject jugadorGO = GameObject.FindGameObjectWithTag("Player");
            if (jugadorGO != null)
            {
                jugador = jugadorGO.transform;
                Debug.Log("Jugador asignado dinámicamente.");
            }
            else
            {
                return;
            }
        }

        if (!persiguiendoJugador)
        {
            float distanciaAlJugador = Vector2.Distance(transform.position, jugador.position);

            if (distanciaAlJugador <= rangoDeteccionJugador)
            {
                persiguiendoJugador = true;
                StopCoroutine(PatrullarSiNoDetectaJugador());
                Debug.Log("Jugador detectado, persiguiendo.");
            }
            else
            {
                return;
            }
        }

        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, jugador.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (path == null || currentWaypoint >= path.vectorPath.Count || enCooldownPostAtaque)
        {
            anim.SetBool("velocity", false);
            return;
        }

        Vector2 siguientePunto = (Vector2)path.vectorPath[currentWaypoint];
        Vector2 direccion = (siguientePunto - rb.position).normalized;

        float distanciaAlPunto = Vector2.Distance(rb.position, siguientePunto);

        if (distanciaAlPunto < 0.1f)
        {
            currentWaypoint++;
        }

        Vector2 nuevaPosicion = rb.position + direccion * velocidadMovimiento * Time.fixedDeltaTime;
        rb.MovePosition(nuevaPosicion);

        anim.SetBool("velocity", true); // Activamos animación de movimiento
    }

    public void PausarTrasGolpear()
    {
        anim.SetBool("velocity", false); // Detenemos animación de movimiento
        if (!enCooldownPostAtaque)
            StartCoroutine(CooldownPostAtaque());
    }

    IEnumerator CooldownPostAtaque()
    {
        enCooldownPostAtaque = true;
        persiguiendoJugador = false;
        path = null;

        yield return new WaitForSeconds(1f);

        enCooldownPostAtaque = false;
    }

    IEnumerator PatrullarSiNoDetectaJugador()
    {
        while (!persiguiendoJugador)
        {
            Vector2 destinoAleatorio = posicionInicial + Random.insideUnitCircle * radioPatrulla;

            if (seeker != null && seeker.IsDone())
            {
                seeker.StartPath(rb.position, destinoAleatorio, OnPathComplete);
            }

            yield return new WaitForSeconds(tiempoEntrePuntosPatrulla);
        }
    }
}
