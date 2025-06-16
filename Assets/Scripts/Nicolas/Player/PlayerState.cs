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
    private bool iniciado = false;

    public static bool cargarDesdeCheckpoint = false;

    private void Start()
    {
        if (iniciado) return;
        iniciado = true;
        Debug.Log("Start PlayerState");

        hud = FindAnyObjectByType<HUDUpdate>();

        // Consumo seguro de la bandera
        bool usarCheckpoint = cargarDesdeCheckpoint;
        cargarDesdeCheckpoint = false;

        if (usarCheckpoint && PlayerPrefs.HasKey("vida") && PlayerPrefs.HasKey("pureza"))
        {
            vidaActual = PlayerPrefs.GetInt("vida");
            purezaActual = PlayerPrefs.GetInt("pureza");
            float posX = PlayerPrefs.GetFloat("posX");
            float posY = PlayerPrefs.GetFloat("posY");
            transform.position = new Vector3(posX, posY, transform.position.z);

            Debug.Log("Cargando datos desde CHECKPOINT");
        }
        else if (PlayerPrefs.HasKey("vida_temp") && PlayerPrefs.HasKey("pureza_temp"))
        {
            vidaActual = PlayerPrefs.GetInt("vida_temp");
            purezaActual = PlayerPrefs.GetInt("pureza_temp");
            float posX = PlayerPrefs.GetFloat("posX_temp");
            float posY = PlayerPrefs.GetFloat("posY_temp");
            transform.position = new Vector3(posX, posY, transform.position.z);

            Debug.Log("Cargando datos desde CAMBIO DE ESCENA");
        }
        else
        {
            vidaActual = vidaMaxima;
            purezaActual = 0;
            Debug.Log("No hay datos guardados, usando valores por defecto");
        }

        ActualizarHUD();
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
        if (primeraPureza) primeraPureza = false;
        if (primerAscensor && purezaActual >= 90) primerAscensor = false;

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

    public void GuardarCheckpoint()
    {
        PlayerPrefs.SetInt("vida", vidaActual);
        PlayerPrefs.SetInt("pureza", purezaActual);
        PlayerPrefs.SetFloat("posX", transform.position.x);
        PlayerPrefs.SetFloat("posY", transform.position.y);
        PlayerPrefs.SetString("sceneName", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
    }
}
