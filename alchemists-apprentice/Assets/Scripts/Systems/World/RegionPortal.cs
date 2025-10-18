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
            log("Player collided with a GameObject with a RegionPortal.", 1);
            
            if (connection == null)
            {
                warn("No RegionConnection assigned to this RegionPortal. Cannot move to new region.");
                return;
            }

            log("Sending RegionConnection to WorldManager to move to new region.", 1);
            WorldManager.Instance.MoveToRegion(connection);
        }
    }
}
