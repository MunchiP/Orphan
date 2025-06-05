using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [Header("Vida")]
    public int vidaMaxima = 100;
    public int vidaActual;

    [Header("Pureza")]
    public int purezaActual;

    void Start()
    {
        vidaActual = vidaMaxima;
        purezaActual = 0;
    }

    // VIDA
    public void QuitarVida(int cantidad)
    {
        vidaActual -= cantidad;
        vidaActual = Mathf.Max(vidaActual, 0); // No bajar de 0
        Debug.Log("Vida actual: " + vidaActual);
    }

    public void AgregarVida(int cantidad)
    {
        vidaActual += cantidad;
        vidaActual = Mathf.Min(vidaActual, vidaMaxima); // No superar el máximo
        Debug.Log("Vida actual: " + vidaActual);
    }

    // PUREZA
    public void QuitarPureza(int cantidad)
    {
        purezaActual -= cantidad;
        purezaActual = Mathf.Max(purezaActual, 0); // No bajar de 0
        Debug.Log("Pureza actual: " + purezaActual);
    }

    public void AgregarPureza(int cantidad)
    {
        purezaActual += cantidad; // ✅ Ya no hay límite superior
        Debug.Log("Pureza actual: " + purezaActual);
    }
}
