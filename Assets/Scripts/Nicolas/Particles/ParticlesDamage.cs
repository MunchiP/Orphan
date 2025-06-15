using UnityEngine;
using System.Collections.Generic;

public class ParticleDamage : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private List<ParticleSystem.Particle> enterParticles = new List<ParticleSystem.Particle>();
    private PlayerKnockback playerKnockbackScript;

    void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        if (_particleSystem == null) return;

        var main = _particleSystem.main;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        GameObject playerGO = GameObject.FindGameObjectWithTag("DetectorParticulas");
        if (playerGO == null) return;

        Collider2D colliderHijo = playerGO.GetComponent<CapsuleCollider2D>();
        if (colliderHijo == null) return;

        playerKnockbackScript = playerGO.GetComponentInParent<PlayerKnockback>();

        var trigger = _particleSystem.trigger;
        trigger.enabled = true;
        trigger.SetCollider(0, colliderHijo);
        trigger.enter = ParticleSystemOverlapAction.Callback;
        trigger.inside = ParticleSystemOverlapAction.Ignore;
        trigger.exit = ParticleSystemOverlapAction.Ignore;
    }

    void OnParticleTrigger()
    {
        int numEntered = _particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enterParticles);

        if (numEntered > 0 && playerKnockbackScript != null)
        {
            playerKnockbackScript.ApplyKnockback(transform.position);
        }
    }
}
