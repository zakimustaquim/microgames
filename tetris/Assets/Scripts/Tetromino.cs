using System.Collections.Generic;
using UnityEngine;

public class Tetromino : LoggingMonoBehaviour
{
    public enum Type
    {
        I, J, L, O, S, T, Z
    }

    public static readonly IDictionary<Type, Color> TetrominoColors = new Dictionary<Type, Color>
    {
        { Type.I, Color.cyan },
        { Type.J, Color.blue },
        { Type.L, Color.yellow },
        { Type.O, Color.yellow },
        { Type.S, Color.green },
        { Type.T, new Color(0.5f, 0f, 0.5f) }, // Purple
        { Type.Z, Color.red }
    };

    [SerializeField] private Type tetrominoType;
    private Color color;

    protected override void Awake()
    {
        base.Awake();

        if (TetrominoColors.TryGetValue(tetrominoType, out color))
        {
            // Successfully retrieved color for the tetromino type
        }
        else
        {
            // Handle unknown tetromino type
            warn($"Unknown tetromino type: {tetrominoType}. Using default color.");
            color = Color.white; // Default color
        }
    }
}
