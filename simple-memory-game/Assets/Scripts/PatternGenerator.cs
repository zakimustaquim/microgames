using System;
using System.Collections.Generic;

public class PatternGenerator
{
    public int CurrLength { get; private set; }
    private readonly int numCells;

    public PatternGenerator(int numCells)
    {
        CurrLength = 2;
        this.numCells = numCells;
    }

    public IList<GameEvent> GenerateNext()
    {
        IList<GameEvent> pattern = new List<GameEvent>();
        Random random = new();

        for (int i = 0; i < CurrLength; i++)
        {
            pattern.Add(new GameEvent(
                (Colors)random.Next(0, Enum.GetValues(typeof(Colors)).Length),
                random.Next(0, numCells)
            ));
        }

        CurrLength++;
        return pattern;
    }
}
