using System.Collections.Generic;
using UnityEngine;

public class MapManagerUI : MonoBehaviour
{
    public static MapManagerUI Instance;
    public GameObject mapaCanvas; // Mapa completo / aun por referenciar
    private Dictionary<string, MapZoneUI> zonas = new();
    private HashSet<string> zonasReveladas = new(); // La uso para guardar las zonas que ya fueron reveladas

    public Transform zonaContainer; // Para que encuentre las zonas


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        // Esta línea busca todos los MapZoneUI hijos del MapUIManager
        MapZoneUI[] zonasUI = zonaContainer.GetComponentsInChildren<MapZoneUI>(true);
        foreach (var zona in zonasUI)
        {
            zona.gameObject.SetActive(false); // Oculta todas al inicio
            zonas[zona.zoneID] = zona;
            Debug.Log("Zona registrada: " + zona.zoneID + " (" + zona.gameObject.name + ")");
        }
    }
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
             Debug.Log("Tab presionado");
            mapaCanvas.SetActive(!mapaCanvas.activeSelf);
        }
    }

    public void RevelarZona(string id)
    {
        if (!zonasReveladas.Contains(id) && zonas.ContainsKey(id))
            {
                zonas[id].Reveal();
                zonasReveladas.Add(id);
                Debug.Log($"Revelando zona con ID: {id}");
            }
            else if (!zonas.ContainsKey(id))
            {
                Debug.LogWarning($"Zona con ID '{id}' no encontrada.");
            }
    }

}
