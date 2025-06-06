using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string GetSavePath(int slot)
    {
        return Application.persistentDataPath + $"/save_slot{slot}.json";
    }

    public static void Save(SaveData data, int slot)
    {
        string path = GetSavePath(slot);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
        Debug.Log($"Datos guardados en slot {slot} en: {path}");
    }

    public static SaveData Load(int slot)
    {
        string path = GetSavePath(slot);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log($"Datos cargados del slot {slot}");
            return data;
        }
        else
        {
            Debug.LogWarning($"No se encontró archivo de guardado en slot {slot}.");
            return null;
        }
    }

    public static bool SlotTieneDatos(int slot)
    {
        return File.Exists(GetSavePath(slot));
    }

    public static void Eliminar(int slot)
{
    string path = GetSavePath(slot);
    if (File.Exists(path))
    {
        File.Delete(path);
        Debug.Log($"Archivo del slot {slot} eliminado.");
    }
    else
    {
        Debug.LogWarning($"No hay archivo en el slot {slot} para eliminar.");
    }
}

}
