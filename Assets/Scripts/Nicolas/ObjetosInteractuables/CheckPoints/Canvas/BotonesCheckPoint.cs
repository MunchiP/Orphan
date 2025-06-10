using UnityEngine;

public class BotonesCheckPoint : MonoBehaviour
{
    public GameObject menuGuardar;
    public GameObject menuSlots;

    public void Volver()
    {
        if (menuGuardar.activeInHierarchy)
        {
            menuGuardar.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            menuSlots.SetActive(false);
            menuGuardar.SetActive(true);
        }

    }

    public void MenuSlots()
    {
        menuGuardar.SetActive(false);
        menuSlots.SetActive(true);
    }
    void Update()
    {
        if (menuGuardar.activeInHierarchy || menuSlots.activeInHierarchy)
        {
            Time.timeScale = 0f;
        }
    }
}
