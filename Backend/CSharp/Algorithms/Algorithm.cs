namespace Algorithms;

public enum Algorithm
{
    Brute_Force,
    Greedy,
    Branch_And_Bound,
    Dynamic,
    Genetic,
}


public class SettingsForGenetic
{
    public bool Fast { get; set; }
    public int MaxIterations { get; set; }
    public int PopulationSize { get; set; }
    public double MutationProbability { get; set; }
    public double EliteRate { get; set; }
    public int NumberOfCrossOverPoints { get; set; }
}

public class Settings
{
    public Algorithm? AlgorithmForKnapsack { get; set; } = null;
    public Algorithm? AlgorithmForTSP { get; set; } = null;
    public SettingsForGenetic? SettingsForGeneticKnapsack { get; set; } = null;
    public SettingsForGenetic? SettingsForGeneticTSP { get; set; } = null;
}