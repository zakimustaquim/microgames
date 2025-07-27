using UnityEngine;
using UnityEngine.InputSystem;

public class BoyScript : MonoBehaviour
{
    // Logging
    private TaggedLogger logger;
    private void log(string message, int level = 2) => logger.Debug(message, level);

    // Movement speed
    [SerializeField] private float speed = 5f;

    void Awake()
    {
        logger = new TaggedLogger("BoyScript", this);
    }

    private void MoveBoy(Vector3 direction)
    {
        transform.position += speed * Time.deltaTime * direction;
        log("Set position to " + transform.position, 1);
    }

    void Update()
    {
        if (Keyboard.current.dKey.isPressed)
        {
            MoveBoy(Vector3.right);
            log("Moving right", 1);
        }
    }

}
