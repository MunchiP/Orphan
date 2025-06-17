using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public PauseMenuNavigation navigation;

    public GameObject continueButton;
    public List<GameObject> otherButtons;

    void Start()
    {
        Debug.Log("[MenuController] Start ejecutado");

        Debug.Log($"ContinueButton isActiveBefore: {continueButton.activeSelf}");

        continueButton.SetActive(true);
        Debug.Log($"ContinueButton isActiveAfter: {continueButton.activeSelf}");

        CheckSavedGameAndSetupMenu();
    }

    void CheckSavedGameAndSetupMenu()
    {
        bool hasSavedGame = HasSavedGame();
        Debug.Log($"[MenuController] Has saved game? {hasSavedGame}");

        continueButton.SetActive(hasSavedGame);
        Debug.Log($"[MenuController] Continue button active? {continueButton.activeSelf}");

        UpdateButtonListAndSelection();
    }

    bool HasSavedGame()
    {
        bool exists = PlayerPrefs.HasKey("SavedGameExists") && PlayerPrefs.GetInt("SavedGameExists") == 1;
        Debug.Log($"[MenuController] Checking SavedGameExists flag: {exists}");
        return exists;
    }

    public void UpdateButtonListAndSelection()
    {
        navigation.buttonList.Clear();

        if (continueButton.activeSelf)
        {
            navigation.buttonList.Add(continueButton);
            Debug.Log("[MenuController] Continue button added to list");
        }

        foreach (var btn in otherButtons)
        {
            if (btn.activeSelf)
            {
                navigation.buttonList.Add(btn);
                Debug.Log($"[MenuController] Added button {btn.name} to list");
            }
        }

        navigation.RestartSelection(0);
        Debug.Log("[MenuController] RestartSelection called with index 0");
    }

    // Puedes llamar a esta función para refrescar manualmente el menú en runtime
    public void RefreshMenu()
    {
        CheckSavedGameAndSetupMenu();
    }
}
