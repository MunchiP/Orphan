using UnityEngine;

public class EnemyStateBossOne : MonoBehaviour
{
    public float vida = 100f;
    private float vidaMaxima;
    private bool destruido = false;
    public EnemyDead enemyDead;

    private HealthBarBoss healthBar; // <-- NUEVO

    void Start()
    {
        vidaMaxima = vida;
        enemyDead = GetComponent<EnemyDead>();

        // Busca la barra en la escena (puedes ajustar esto según tu jerarquía)
        healthBar = FindAnyObjectByType<HealthBarBoss>();

        if (healthBar != null)
            healthBar.ActualizarBarra(vida, vidaMaxima);
    }

    public void TomarDano(float cantidad)
    {
        if (destruido) return;

        vida -= cantidad;
        Debug.Log($"El enemigo recibió {cantidad} de daño. Vida restante: {vida}");

        if (healthBar != null)
            healthBar.ActualizarBarra(vida, vidaMaxima); // <-- ACTUALIZA

        if (vida <= 0)
        {
            vida = 0;
            destruido = true;
            OnDestruido();
        }
    }

    private void OnDestruido()
    {
        enemyDead.GenerarLoot();
        Destroy(this.gameObject);
        Debug.Log("Destruido");
    }
}
