using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [Header("Vida")]
    public int vidaMaxima = 100;
    public int vidaActual;
    public bool primeraPureza = true;

    [Header("Pureza")]
    public int purezaActual;
    private ManagerTutorial managerTutorial;

    void Start()
    {
        managerTutorial = FindAnyObjectByType<ManagerTutorial>();
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
        if (primeraPureza && managerTutorial != null)
        {
            managerTutorial.PrimeraPureza();
            primeraPureza = false;
        }
        else
        {
            Debug.Log("no encontro manager tutorial script");
        }
        purezaActual += cantidad; // ✅ Ya no hay límite superior
        Debug.Log("Pureza actual: " + purezaActual);
    }

    public void CargarDatos(SaveData data)
    {
        purezaActual = data.pureza;
        vidaActual = data.vida;
        transform.position = new Vector3(data.posX, data.posY, transform.position.z);
        Debug.Log("Datos cargados y aplicados al jugador");
    }

    public SaveData ObtenerDatosParaGuardar()
    {
        return new SaveData
        {
            pureza = purezaActual,
            vida = vidaActual,
            posX = transform.position.x,
            posY = transform.position.y
        };
    }
}
