using UnityEngine;

public class CamMovement : MonoBehaviour
{

    // Referenciando elementos a usar

    public Transform player;
    public float smoothSpeed;
    public Vector3 offset;

    void FixedUpdate()
    {
        Vector3 desirePos = player.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desirePos, smoothSpeed * Time.deltaTime);
        transform.position = smoothPosition;

        transform.position = smoothPosition;
    }
}
