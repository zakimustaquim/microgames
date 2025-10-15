using UnityEngine;

[System.Serializable]
public class EntityStatsRuntime : Logging
{
    public EntityStats baseStats;

    private int currHealth;
    private int currMana;

    private int strength;
    private int defense;
    private int speed;
    private int luck;

    public EntityStatsRuntime(EntityStats baseStats)
    {
        this.baseStats = baseStats;

        currHealth = baseStats.maxHealth;
        currMana = baseStats.maxMana;
        strength = baseStats.strength;
        defense = baseStats.defense;
        speed = baseStats.speed;
        luck = baseStats.luck;
    }

    public void TakeDamage(int damage)
    {
        int finalDamage = Mathf.Max(damage - defense, 0);
        currHealth -= finalDamage;
        currHealth = Mathf.Max(currHealth, 0);

        log($"Took ${damage} damage. New health is ${currHealth}", 3);
    }

    public void Heal(int healAmount)
    {
        currHealth = Mathf.Min(currHealth + healAmount, baseStats.maxHealth);

        log($"Healed ${healAmount} health. New health is ${currHealth}", 3);
    }

    public void IncrementStat(EntityStats.EntityStatType type, int amount)
    {
        switch (type)
        {
            case EntityStats.EntityStatType.STRENGTH:
                strength = Mathf.Max(strength + amount, 0);
                break;
            case EntityStats.EntityStatType.DEFENSE:
                defense = Mathf.Max(defense + amount, 0);
                break;
            case EntityStats.EntityStatType.SPEED:
                speed = Mathf.Max(speed + amount, 0);
                break;
            case EntityStats.EntityStatType.LUCK:
                luck = Mathf.Max(luck + amount, 0);
                break;
            case EntityStats.EntityStatType.HEALTH:
                currHealth = Mathf.Clamp(currHealth + amount, 0, baseStats.maxHealth);
                break;
            case EntityStats.EntityStatType.MANA:
                currMana = Mathf.Clamp(currMana + amount, 0, baseStats.maxMana);
                break;
        }
        log($"Incremented {type} by {amount}. New value is {GetStat(type)}", 3);
    }

    public int GetStat(EntityStats.EntityStatType type)
    {
        return type switch
        {
            EntityStats.EntityStatType.STRENGTH => strength,
            EntityStats.EntityStatType.DEFENSE => defense,
            EntityStats.EntityStatType.SPEED => speed,
            EntityStats.EntityStatType.LUCK => luck,
            EntityStats.EntityStatType.HEALTH => currHealth,
            EntityStats.EntityStatType.MANA => currMana,
            _ => throw new System.Exception($"Unknown stat type: {type}"),
        };
    }
}
