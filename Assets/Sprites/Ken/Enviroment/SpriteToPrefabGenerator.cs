using UnityEngine;
using UnityEditor;
using System.IO;

public class SpriteToPrefabGenerator : MonoBehaviour
{
    [MenuItem("Tools/Generar Prefabs desde Atlas")]
    static void GeneratePrefabs()
    {
        string spritePath = "Assets/Sprites/Ken/Enviroment/CaveAtlas.png"; // Cambia esto con la ruta de tu atlas
        string outputPath = "Assets/Prefabs/Ken/Enviroment"; // Carpeta donde se guardarán los prefabs

        // Crea carpeta si no existe
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        Object[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(spritePath);

        foreach (Object obj in sprites)
        {
            if (obj is Sprite sprite)
            {
                GameObject go = new GameObject(sprite.name);
                SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
                sr.sprite = sprite;

                string prefabPath = outputPath + sprite.name + ".prefab";
                PrefabUtility.SaveAsPrefabAsset(go, prefabPath);
                GameObject.DestroyImmediate(go);
            }
        }

        Debug.Log("¡Prefabs generados exitosamente!");
    }
}
