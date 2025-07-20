using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuadrantScript : MonoBehaviour
{
    public Color currColor;

    public int quadrantIndex;
    public GameManager.GameUnit currGameUnit;
    private float timer = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetOpacity(0f); // Set initial opacity to 0
    }

    // Update is called once per frame
    void Update()
    {
        if (currGameUnit == null)
        {
            return; // If no current game unit, do nothing
        }

        // If animating, update the opacity over time - fade in and then out over 1 second
        if (currGameUnit.status == QuadrantStatus.CurrentlyAnimating)
        {
            timer += Time.deltaTime; // Increment timer by the time since last frame
            if (timer < 0.5f) // Fade in for the first half second
            {
                SetOpacity(timer * 2f); // Scale opacity from 0 to 1
            }
            else if (timer < 1.0f) // Fade out for the second half second
            {
                SetOpacity(1f - (timer - 0.5f) * 2f); // Scale opacity from 1 to 0
            }
            else if (timer >= 1.0f) // If the animation is complete
            {
                Debug.Log("Animation complete for quadrant " + quadrantIndex);
                currGameUnit.status = QuadrantStatus.InProgress; // Stop animating
                SetOpacity(0f); // Ensure opacity is set to 0 at the end
            }
        }
    }

    void SetOpacity(float opacity)
    {
        Color color = gameObject.GetComponent<Image>().color;
        color.a = opacity; // Set the opacity
        SetColor(color); // Apply the color with the new opacity
    }

    void SetColor(Color color)
    {
        gameObject.GetComponent<Image>().color = color; // Set the color
    }

    public void Animate(GameManager.GameUnit gameUnit)
    {
        currGameUnit = gameUnit; // Set the current game unit
        Debug.Log("Beginning animation for quadrant " + quadrantIndex);
        // Update the current color
        SetColor(currGameUnit.gameEvent.GetUnityColor()); 

        SetOpacity(0f); // Reset opacity to 0
        timer = 0.0f; // Reset timer
        currGameUnit.status = QuadrantStatus.CurrentlyAnimating;
    }
}
