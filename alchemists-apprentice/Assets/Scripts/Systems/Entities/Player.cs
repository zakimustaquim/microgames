using UnityEngine;

public class Player : EntityWithStats
{
    public static Player Instance { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            warn("Multiple instances of Player detected! Destroying duplicate.");
            return;
        }

        Instance = this;
    }

    public void UseItem(ConsumableItem item)
    {
        if (item != null)
        {
            item.OnConsume(gameObject);
            log($"Player used item: {item.itemName}");
        }
        else
        {
            warn("Attempted to use a null item.");
        }
    }
}
