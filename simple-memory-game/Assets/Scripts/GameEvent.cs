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
        return color switch
        {
            Colors.RED => Color.red,
            Colors.GREEN => Color.green,
            Colors.BLUE => Color.blue,
            Colors.YELLOW => Color.yellow,
            _ => Color.white,// Default color if none match
        };
    }

    public override string ToString()
    {
        return $"GameEvent(Color: {color}, Quadrant: {quadrant})";
    }
}
