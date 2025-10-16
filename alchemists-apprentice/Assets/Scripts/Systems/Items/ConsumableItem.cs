using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable Item", menuName = "Items/Consumable Item")]
public class ConsumableItem : Item
{
    public List<ItemEffect> effects;

    public void OnConsume(GameObject consumer)
    {
        log("Consuming item: " + itemName);
        foreach (var effect in effects)
        {
            effect.Apply(consumer);
        }
        log("Finished consuming item: " + itemName);
    }
}
