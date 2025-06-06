using UnityEngine;

public class CheckPointToSave : MonoBehaviour, IInteractable
{
    public PlayerState playerState;
    public int healAmount = 100;
    public GameObject menuGuardar;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerState = FindAnyObjectByType<PlayerState>();
    }

    public void Interact()
    {
        // Por defecto guardamos en slot 1 si se usa Interact sin especificar
        playerState.AgregarVida(100);
        menuGuardar.SetActive(true);
    }

    public void SaveInSlot1()
    {
        SaveInSlot(1);
    }

    public void SaveInSlot2()
    {
        SaveInSlot(2);
    }

    public void SaveInSlot3()
    {
        SaveInSlot(3);
    }

    private void SaveInSlot(int slotNumber)
    {
        SaveData data = new SaveData    
        {
            pureza = playerState.purezaActual,
            vida = healAmount,
            posX = player.position.x,
            posY = player.position.y
        };

        SaveSystem.Save(data, slotNumber);
        Debug.Log($"Guardado en slot {slotNumber}");
    }
}
