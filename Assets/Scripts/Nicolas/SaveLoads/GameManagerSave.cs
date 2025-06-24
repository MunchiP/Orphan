using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManagerSave : MonoBehaviour
{
    public static GameManagerSave Instance;
    private ButtonListManager fadeToBlack;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        fadeToBlack = GetComponent<ButtonListManager>();
    }

    public void GuardarPartida(PlayerState player)
    {
        SaveData data = player.ObtenerDatosParaGuardar();

        PlayerPrefs.SetInt("vida", data.vida);
        PlayerPrefs.SetInt("pureza", data.pureza);
        PlayerPrefs.SetFloat("posX", data.posX);
        PlayerPrefs.SetFloat("posY", data.posY);
        PlayerPrefs.SetString("scene", data.sceneName);
        PlayerPrefs.SetInt("clothes", data.clothes);

        // Flag para indicar que hay partida guardada
        PlayerPrefs.SetInt("SavedGameExists", 1);
        Debug.Log("Partida guardada y flag SavedGameExists seteado . " + PlayerPrefs.GetInt("SavedGameExists"));

        PlayerPrefs.Save();

        Debug.Log("Partida guardada");
    }

    public void CargarPartida()
    {
        if (PlayerPrefs.HasKey("scene"))
        {
            if (Time.timeScale < 1)
            {
                Time.timeScale = 1f;
            }

            string escena = PlayerPrefs.GetString("scene");

            // Poner la bandera para que PlayerState cargue datos desde checkpoint al iniciar la escena
            PlayerState.cargarDesdeCheckpoint = true;

            // Cambiar escena por nombre
            fadeToBlack.ChangeSceneById(escena);

            StartCoroutine(EsperarYCargarDatos());
        }
        else
        {
            Debug.LogWarning("No hay partida guardada. Cargando escena 0...");

            if (Time.timeScale < 1)
            {
                Time.timeScale = 1f;
            }

            PlayerState.cargarDesdeCheckpoint = false; // No hay checkpoint que cargar

            // Cambiar a escena 0 directamente
            fadeToBlack.ChangeSceneByIndex(1);
        }
    }

    private IEnumerator EsperarYCargarDatos()
    {
        string escena = PlayerPrefs.GetString("scene");

        // Espera a que la escena se cargue
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == escena);

        // Busca el PlayerState y (opcionalmente) actualiza los datos - aunque PlayerState debería cargarlo solo por la bandera
        PlayerState player = FindAnyObjectByType<PlayerState>();
        if (player != null)
        {
            // Opcional: puedes omitir esta parte porque PlayerState lo carga en Start()
            player.vidaActual = PlayerPrefs.GetInt("vida");
            player.purezaActual = PlayerPrefs.GetInt("pureza");
            float posX = PlayerPrefs.GetFloat("posX");
            float posY = PlayerPrefs.GetFloat("posY");
            player.transform.position = new Vector3(posX, posY, player.transform.position.z);

            player.ActualizarHUD();

            Debug.Log("Datos de jugador cargados después de cambiar de escena");
        }
        else
        {
            Debug.LogWarning("No se encontró el PlayerState en la escena.");
        }
    }
}
