using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemigoVoladorIA : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidadMovimiento = 4f;
    public float tiempoEntreActualizacionesPath = 0.5f;

    [Header("Distancias")]
    public float rangoDeteccionJugador = 8f;
    public float distanciaParaAtacar = 0.5f;

    private Transform jugador;
    private Rigidbody2D rb;
    private Seeker seeker;

    private Path path;
    private int currentWaypoint = 0;

    public bool persiguiendoJugador = false;
    public bool enCooldownPostAtaque = false;

    void Awake()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();

        if (jugador == null)
            Debug.LogError("No se encontró el jugador con tag 'Player'");

        if (seeker == null)
            Debug.LogError("No se encontró Seeker en el enemigo");

        if (rb == null)
            Debug.LogError("No se encontró Rigidbody2D en el enemigo");
    }

    void Start()
    {
        InvokeRepeating(nameof(ActualizarPath), 0f, tiempoEntreActualizacionesPath);
    }

    void ActualizarPath()
{
    if (jugador == null) return;

    if (!persiguiendoJugador)
    {
        float distanciaAlJugador = Vector2.Distance(transform.position, jugador.position);

        // Activar persecución sólo si está en rango de detección
        if (distanciaAlJugador <= rangoDeteccionJugador)
        {
            persiguiendoJugador = true;
            Debug.Log("Jugador detectado, persiguiendo globalmente.");
        }
        else
        {
            // No hacer nada si no está en rango y no persigue
            return;
        }
    }

    // Ya persiguiendo, siempre actualizar el path hacia el jugador sin importar la distancia
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
            return;

        Vector2 siguientePunto = (Vector2)path.vectorPath[currentWaypoint];
        Vector2 direccion = (siguientePunto - rb.position).normalized;

        float distanciaAlPunto = Vector2.Distance(rb.position, siguientePunto);

        if (distanciaAlPunto < 0.1f)
        {
            currentWaypoint++;
        }

        // Mueve usando MovePosition para no romper colisiones ni volar
        Vector2 nuevaPosicion = rb.position + direccion * velocidadMovimiento * Time.fixedDeltaTime;
        rb.MovePosition(nuevaPosicion);
    }

    // Llamar desde PlayerKnockbackTrigger cuando el enemigo golpea al jugador
    public void PausarTrasGolpear()
    {
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
        // La actualización de path se reanudará automáticamente en ActualizarPath()
    }
}
