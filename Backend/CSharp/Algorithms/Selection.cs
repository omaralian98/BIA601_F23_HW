namespace Algorithms;

public static class Selection
{
    public static List<T> Roulette<T>(this List<(int Fitness, T Solution)> individuals, int elite, bool lowestIsBetter = false)
    {
        if (lowestIsBetter)
            return individuals.Take(elite).Select(x => x.Solution).ToList();

        var totalFitness = individuals.Aggregate(0, (x, y) => {
            return x + y.Fitness;
        });

        if (totalFitness == 0) totalFitness = 1;

        var individualsWithScores = individuals.Select(x => (x.Solution, Counter: Convert.ToInt32(Math.Round(elite * ((decimal)x.Fitness / totalFitness), 0)))).ToList();
        return individualsWithScores.Aggregate(new List<T>(), (x, y) => {
            for (int i = 0; i < y.Counter; i++)
                x.Add(y.Solution);
            return x;
        });
    }

    //public static List<T> Roulette<T>(this List<(int Fitness, T Solution)> individuals, int elite, bool lowestIsBetter = false)
    //{
    //    //if (lowestIsBetter)
    //    //{
    //    //    return individuals.Take(elite).Select(x => x.Solution).ToList();
    //    //}
    //    var totalFitness = individuals.Aggregate(0, (x, y) =>
    //    {
    //        return x + y.Fitness;
    //    });

    //    var individualsWithScores = individuals
    //        .Select(x => (x.Solution, Counter: Convert.ToInt32(Math.Round(elite * ((decimal)totalFitness / x.Fitness), 0)))).ToList();

    //    var test = individualsWithScores.Aggregate(new List<T>(), (x, y) =>
    //    {
    //        for (int i = 0; i < y.Counter; i++)
    //        {
    //            x.Add(y.Solution);
    //        }
    //        return x;
    //    });
    //    Console.WriteLine("Elite: {0}", elite);
    //    Console.WriteLine(test.Count);
    //    Console.WriteLine();
    //    Console.ReadLine();
    //    return test;
    //}
}
