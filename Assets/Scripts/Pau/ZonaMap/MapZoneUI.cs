using UnityEngine;

public class MapZoneUI : MonoBehaviour
{

    // este script lo debe tener cada una de las zonas para que se activen
    // e inician desativados claramente...
    public string zoneID; // identificador unico para cada zona
    private bool reveled = false; // verificacion si la zona fue revelada o no

    public void Reveal()
    {
        if (!reveled)
        {
            gameObject.SetActive(true);
            reveled = true;
        }
    }

}
