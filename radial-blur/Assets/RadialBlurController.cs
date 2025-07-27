using UnityEngine;

public class RadialBlurController : MonoBehaviour 
{
    [Header("Radial Blur Settings")]
    [SerializeField] private Material radialBlurMaterial;
    [SerializeField] private Transform player;
    [SerializeField] private Camera playerCamera;
    
    [Header("Blur Configuration")]
    [SerializeField] private float maxBlurStrength = 1.0f;
    [SerializeField] private float blurRadius = 0.8f;
    [SerializeField] private float triggerDistance = 5.0f; // Distance player needs to move right before blur starts
    [SerializeField] private float maxDistance = 20.0f; // Distance at which blur reaches maximum
    
    [Header("Blur Center Options")]
    [SerializeField] private bool followPlayer = true; // Whether blur center should follow player
    [SerializeField] private float centerX = 0.5f; // Used only if followPlayer is false
    [SerializeField] private float centerY = 0.5f; // Used only if followPlayer is false
    
    [Header("Debug")]
    [SerializeField] private bool debugMode = false;
    [SerializeField] private float debugBlurStrength = 0.5f;
    
    // Private variables
    private float playerStartX;
    private TaggedLogger logger;
    
    // Shader property IDs for performance
    private static readonly int BlurStrengthProperty = Shader.PropertyToID("_BlurStrength");
    private static readonly int BlurRadiusProperty = Shader.PropertyToID("_BlurRadius");
    private static readonly int CenterXProperty = Shader.PropertyToID("_CenterX");
    private static readonly int CenterYProperty = Shader.PropertyToID("_CenterY");
    
    private void log(string message, int level = 2) => logger?.Debug(message, level);

    void Awake()
    {
        logger = new TaggedLogger("RadialBlurController", this);
        
        if (player == null)
        {
            log("Player reference not set, trying to find player with BoyScript component", 2);
            var boyScript = FindFirstObjectByType<BoyScript>();
            if (boyScript != null)
            {
                player = boyScript.transform;
                log("Found player: " + player.name, 2);
            }
            else
            {
                log("Could not find player with BoyScript component", 2);
            }
        }
        
        if (playerCamera == null)
        {
            log("Player camera not set, using main camera", 2);
            playerCamera = Camera.main;
            if (playerCamera == null)
            {
                log("Could not find main camera", 2);
            }
            else
            {
                log("Using main camera: " + playerCamera.name, 2);
            }
        }
        
        if (radialBlurMaterial == null)
        {
            log("Radial blur material not assigned", 2);
        }
    }

    void Start()
    {
        if (player != null)
        {
            playerStartX = player.position.x;
            log("Player start position X: " + playerStartX, 2);
        }
        
        // Initialize shader properties
        if (radialBlurMaterial != null)
        {
            radialBlurMaterial.SetFloat(BlurStrengthProperty, 0f);
            radialBlurMaterial.SetFloat(BlurRadiusProperty, blurRadius);
            
            // Set initial blur center
            if (followPlayer && player != null && playerCamera != null)
            {
                UpdateBlurCenter();
            }
            else
            {
                radialBlurMaterial.SetFloat(CenterXProperty, centerX);
                radialBlurMaterial.SetFloat(CenterYProperty, centerY);
            }
            
            log("Initialized radial blur material properties", 2);
        }
    }

    void Update()
    {
        if (player == null || radialBlurMaterial == null) return;
        
        // Debug mode - use fixed blur strength for testing
        if (debugMode)
        {
            radialBlurMaterial.SetFloat(BlurStrengthProperty, debugBlurStrength);
            
            // Update blur center if following player
            if (followPlayer && playerCamera != null)
            {
                UpdateBlurCenter();
            }
            
            log($"Debug mode: blur strength set to {debugBlurStrength:F2}", 1);
            return;
        }
        
        // Update blur center if following player
        if (followPlayer && playerCamera != null)
        {
            UpdateBlurCenter();
        }
        
        // Calculate how far the player has moved to the right
        float distanceMoved = player.position.x - playerStartX;
        
        // Only apply blur if player has moved past the trigger distance
        if (distanceMoved > triggerDistance)
        {
            // Calculate blur strength based on distance moved
            float adjustedDistance = distanceMoved - triggerDistance;
            float blurStrength = Mathf.Clamp01(adjustedDistance / (maxDistance - triggerDistance)) * maxBlurStrength;
            
            // Apply the blur strength to the material
            radialBlurMaterial.SetFloat(BlurStrengthProperty, blurStrength);
            
            log($"Player moved {distanceMoved:F2} units right, blur strength: {blurStrength:F2}", 1);
        }
        else
        {
            // No blur if player hasn't moved far enough
            radialBlurMaterial.SetFloat(BlurStrengthProperty, 0f);
        }
    }
    
    // Method to update blur center based on player's screen position
    private void UpdateBlurCenter()
    {
        if (player == null || playerCamera == null) return;
        
        // Convert player world position to viewport coordinates (0-1 range)
        Vector3 viewportPos = playerCamera.WorldToViewportPoint(player.position);
        
        // Clamp to ensure values stay within 0-1 range
        float screenCenterX = Mathf.Clamp01(viewportPos.x);
        float screenCenterY = Mathf.Clamp01(viewportPos.y);
        
        // Update material properties
        radialBlurMaterial.SetFloat(CenterXProperty, screenCenterX);
        radialBlurMaterial.SetFloat(CenterYProperty, screenCenterY);
        
        log($"Updated blur center to player screen position: ({screenCenterX:F2}, {screenCenterY:F2})", 1);
    }
    
    // Method to manually set blur strength (useful for testing)
    public void SetBlurStrength(float strength)
    {
        if (radialBlurMaterial != null)
        {
            radialBlurMaterial.SetFloat(BlurStrengthProperty, Mathf.Clamp01(strength));
            log("Manually set blur strength to: " + strength, 2);
        }
    }
    
    // Method to update blur center position (overrides followPlayer behavior)
    public void SetBlurCenter(float x, float y)
    {
        centerX = Mathf.Clamp01(x);
        centerY = Mathf.Clamp01(y);
        
        if (radialBlurMaterial != null)
        {
            radialBlurMaterial.SetFloat(CenterXProperty, centerX);
            radialBlurMaterial.SetFloat(CenterYProperty, centerY);
            log($"Manually set blur center to ({centerX:F2}, {centerY:F2})", 2);
        }
        
        // Temporarily disable followPlayer when manually setting center
        followPlayer = false;
        log("Disabled followPlayer due to manual blur center override", 2);
    }
    
    // Method to toggle whether blur center follows player
    public void SetFollowPlayer(bool follow)
    {
        followPlayer = follow;
        log($"Set followPlayer to: {follow}", 2);
        
        if (follow && player != null && playerCamera != null)
        {
            UpdateBlurCenter();
        }
    }
}
