using UnityEngine;

[CreateAssetMenu(fileName = "New Mana Restore Effect", menuName = "Items/Effects/Mana Restore Effect")]
public class ManaRestoreEffect : ItemEffect
{
    public int manaAmount;

    public override void Apply(GameObject target)
    {
        // Implement mana restoration logic here
        if (target.TryGetComponent<Player>(out var player))
        {
            player.RestoreMana(manaAmount);
            log($"Restored {manaAmount} mana to {target.name}.");
        }
        else
        {
            warn($"No Player component found on {target.name}. Cannot apply mana restore effect.");
        }
    }
}
