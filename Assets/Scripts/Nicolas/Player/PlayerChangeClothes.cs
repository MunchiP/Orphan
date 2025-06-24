using UnityEngine;
using System.Linq;

public class PlayerChangeClothes : MonoBehaviour
{
    [Header("Nombres de los objetos con SpriteRenderer")]
    private string cloth1Name = "brazoSalamandra1";
    private string cloth2Name = "brazoSalamandra2";
    private string cloth3Name = "cuerpoSalamandra";
    private string balanceo1Name = "Balanceo1";
    private string balanceo2Name = "Balanceo2";
    private string balanceo3Name = "Balanceo3";

    private SpriteRenderer clothSpriteRenderer;
    private SpriteRenderer clothSpriteRenderer2;
    private SpriteRenderer clothSpriteRenderer3;
    private SpriteRenderer balanceoRenderer;
    private SpriteRenderer balanceoRenderer2;
    private SpriteRenderer balanceoRenderer3;

    [Header("Sprites nuevos")]
    [SerializeField] private Sprite clothesChanges;
    [SerializeField] private Sprite clothesChanges2;
    [SerializeField] private Sprite clothesChanges3;
    [SerializeField] private Sprite balanceoChanges1;
    [SerializeField] private Sprite balanceoChanges2;
    [SerializeField] private Sprite balanceoChanges3;

    void Start()
    {
        ReconectarRenderers();
        if (PlayerPrefs.GetInt("clothes") == 1)
        {
            ApplyClothes();
        }
    }

    void OnEnable()
    {
        ReconectarRenderers();
        if (PlayerPrefs.GetInt("clothes") == 1)
        {
            ApplyClothes();
        }
    }

    public void Change()
    {
        PlayerPrefs.SetInt("clothes", 1);
        ApplyClothes();
    }

    private void ApplyClothes()
    {
        if (clothSpriteRenderer && clothesChanges) clothSpriteRenderer.sprite = clothesChanges;
        if (clothSpriteRenderer2 && clothesChanges2) clothSpriteRenderer2.sprite = clothesChanges2;
        if (clothSpriteRenderer3 && clothesChanges3) clothSpriteRenderer3.sprite = clothesChanges3;

        if (balanceoRenderer && balanceoChanges1) balanceoRenderer.sprite = balanceoChanges1;
        if (balanceoRenderer2 && balanceoChanges2) balanceoRenderer2.sprite = balanceoChanges2;
        if (balanceoRenderer3 && balanceoChanges3) balanceoRenderer3.sprite = balanceoChanges3;
    }

    private void ReconectarRenderers()
    {
        var allTransforms = FindObjectsByType<Transform>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        clothSpriteRenderer = FindOnly(cloth1Name, allTransforms);
        clothSpriteRenderer2 = FindOnly(cloth2Name, allTransforms);
        clothSpriteRenderer3 = FindOnly(cloth3Name, allTransforms); // SOLO cuerpoSalamandra

        balanceoRenderer = FindOnly(balanceo1Name, allTransforms);
        balanceoRenderer2 = FindOnly(balanceo2Name, allTransforms);
        balanceoRenderer3 = FindOnly(balanceo3Name, allTransforms);
    }

    private SpriteRenderer FindOnly(string name, Transform[] all)
    {
        var result = all.FirstOrDefault(t => t.name == name);
        return result != null ? result.GetComponent<SpriteRenderer>() : null;
    }
}
