using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManagerSave : MonoBehaviour
{
    public static GameManagerSave Instance;
    private FadeToBlack fadeToBlack;
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
        fadeToBlack = GetComponent<FadeToBlack>();
    }


    public void GuardarPartida(PlayerState player)
    {
        SaveData data = player.ObtenerDatosParaGuardar();

        PlayerPrefs.SetInt("vida", data.vida);
        PlayerPrefs.SetInt("pureza", data.pureza);
        PlayerPrefs.SetFloat("posX", data.posX);
        PlayerPrefs.SetFloat("posY", data.posY);
        PlayerPrefs.SetString("scene", data.sceneName);

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
            string escena = PlayerPrefs.GetString("scene");
            fadeToBlack.FadeToScene(escena);

            StartCoroutine(EsperarYCargarDatos());
        }
        else
        {
            Debug.LogWarning("No hay partida guardada");
        }
    }

    private IEnumerator EsperarYCargarDatos()
    {
        string escena = PlayerPrefs.GetString("scene");

        // Espera a que la escena se cargue
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == escena);

        // Busca el PlayerState y carga los datos guardados
        PlayerState player = FindAnyObjectByType<PlayerState>();
        if (player != null)
        {
            player.vidaActual = PlayerPrefs.GetInt("vida");
            player.purezaActual = PlayerPrefs.GetInt("pureza");
            float posX = PlayerPrefs.GetFloat("posX");
            float posY = PlayerPrefs.GetFloat("posY");
            player.transform.position = new Vector3(posX, posY, player.transform.position.z);

            Debug.Log("Datos de jugador cargados después de cambiar de escena");
        }
        else
        {
            Debug.LogWarning("No se encontró el PlayerState en la escena.");
        }
    }
}
