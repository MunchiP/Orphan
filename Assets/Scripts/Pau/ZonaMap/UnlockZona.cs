using UnityEngine;

public class UnlockZona : MonoBehaviour
{
    public string zoneID;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            MapManagerUI.Instance.RevelarZona(zoneID);
        }
    }
}
