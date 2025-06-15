using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using TMPro;

public class LocalizedDialogueText : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public LocalizedString localizedString;

    void OnEnable()
    {
        // Te suscribes al cambio de idioma
        localizedString.StringChanged += UpdateText;
        UpdateText(localizedString.GetLocalizedString());
    }

    void OnDisable()
    {
        // Te desuscribes al salir
        localizedString.StringChanged -= UpdateText;
    }

    void UpdateText(string value)
    {
        textMesh.text = value;
    }

    // Si quieres cambiar de texto dinámicamente
    public void SetKey(string tableName, string key)
    {
        localizedString.TableReference = tableName;
        localizedString.TableEntryReference = key;
        localizedString.RefreshString();
    }
}
