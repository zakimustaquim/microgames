using System;
using System.Collections.Generic;
using UnityEngine;

public class PatternGenerator
{
    public int CurrLength { get; private set; }
    private readonly int numCells;

    public PatternGenerator(int numCells=4)
    {
        CurrLength = 2;
        this.numCells = numCells;
    }

    public IList<GameEvent> GenerateNext()
    {
        IList<GameEvent> pattern = new List<GameEvent>();
        System.Random random = new();

        for (int i = 0; i < CurrLength; i++)
        {
            pattern.Add(new GameEvent(
                (Colors)random.Next(0, Enum.GetValues(typeof(Colors)).Length),
                random.Next(1, numCells + 1)
            ));
        }

        CurrLength++;
        Debug.Log($"Generated pattern of length {CurrLength - 1}: {string.Join(", ", pattern)}");
        return pattern;
    }
}
