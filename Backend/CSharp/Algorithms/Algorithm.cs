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

    public override string ToString()
    {
        return $"Fast: {Fast}, MaxIterations: {MaxIterations}, PopulationSize: {PopulationSize}, " +
               $"MutationProbability: {MutationProbability}, EliteRate: {EliteRate}, " +
               $"NumberOfCrossOverPoints: {NumberOfCrossOverPoints}";
    }
}

public class Settings
{
    public Algorithm? AlgorithmForKnapsack { get; set; } = null;
    public Algorithm? AlgorithmForTSP { get; set; } = null;
    public SettingsForGenetic? SettingsForGeneticKnapsack { get; set; } = null;
    public SettingsForGenetic? SettingsForGeneticTSP { get; set; } = null;

    public override string ToString()
    {
        string knapsackAlgorithm = AlgorithmForKnapsack?.ToString() ?? "None";
        string tspAlgorithm = AlgorithmForTSP?.ToString() ?? "None";
        string geneticKnapsackSettings = SettingsForGeneticKnapsack?.ToString() ?? "None";
        string geneticTSPSettings =  SettingsForGeneticTSP?.ToString() ?? "None";

        return $"AlgorithmForKnapsack: {knapsackAlgorithm}\n AlgorithmForTSP: {tspAlgorithm}\n " +
               $"SettingsForGeneticKnapsack: {geneticKnapsackSettings}\n SettingsForGeneticTSP: {geneticTSPSettings}";
    }
}
