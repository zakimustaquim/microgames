using UnityEngine;

// Script to have the camera follow the player
public class CameraScript : MonoBehaviour
{
    // Reference to the player object
    [SerializeField] private Transform player;
    
    // Reference to the radial blur controller
    private RadialBlurController radialBlurController;

    // Logging
    private TaggedLogger logger;
    private void log(string message, int level = 2) => logger.Debug(message, level);

    void Awake()
    {
        logger = new TaggedLogger("CameraScript", this);
        log("player reference set to: " + player, 2);
        
        // Find the RadialBlurController component (should be on the same GameObject)
        radialBlurController = GetComponent<RadialBlurController>();
        if (radialBlurController == null)
        {
            log("RadialBlurController not found on the same GameObject, searching in scene", 2);
            radialBlurController = FindFirstObjectByType<RadialBlurController>();
            if (radialBlurController != null)
            {
                log("Found RadialBlurController in scene: " + radialBlurController.name, 2);
            }
            else
            {
                log("RadialBlurController not found in scene", 2);
            }
        }
        else
        {
            log("Found RadialBlurController on the same GameObject", 2);
        }
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
