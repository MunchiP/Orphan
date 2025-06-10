using TMPro;
using UnityEngine;


// Implementaré un ScriptableObject para agrupar las características del diálogo


[CreateAssetMenu(fileName = "New Character", menuName = "Dialogue/Character")]
public class CharacterData : ScriptableObject

{

    //Relacion con el diccionario si es monolith o no
    [SerializeField] private bool isMonolith;

    public bool IsMonolith => isMonolith;


    [SerializeField] private string characterName;
    [SerializeField] private Sprite portrait;
    [SerializeField] private Color nameColor;
    [SerializeField] private TMP_FontAsset textFont;

    // Gets y Sets para acceder a ellos
    public string CharacterName {get { return characterName; }}
    public Sprite Portrait {get { return portrait; }}
    public Color NameColor {get { return nameColor; }}
    public TMP_FontAsset TextFont {get { return textFont; }}

}
