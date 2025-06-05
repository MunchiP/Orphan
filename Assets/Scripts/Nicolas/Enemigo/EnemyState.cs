using UnityEngine;

public class EnemyState : MonoBehaviour
{
    public float vida = 100f;
    private bool destruido = false;

    void Update()
    {
        // Puedes probar aquí con una tecla si quieres: solo para testeo
        // if (Input.GetKeyDown(KeyCode.K)) TomarDanio(25);
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
        Debug.Log("Destruido");
        // Aquí puedes agregar animación de muerte, efectos, etc.
    }
}