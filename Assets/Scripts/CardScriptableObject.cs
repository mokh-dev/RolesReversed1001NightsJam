using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Scriptable Objects/Card")]

public class CardScriptableObject : ScriptableObject
{
    public string id;
    public int value;

    public Sprite frontSprite;
    public Sprite backSprite;
}
