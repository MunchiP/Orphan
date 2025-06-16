using UnityEngine;

public class RopeTiling : MonoBehaviour
{
    public Transform elevator;            // Referencia al ascensor
    public float scrollSpeed = 1f;        // Velocidad del scroll visual
    public SpriteRenderer ropeRenderer;   // SpriteRenderer con material que tenga WrapMode: Repeat

    private Material ropeMaterial;
    private float lastElevatorY;
    private float offsetY;

    void Start()
    {
        ropeMaterial = ropeRenderer.material;
        lastElevatorY = elevator.position.y;
        offsetY = 0f;
    }

    void Update()
    {
        float currentY = elevator.position.y;
        float deltaY = currentY - lastElevatorY;

        // Calcula dirección (sube o baja)
        offsetY += deltaY * scrollSpeed;

        // Aplica desplazamiento vertical al material
        ropeMaterial.SetTextureOffset("_BaseMap", new Vector2(0f, offsetY)); // Usa "_BaseMap" si estás en URP

        lastElevatorY = currentY;
    }
}