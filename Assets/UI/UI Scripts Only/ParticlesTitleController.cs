using UnityEngine;
using UnityEngine.SceneManagement;

public class ParticlesTitleController : MonoBehaviour
{
    public GameObject particlesObject;
    private SymbolsTitleScreenSpawner symbolsCreatorScript;
    void Start()
    {
        symbolsCreatorScript = particlesObject.GetComponent<SymbolsTitleScreenSpawner>();
    }

    public void OnEnable()
    {
        
        SceneManager.sceneLoaded += OnSceneLoaded;

    }
    public void OnDisable()
    {
        
        SceneManager.sceneLoaded -= OnSceneLoaded;

    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            symbolsCreatorScript.DeactivateSymbols();
            symbolsCreatorScript.enabled = false;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 0)
        {

        }
    }
    void Update()
    {
        
    }
}
