using UnityEngine;

public class GameEvent
{
    public Colors color;
    public int quadrant;

    public GameEvent(Colors color, int quadrant)
    {
        this.color = color;
        this.quadrant = quadrant;
    }

    public Color GetUnityColor()
    {
        switch (color)
        {
            case Colors.RED:
                return Color.red;
            case Colors.GREEN:
                return Color.green;
            case Colors.BLUE:
                return Color.blue;
            case Colors.YELLOW:
                return Color.yellow;
            default:
                return Color.white; // Default color if none match
        }
    }
}
