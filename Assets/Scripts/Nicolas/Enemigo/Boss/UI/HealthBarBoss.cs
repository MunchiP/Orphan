using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBarBoss : MonoBehaviour
{
    public Image barraVida;         // La barra principal (verde o roja)
    public Image barraRetardo;      // La barra amarilla (simula golpe)
    public float velocidadRetardo = 1.5f;

    private Coroutine corutinaRetardo;

    void Start()
    {
        if (barraVida != null)
            barraVida.fillAmount = 1f;

        if (barraRetardo != null)
            barraRetardo.fillAmount = 1f;
    }

    public void ActualizarBarra(float vida, float maxVida)
    {
        if (barraVida == null || barraRetardo == null) return;

        float porcentaje = vida / maxVida;

        // Actualiza la barra principal inmediatamente
        barraVida.fillAmount = porcentaje;

        if (corutinaRetardo != null)
            StopCoroutine(corutinaRetardo);

        // Solo hace retardo si la barra amarilla tiene que bajar
        if (barraRetardo.fillAmount > porcentaje)
        {
            corutinaRetardo = StartCoroutine(RetardoDaño(porcentaje));
        }
        else
        {
            // Si la barra amarilla tiene que subir, actualizar instantáneamente
            barraRetardo.fillAmount = porcentaje;
        }
    }


    IEnumerator RetardoDaño(float objetivo)
    {
        float actual = barraRetardo.fillAmount;

        float velocidadReal = velocidadRetardo / 10f; // 10 veces más lento

        // Si la barra amarilla está por debajo del objetivo (cura), subimos lentamente
        while (!Mathf.Approximately(actual, objetivo))
        {
            if (actual > objetivo)
            {
                actual -= Time.deltaTime * velocidadReal;
                actual = Mathf.Max(actual, objetivo);
            }
            else if (actual < objetivo)
            {
                actual += Time.deltaTime * velocidadReal;
                actual = Mathf.Min(actual, objetivo);
            }
            barraRetardo.fillAmount = actual;
            yield return null;
        }
    }
}
