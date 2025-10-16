using UnityEngine;

[CreateAssetMenu(fileName = "New Heal Effect", menuName = "Items/Effects/Heal Effect")]
public class HealEffect : ItemEffect
{
    public int healAmount;

    public override void Apply(GameObject target)
    {
        log("Applying HealEffect to " + target.name);

        // Implement healing logic here
        if (target.TryGetComponent<Player>(out var player))
        {
            player.Heal(healAmount);
            log($"Healed {target.name} for {healAmount} points.");
        }
        else
        {
            warn($"No Player component found on {target.name}. Cannot apply heal effect.");
        }
    }
}
