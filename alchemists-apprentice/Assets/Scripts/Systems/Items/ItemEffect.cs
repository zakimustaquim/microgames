using UnityEngine;

public abstract class ItemEffect : LoggingScriptableObject
{
    public abstract void Apply(GameObject target);
}
