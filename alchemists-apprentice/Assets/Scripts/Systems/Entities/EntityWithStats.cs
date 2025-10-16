using UnityEngine;

public class EntityWithStats : LoggingMonoBehaviour
{
    [SerializeField]
    private EntityStats baseStats;

    protected EntityStatsRuntime stats;

    protected override void Awake()
    {
        base.Awake();
        stats = new EntityStatsRuntime(baseStats);
        log($"Initialized entity stats with {baseStats}");
    }

    public void Heal(int amount)
    {
        if (stats == null)
        {
            warn("Cannot heal - stats not initialized!");
            return;
        }

        stats.Heal(amount);
    }

    public void RestoreMana (int amount)
    {
        if (stats == null)
        {
            warn("Cannot restore mana - stats not initialized!");
            return;
        }
        stats.RestoreMana(amount);
    }
}
