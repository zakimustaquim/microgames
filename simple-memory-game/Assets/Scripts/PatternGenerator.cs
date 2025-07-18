using System;
using System.Collections.Generic;

public class PatternGenerator
{
    private int currLength;
    private readonly int numCells;

    public PatternGenerator(int numCells)
    {
        currLength = 3;
        this.numCells = numCells;
    }

    public IList<GameEvent> GenerateNext()
    {
        IList<GameEvent> pattern = new List<GameEvent>();
        Random random = new();

        for (int i = 0; i < currLength; i++)
        {
            pattern.Add(new GameEvent(
                (Colors)random.Next(0, Enum.GetValues(typeof(Colors)).Length),
                random.Next(0, numCells)
            ));
        }

        currLength++;
        return pattern;
    }
}
