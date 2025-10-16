using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable Item", menuName = "Items/Consumable Item")]
public class ConsumableItem : Item
{
    public List<ItemEffect> effects;

    public void OnConsume(GameObject consumer)
    {
        foreach (var effect in effects)
        {
            effect.Apply(consumer);
        }
    }
}
