using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeToBlack : MonoBehaviour
{
    public Image fadeImage;              // Asignar desde el Inspector
    public float fadeDuration = 2f;      // Duración del fade (igual para fade in y out)
    public float fadeInDelay = 1f;       // Tiempo que espera antes de iniciar el fade in

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // Mantener este objeto entre escenas
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (fadeImage != null)
        {
            fadeImage.color = new Color(0, 0, 0, 1f); // Comienza en negro
            fadeImage.enabled = true;
            StartCoroutine(FadeFromBlack());
        }

        Debug.Log("[FadeToBlack] Scene loaded: " + scene.name);
    }

    public void FadeToScene(int sceneIndex, System.Action beforeSceneLoad = null)
    {
        if (fadeImage != null)
        {
            StartCoroutine(FadeThenLoadScene(sceneIndex, beforeSceneLoad));
        }
        else
        {
            Debug.LogWarning("[FadeToBlack] No fade image assigned.");
        }
    }

    private IEnumerator FadeThenLoadScene(int sceneIndex, System.Action beforeSceneLoad)
    {
        fadeImage.enabled = true;
        float time = 0f;
        Color startColor = fadeImage.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f); // Fade a negro

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(time / fadeDuration);
            fadeImage.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        fadeImage.color = targetColor;

        beforeSceneLoad?.Invoke();

        SceneManager.LoadSceneAsync(sceneIndex);
    }

    private IEnumerator FadeFromBlack()
    {
        // Espera antes de empezar el fade in
        if (fadeInDelay > 0f)
        {
            yield return new WaitForSecondsRealtime(fadeInDelay);
        }

        float time = 0f;
        Color startColor = fadeImage.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f); // Fade a transparente

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(time / fadeDuration);
            fadeImage.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        fadeImage.color = targetColor;
        // No desactivamos la imagen, solo dejamos alpha 0
    }

    public void FadeToScene(string sceneName, System.Action beforeSceneLoad = null)
    {
        if (fadeImage != null)
        {
            StartCoroutine(FadeThenLoadScene(sceneName, beforeSceneLoad));
        }
        else
        {
            Debug.LogWarning("[FadeToBlack] No fade image assigned.");
        }
    }

    private IEnumerator FadeThenLoadScene(string sceneName, System.Action beforeSceneLoad)
    {
        fadeImage.enabled = true;
        float time = 0f;
        Color startColor = fadeImage.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f); // Fade a negro

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(time / fadeDuration);
            fadeImage.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        fadeImage.color = targetColor;

        beforeSceneLoad?.Invoke();

        SceneManager.LoadSceneAsync(sceneName);
    }
}
