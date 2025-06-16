using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;

public class FadeToBlack : MonoBehaviour
{
    [SerializeField] Image fadeImage;
    public float fadeDuration = 4.0f;

    public void Awake()
    {
        
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
        fadeImage = GameObject.FindGameObjectWithTag("FadeUI")?.GetComponent<Image>();
        if (fadeImage != null)
        {
            fadeImage.enabled = false;
        }
        if (fadeImage == null)
        {
            Debug.Log("fadeImage not in current scene");
        }
            
    }

    public void Start()
    {
        
            
    }
    public void FadeToScene(string sceneName)
    {
        
        StartCoroutine(FadeThenLoadScene(sceneName));
    }

    private IEnumerator FadeThenLoadScene(string sceneName)
    {
        fadeImage.enabled = true;
        float time = 0;
        Color color = fadeImage.color;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float imageAlpha = Mathf.Clamp01(time / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, imageAlpha);
            yield return null;
        }

        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(sceneName);
    }
}
