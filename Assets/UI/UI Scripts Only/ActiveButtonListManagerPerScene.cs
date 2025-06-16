using UnityEngine;
using UnityEngine.SceneManagement;

public class ActiveButtonListManagerPerScene : MonoBehaviour
{
    
    public TitleButtonListManager titleButtonListScript;
    public ButtonListManager menuButtonListScript;

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
        titleButtonListScript.enabled = scene.buildIndex == 0;
        menuButtonListScript.enabled = scene.buildIndex == 1;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
