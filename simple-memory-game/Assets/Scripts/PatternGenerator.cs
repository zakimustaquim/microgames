using System;
using System.Collections.Generic;

public class PatternGenerator
{
    private int currLength;

    public PatternGenerator()
    {
        currLength = 3;
    }

    public IList<Colors> GenerateNext()
    {
        IList<Colors> pattern = new List<Colors>();
        Random random = new();

        for (int i = 0; i < currLength; i++)
        {
            pattern.Add((Colors)random.Next(0, 4));
        }

        currLength++;
        return pattern;
    }
}
