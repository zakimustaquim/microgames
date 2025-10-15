using UnityEngine;

public class Player : LoggingMonoBehaviour
{
    [SerializeField]
    private EntityStats baseStats;

    private EntityStatsRuntime stats;

    protected override void Awake()
    {
        base.Awake();
        stats = new EntityStatsRuntime(baseStats);
        log($"Initialized player stats with {baseStats}", 4);
    }
}
