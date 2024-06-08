﻿namespace Algorithms;

public static class Selection
{
    public static List<T> Roulette<T>(this List<(int Fitness, T Solution)> individuals, int elite)
    {
        var totalFitness = individuals.Aggregate(0, (x, y) =>
        {
            return x + y.Fitness;
        });

        var individualsWithScores = individuals
            .Select(x => (x.Solution, Counter: Convert.ToInt32(Math.Round(elite * ((decimal)x.Fitness / totalFitness), 0)))).ToList();
        return individualsWithScores.Aggregate(new List<T>(), (x, y) =>
        {
            for (int i = 0; i < y.Counter; i++)
            {
                x.Add(y.Solution);
            }
            return x;
        });
    }
}
