using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class HUDUpdate : MonoBehaviour
{
    public Image[] vidas;          // Iconos de vida en el HUD
    public Sprite spriteOn;        // Sprite cuando la vida está activa
    public Sprite spriteOff;       // Sprite cuando la vida está perdida
    public TMP_Text textoPureza;

    private PlayerState playerState;

    void Start()
    {
        vidas = GetComponentsInChildren<Image>();
        playerState = FindAnyObjectByType<PlayerState>();
        textoPureza.text = playerState.purezaActual.ToString();

        for (int i = 0; i < vidas.Length; i++)
        {
            vidas[i].sprite = spriteOn;
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            int vida = playerState.vidaActual;
            textoPureza.text = playerState.purezaActual.ToString();

            // Cada 20 puntos es una vida visual (ej. 100 = 5, 60 = 3, etc.)
            int vidasActivas = Mathf.Clamp(vida / 20, 0, vidas.Length);

            for (int i = 0; i < vidas.Length; i++)
            {
                if (i < vidasActivas)
                {
                    vidas[i].sprite = spriteOn;
                }
                else
                {
                    vidas[i].sprite = spriteOff;
                }
            }
        }
    }
}
