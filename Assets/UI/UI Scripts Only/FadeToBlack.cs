using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeToBlack : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1.5f;

    private bool isTransitioning = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reasignar el fadeImage cada vez que se carga una nueva escena
        GameObject fadeObj = GameObject.FindGameObjectWithTag("FadeUI");
        if (fadeObj != null)
        {
            fadeImage = fadeObj.GetComponent<Image>();
            fadeImage.color = new Color(0f, 0f, 0f, 1f); // Asegura que empiece en negro
            StartCoroutine(FadeIn());
        }
        else
        {
            Debug.LogWarning("No se encontró FadeUI en la nueva escena.");
        }
    }

    public void FadeToSceneByIndex(int sceneIndex)
    {
        if (!isTransitioning)
        {
            StartCoroutine(FadeOutAndLoad(sceneIndex));
        }
    }

    private IEnumerator FadeOutAndLoad(int sceneIndex)
    {
        isTransitioning = true;

        if (fadeImage == null)
        {
            Debug.LogWarning("fadeImage no asignado.");
            SceneManager.LoadScene(sceneIndex);
            yield break;
        }

        fadeImage.gameObject.SetActive(true);
        fadeImage.enabled = true;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float alpha = Mathf.Clamp01(t / fadeDuration);
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneIndex);
    }

    private IEnumerator FadeIn()
    {
        if (fadeImage == null) yield break;

        fadeImage.enabled = true;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float alpha = 1f - Mathf.Clamp01(t / fadeDuration);
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        fadeImage.enabled = false;
        isTransitioning = false;
    }
}
