using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines a region within the game world.
/// </summary>
[CreateAssetMenu(menuName = "World/Region")]
public class WorldRegion : LoggingScriptableObject
{
    public string regionName;
    public string sceneName;
    [TextArea] public string description;

    public List<RegionConnection> connections;
}
