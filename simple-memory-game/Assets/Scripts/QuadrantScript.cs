using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuadrantScript : MonoBehaviour
{
    public Color currColor;

    public int quadrantIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetOpacity(0f); // Set initial opacity to 0
    }

    // Update is called once per frame
    void Update()
    {
        // Update the current color
        currColor = gameObject.GetComponent<Image>().color;
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
}
