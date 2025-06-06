using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    private PlayerState playerState;
    private int totalSlots = 3;

    private void Start()
    {
        playerState = FindAnyObjectByType<PlayerState>();
        if (playerState == null)
            Debug.LogError("No se encontró PlayerState en la escena");

        MostrarEstadoDeSlots();
    }

    public void GuardarEnSlot(int slot)
    {
        if (playerState == null) return;

        SaveData data = playerState.ObtenerDatosParaGuardar();
        SaveSystem.Save(data, slot);
        MostrarEstadoDeSlots();
    }

    public void CargarDesdeSlot(int slot)
    {
        if (playerState == null) return;

        SaveData data = SaveSystem.Load(slot);
        if (data != null)
        {
            playerState.CargarDatos(data);
        }
    }

    public void EliminarSlot(int slot)
    {
        SaveSystem.Eliminar(slot);
        Debug.Log($"🗑️ Slot {slot} eliminado.");
        MostrarEstadoDeSlots();
    }

    private void MostrarEstadoDeSlots()
    {
        for (int i = 1; i <= totalSlots; i++)
        {
            if (SaveSystem.SlotTieneDatos(i))
                Debug.Log($"Slot {i}: ✅ Datos guardados.");
            else
                Debug.Log($"Slot {i}: 🟥 Vacío.");
        }
    }
}
