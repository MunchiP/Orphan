using UnityEngine;
using UnityEngine.SceneManagement;

public class DeactivateTitleNavigationDuringGameplay : MonoBehaviour
{

    public GameObject titleUINavigation;
    private VerticalOnlyNavigation titleUINavigationScript;

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
       
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            titleUINavigation.SetActive(false);
            titleUINavigationScript.enabled = false;
        }
    }
    void Start()
    {
        
        titleUINavigationScript = GetComponent<VerticalOnlyNavigation>();
    }

    
    void Update()
    {
        
    }
}
