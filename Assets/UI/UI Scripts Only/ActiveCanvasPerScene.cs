using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ActiveCanvasPerScene : MonoBehaviour
{
    public GameObject titleCanvas;
    public GameObject pauseCanvas;
    public GameObject particlesAndBackground;
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
        titleCanvas.SetActive(scene.buildIndex == 0);
        particlesAndBackground.SetActive(scene.buildIndex == 0);
        pauseCanvas.SetActive(scene.buildIndex == 1);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("currentScene is " + SceneManager.GetActiveScene().buildIndex);
    }
}
