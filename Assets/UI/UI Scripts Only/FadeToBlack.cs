using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;

public class FadeToBlack : MonoBehaviour
{
    public GameObject fadeImageObject;
    private Image imageFadeUse;
    public float fadeDuration = 2f;

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
        imageFadeUse = fadeImageObject.GetComponent<Image>();
        if (imageFadeUse != null)
        {
            imageFadeUse.enabled = false;
        }
        if (imageFadeUse == null)
        {
            Debug.Log("fadeImage not in current scene");
        }
            
    }

    public void Start()
    {
        
            
    }
    public void FadeToScene(int sceneIndex)
    {

        if (imageFadeUse == null && fadeImageObject != null)
        {
            imageFadeUse = fadeImageObject.GetComponent<Image>();
        }

        if (imageFadeUse != null)
        {
            StartCoroutine(FadeThenLoadScene(sceneIndex));
        }
        else
        {
            Debug.LogWarning("[FadeToBlack] imageFadeUse is null! Make sure fadeImageObject is assigned and has an Image component.");
        }
    }

    private IEnumerator FadeThenLoadScene(int sceneIndex)
    {
        imageFadeUse.enabled = true;

        float time = 0f;
        Color startColor = imageFadeUse.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

        Debug.Log("[FadeToBlack] Starting fade...");

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime; // use unscaledDeltaTime in case timeScale = 0
            float t = Mathf.Clamp01(time / fadeDuration);
            imageFadeUse.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        imageFadeUse.color = targetColor;
        Debug.Log("[FadeToBlack] Fade complete. Loading scene now!");

        SceneManager.LoadScene(sceneIndex);
    }
}
