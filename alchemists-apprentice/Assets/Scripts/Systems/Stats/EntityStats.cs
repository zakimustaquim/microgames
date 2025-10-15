using UnityEngine;

[CreateAssetMenu(menuName = "EntityStats")]
public class EntityStats : LoggingScriptableObject
{
    public enum EntityStatType
    {
        STRENGTH,
        DEFENSE,
        SPEED,
        LUCK,
        HEALTH,
        MANA,
    }

    public int maxHealth;

    public int maxMana;

    public int strength;
    public int defense;
    public int speed;
    public int luck;

    public override string ToString()
    {
        return $"[EntityStats: maxHealth={maxHealth}, maxMana={maxMana}, strength={strength}, defense={defense}, speed={speed}, luck={luck}]";
    }
}
