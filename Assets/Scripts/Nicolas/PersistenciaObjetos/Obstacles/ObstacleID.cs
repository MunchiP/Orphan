using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class ObstacleID : MonoBehaviour
{
    [SerializeField]
    public string obstacleID;

    [SerializeField] public string ObstacleIDValue => obstacleID;

    private void Awake()
    {
            string sceneName = gameObject.scene.name;
            string objectName = gameObject.name;
            Vector3 position = transform.position;
            // Generar ID legible: escena_nombre_posX_posY
            obstacleID = $"{sceneName}_{objectName}_{Mathf.RoundToInt(position.x)}_{Mathf.RoundToInt(position.y)}";
            // Marcar el objeto como modificado para que se guarde
            EditorUtility.SetDirty(this);
    }

    // private void Start()
    // {
    //     if (string.IsNullOrEmpty(obstacleID))
    //     {
    //         string sceneName = gameObject.scene.name;
    //         string objectName = gameObject.name;
    //         Vector3 position = transform.position;

    //         // Generar ID legible: escena_nombre_posX_posY
    //         obstacleID = $"{sceneName}_{objectName}_{Mathf.RoundToInt(position.x)}_{Mathf.RoundToInt(position.y)}";

    //         // Marcar el objeto como modificado para que se guarde
    //         EditorUtility.SetDirty(this);
    //     }
    // }
}
