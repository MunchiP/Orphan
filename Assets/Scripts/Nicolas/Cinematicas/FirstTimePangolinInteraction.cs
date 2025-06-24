using UnityEngine;
using System.Collections;

public class FirstTimePangolinInteractrion : MonoBehaviour
{
    public GameObject sceneDesactivate;
    private const string FirstTimeKey = "PrimeraVez_EscenaPangolin";
    public GameObject fadeObject;
    private PlayerController playerController;

    void Start()
    {
        PlayerPrefs.DeleteAll();
        bool esPrimeraVez = PlayerPrefs.GetInt(FirstTimeKey, 1) == 1;
        playerController = sceneDesactivate.GetComponent<PlayerController>();
        playerController.enabled = false;
        if (esPrimeraVez)
        {
            // Es la primera vez: escena desactivada, este activo
            sceneDesactivate.SetActive(false);
            gameObject.SetActive(true);
            this.gameObject.SetActive(true);
            this.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            // No es la primera vez: todo desactivado excepto la escena
            gameObject.SetActive(false);
            sceneDesactivate.SetActive(true);
            this.gameObject.SetActive(false);
            this.transform.parent.gameObject.SetActive(false);
        }
    }

    public void ActivarEscena()
    {
        sceneDesactivate.SetActive(true);
        PlayerPrefs.SetInt(FirstTimeKey, 0);
        PlayerPrefs.Save();

        StartCoroutine(DesactivarAmbosDespuésDeDelay());
    }

    private IEnumerator DesactivarAmbosDespuésDeDelay()
    {
        yield return new WaitForSeconds(2f);
        playerController.enabled = true;
        gameObject.SetActive(false);
        this.transform.parent.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
