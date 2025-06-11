using System.Collections.Generic;
using UnityEngine;



    [CreateAssetMenu(fileName = "HelperMonolithData", menuName = "Dialogue/Monolith Character")]

public class HelperMonolithData : ScriptableObject
{
    [System.Serializable]
    public class MonolithVisual
    {
        public string monolithName;
        public Sprite monolithImage;
    }

    public List<MonolithVisual> monolithVisual;

    public Sprite GetImageForMonolith(string name)
    {
        foreach (var item in monolithVisual)
        {
            if (item.monolithName == name)
            {
                return item.monolithImage;
            }
        }
        return null;
    }
}
