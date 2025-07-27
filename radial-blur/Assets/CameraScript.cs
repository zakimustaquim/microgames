using UnityEngine;

// Script to have the camera follow the player
public class CameraScript : MonoBehaviour
{
    // Reference to the player object
    [SerializeField] private Transform player;

    // Logging
    private TaggedLogger logger;
    private void log(string message, int level = 2) => logger.Debug(message, level);

    void Awake()
    {
        logger = new TaggedLogger("CameraScript", this);
        log("player reference set to: " + player, 2);
    }

    void Update()
    {
        if (player != null)
        {
            // Update camera position to follow the player
            transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
            log("Camera position updated to follow player: " + transform.position, 1);
        }
        else
        {
            log("Player reference is null, cannot update camera position", 1);
        }
    }
}
