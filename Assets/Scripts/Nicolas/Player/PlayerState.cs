using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [Header("Vida")]
    public int vidaMaxima = 100;
    public int vidaActual;

    [Header("Pureza")]
    public int purezaActual;

    private bool primeraPureza = true;
    private bool primerAscensor = true;

    private HUDUpdate hud;

    private void Start()
    {
        // Referencia al HUD (asumiendo que está en escena)
        hud = FindAnyObjectByType<HUDUpdate>();

        // Cargar datos si existen
        if (PlayerPrefs.HasKey("vida") && PlayerPrefs.HasKey("pureza"))
        {
            vidaActual = PlayerPrefs.GetInt("vida");
            purezaActual = PlayerPrefs.GetInt("pureza");
            float posX = PlayerPrefs.GetFloat("posX");
            float posY = PlayerPrefs.GetFloat("posY");
            transform.position = new Vector3(posX, posY, transform.position.z);

            Debug.Log("Datos de jugador cargados desde PlayerPrefs");

            ActualizarHUD();
        }
        else
        {
            vidaActual = vidaMaxima;
            purezaActual = 0;
            Debug.Log("No hay datos guardados, usando valores por defecto");
            ActualizarHUD();
        }
    }

    public void AgregarVida(int cantidad)
    {
        vidaActual += cantidad;
        vidaActual = Mathf.Min(vidaActual, vidaMaxima);
        Debug.Log("Vida actual: " + vidaActual);
        ActualizarHUD();
    }

    public void QuitarVida(int cantidad)
    {
        vidaActual -= cantidad;
        vidaActual = Mathf.Max(vidaActual, 0);
        Debug.Log("Vida actual: " + vidaActual);
        ActualizarHUD();
    }

    public void AgregarPureza(int cantidad)
    {
        if (primeraPureza)
        {
            primeraPureza = false;
        }
        if (primerAscensor && purezaActual >= 90)
        {
            primerAscensor = false;
        }

        purezaActual += cantidad;
        Debug.Log("Pureza actual: " + purezaActual);
        ActualizarHUD();
    }

    public void QuitarPureza(int cantidad)
    {
        purezaActual -= cantidad;
        purezaActual = Mathf.Max(purezaActual, 0);
        Debug.Log("Pureza actual: " + purezaActual);
        ActualizarHUD();
    }

    public void ActualizarHUD()
    {
        if (hud != null)
        {
            hud.ActualizarHUD(vidaActual, purezaActual);
        }
    }

    public SaveData ObtenerDatosParaGuardar()
    {
        SaveData data = new SaveData
        {
            vida = this.vidaActual,
            pureza = this.purezaActual,
            posX = transform.position.x,
            posY = transform.position.y,
            sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        };
        return data;
    }

}
