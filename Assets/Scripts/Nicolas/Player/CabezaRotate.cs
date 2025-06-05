using UnityEngine;

public class CabezaRotate : MonoBehaviour
{
    private GrappleLiana grappleLiana;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grappleLiana = GetComponentInParent<GrappleLiana>();
    }

    // Update is called once per frame
    void Update()
    {
        if (grappleLiana.isAttached)
        {
            Vector2 direction = (Vector2)grappleLiana.grapplePoint - (Vector2)transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(1f, 1f, angle-90f);
        }
        else
        {
            transform.rotation = grappleLiana.rotationSave;
        }
    }
}
