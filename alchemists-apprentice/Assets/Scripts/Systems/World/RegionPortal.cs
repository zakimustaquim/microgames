using UnityEngine;

/// <summary>
/// A portal for GameObjects which contain a Collider that triggers
/// when the player enters it, moving them to another region.
/// It is tied to a particular RegionConnection. When entered,
/// it sends the RegionConnection to the WorldManager to handle the transition.
/// </summary>
[RequireComponent(typeof(Collider))]
public class RegionPortal : LoggingMonoBehaviour
{
    [SerializeField]
    private RegionConnection connection;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WorldManager.Instance.MoveToRegion(connection);
        }
    }
}
