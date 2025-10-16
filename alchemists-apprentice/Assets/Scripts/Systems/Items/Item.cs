using UnityEngine;

public abstract class Item : LoggingScriptableObject
{
    public string itemName;
    [TextArea] public string description;
    public Sprite icon;
    public int maxStack;
    public int sellPrice;
}
