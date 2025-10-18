using UnityEngine;

[CreateAssetMenu(fileName = "New Has Item Condition", menuName = "Dialogue/Conditions/Has Item", order = 1)]
public class HasItemCondition : LoggingScriptableObject, IDialogueCondition
{
    public Item item;
    public int quantity = 1;

    public bool IsMet()
    {
        // TODO: check if the player has the item in their inventory
        throw new System.NotImplementedException();
    }
}
