using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDUpdate : MonoBehaviour
{
    public Image[] vidas;          // Iconos de vida en el HUD
    public Sprite spriteOn;        // Sprite cuando la vida está activa
    public Sprite spriteOff;       // Sprite cuando la vida está perdida
    public TMP_Text textoPureza;

    void Start()
    {
        // Si quieres puedes inicializar el HUD con valores predeterminados
    }

    public void ActualizarHUD(int vidaActual, int purezaActual)
    {
        textoPureza.text = purezaActual.ToString();

        // Cada 20 puntos es una vida visual (ej. 100 = 5, 60 = 3, etc.)
        int vidasActivas = Mathf.Clamp(vidaActual / 20, 0, vidas.Length);

        for (int i = 0; i < vidas.Length; i++)
        {
            vidas[i].sprite = (i < vidasActivas) ? spriteOn : spriteOff;
        }
    }

}
