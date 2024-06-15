namespace Algorithms;

public class Mode2
{
    public static (int TotalDistance, int[] BestRoute, int TotalValue, int[] IncludedItems) Start(int[][] distances, int capacity, int[] weights, int[] values, Settings? settings = null)
    {
        int n = weights.Length;
        (int TotalValue, int[] IncludedItems) = Mode1.StartKnapsack(capacity, weights, values, settings?.AlgorithmForKnapsack, settings?.SettingsForGeneticKnapsack);

        var (newDistances, correctness) = TrimDistances(IncludedItems, distances);

        (int TotalDistace, int[] BestRoute) = Mode1.StartTSP(newDistances, settings?.AlgorithmForTSP, settings?.SettingsForGeneticTSP);
        int[] CorrectBestRoute = new int[BestRoute.Length];

        for (int i = 0; i < BestRoute.Length; i++)
        {
            CorrectBestRoute[i] = correctness[BestRoute[i]];
        }
        return (TotalDistace, CorrectBestRoute, TotalValue, IncludedItems);

        
    }
    public static (int[][] NewDistances, Dictionary<int, int>? Correctness) TrimDistances(int[] includedItems, int[][] distances)
    {
        Dictionary<int, int> Correctness = [];
        int[][] array = (int[][])distances.Clone();
        int counter = includedItems.Length - 1;

        for (int i = 0; i < includedItems.Length; i++)
        {
            Correctness[i] = includedItems[i];
        }

        for (int i = distances.Length - 1; i >= 0 ; i--)
        {
            if (counter >= 0 && includedItems[counter] == i)
            {
                counter--;
            }
            else
            {
                array = Erase(i, i, array);
            }
        }
        return (array, Correctness);

        static T[][] Erase<T>(int x, int y, T[][] matrix)
        {
            T[][] erasedMatrix = new T[matrix.Length - 1][];
            int counter = 0;

            for (int i = 0; i < matrix.Length; i++)
            {
                if (i == x) continue;
                if (counter >= erasedMatrix.Length) break;
                erasedMatrix[counter] = new T[matrix[i].Length - 1];
                int secondCounter = 0;
                for (int j = 0; j < matrix[counter].Length; j++)
                {
                    if (j == y) continue;
                    if (secondCounter >= erasedMatrix[counter].Length) break;
                    erasedMatrix[counter][secondCounter++] = matrix[i][j];
                }
                counter++;
            }
            return erasedMatrix;
        }
    }
}
