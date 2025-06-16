using UnityEngine;

public class EnemyState : MonoBehaviour
{
    public float vida = 100f;
    public float vidaMaxima;
    private bool destruido = false;
    public EnemyDead enemyDead;

    void Start()
    {
        vidaMaxima = vida;
        enemyDead = GetComponent<EnemyDead>();
    }

    public void TomarDano(float cantidad)
    {
        if (destruido) return;

        vida -= cantidad;
        Debug.Log($"El enemigo recibió {cantidad} de daño. Vida restante: {vida}");

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
        // Aquí puedes agregar animación de muerte, efectos, etc.
    }
}