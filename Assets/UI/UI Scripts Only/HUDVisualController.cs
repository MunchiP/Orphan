using UnityEngine;

public class HUDVisualController : MonoBehaviour
{
    public GameObject playerHUD;
    public PauseMenuAccess PauseMenuAccessScript;
    public InventoryAccess InventoryAccessScript;

    public void Update()
    {
        if (playerHUD != null)
        {
            if (PauseMenuAccessScript.isGameOnPauseMenu || InventoryAccessScript.isGameOnInventory)
            {
                playerHUD.SetActive(false);
            }
            else if(!PauseMenuAccessScript.isGameOnPauseMenu && !InventoryAccessScript.isGameOnInventory)
            {
                playerHUD.SetActive(true);
            }
        }
    }
}
