using System;
using System.Collections.Generic;

public class PatternGenerator
{
    private int currLength;

    public PatternGenerator()
    {
        currLength = 3;
    }

    public IList<GameEvent> GenerateNext()
    {
        IList<GameEvent> pattern = new List<GameEvent>();
        Random random = new();

        for (int i = 0; i < currLength; i++)
        {
            pattern.Add(new GameEvent((Colors)random.Next(0, 4), random.Next(0, 4)));
        }

        currLength++;
        return pattern;
    }
}
