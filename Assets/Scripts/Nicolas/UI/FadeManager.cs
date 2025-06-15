using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance { get; private set; }

    [Header("Fade Config")]
    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private float fadeOutDuration = 1f;
    [SerializeField] private Color fadeColor = Color.black;

    private Canvas fadeCanvas;
    private Image fadePanel;
    private bool isFading = false;
    private int sceneToLoad = -1;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SetupFadeCanvas();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void SetupFadeCanvas()
    {
        GameObject canvasGO = new GameObject("FadeCanvas");
        fadeCanvas = canvasGO.AddComponent<Canvas>();
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();
        fadeCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        fadeCanvas.sortingOrder = 999;

        canvasGO.transform.SetParent(transform);
        canvasGO.transform.localPosition = Vector3.zero;

        GameObject panelGO = new GameObject("FadePanel");
        fadePanel = panelGO.AddComponent<Image>();
        fadePanel.raycastTarget = false;

        RectTransform rect = panelGO.GetComponent<RectTransform>();
        rect.SetParent(fadeCanvas.transform);
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        fadePanel.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0f);
        fadePanel.gameObject.SetActive(false);
    }

    public void LoadSceneByIndex(int index)
    {
        if (!isFading)
        {
            sceneToLoad = index;
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeOut()
    {
        isFading = true;
        fadePanel.gameObject.SetActive(true);
        float timer = 0f;

        while (timer < fadeOutDuration)
        {
            timer += Time.unscaledDeltaTime;
            float alpha = Mathf.Clamp01(timer / fadeOutDuration);
            fadePanel.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha);
            yield return null;
        }

        fadePanel.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 1f);

        // Ahora sí carga la escena
        SceneManager.LoadScene(sceneToLoad);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float timer = 0f;

        while (timer < fadeInDuration)
        {
            timer += Time.unscaledDeltaTime;
            float alpha = Mathf.Clamp01(1f - (timer / fadeInDuration));
            fadePanel.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha);
            yield return null;
        }

        fadePanel.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0f);
        fadePanel.gameObject.SetActive(false);
        isFading = false;
    }

    public void LoadNextScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex + 1;
        if (index >= SceneManager.sceneCountInBuildSettings)
            index = 0;
        LoadSceneByIndex(index);
    }
}
