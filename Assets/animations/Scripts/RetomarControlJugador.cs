using UnityEngine;

public class RetomarControlJugador : MonoBehaviour
{
    PlayerController playerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
    }

    public void RetomarControl()
    {
        playerController.enabled = true;
    }
}
