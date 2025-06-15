using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;

public class FadeToBlack : MonoBehaviour
{
    
    public Image fadeImageTitle;
    public Image fadeImageInGame;
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
        if (fadeImageTitle != null)
        {
            fadeImageTitle.color = new Color(0, 0, 0, 0);
            fadeImageTitle.enabled = false;
        }

        if (fadeImageInGame != null)
        {
            fadeImageInGame.color = new Color(0, 0, 0, 0);
            fadeImageInGame.enabled = false;
        }

        Debug.Log("[FadeToBlack] Reset both fade images after scene load.");
    }



    public void Start()
    {
        
            
    }
    public void FadeToScene(int sceneIndex, System.Action beforeSceneLoad = null)
    {
        Image imageToUse = null;

        if (SceneManager.GetActiveScene().buildIndex == 0)
            imageToUse = fadeImageTitle;
        else if (SceneManager.GetActiveScene().buildIndex == 1)
            imageToUse = fadeImageInGame;

        if (imageToUse != null)
        {
            StartCoroutine(FadeThenLoadScene(sceneIndex, imageToUse, beforeSceneLoad));
        }
        else
        {
            Debug.LogWarning("[FadeToBlack] No fade image assigned for current scene.");
        }
    }

    private IEnumerator FadeThenLoadScene(int sceneIndex, Image image, System.Action beforeSceneLoad)
    {
        image.enabled = true;
        float time = 0f;
        Color startColor = image.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(time / fadeDuration);
            image.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        image.color = targetColor;

        //  Run any custom logic BEFORE the scene loads
        beforeSceneLoad?.Invoke();

        SceneManager.LoadScene(sceneIndex);
    }
}
