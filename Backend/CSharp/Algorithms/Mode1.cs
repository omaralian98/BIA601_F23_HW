namespace Algorithms;

public class Mode1
{
    public static (int TotalDistance, int[] BestRoute, int TotalValue, string Solution) Start(int[][] distances, int capacity, int[] weights, int[] values)
    {


        (int TotalValue, string Solution) = Knapsack(capacity, weights, values);
        (int TotalDistace, int[] BestRoute) = StartTSP(distances);


        return (TotalDistace, BestRoute, TotalValue, Solution);
    }

    internal static (int TotalDistance, int[] BestRoute) StartTSP(int[][] distances)
    {
        int n = distances.Length;
        int TotalDistace = 0;
        int[] BestRoute;
        if (n <= 17)
        {
            (TotalDistace, BestRoute) = TSP(distances);
        }
        else
        {
            (TotalDistace, BestRoute) = TSP(distances);
        }
        return (TotalDistace, BestRoute);
    }

    internal static (int TotalValue, string Solution) Knapsack(int capacity, int[] weights, int[] values)
    {
        int n = weights.Length;
        int[,] memo = new int[n + 1, capacity + 1];

        for (int i = 0; i <= n; i++)
        {
            for (int j = 0; j <= capacity; j++)
            {
                if (i == 0 || j == 0)
                {
                    memo[i, j] = 0;
                }
                else
                {
                    memo[i, j] = memo[i - 1, j];

                    if (weights[i - 1] <= j)
                    {
                        memo[i, j] = Math.Max(memo[i, j], values[i - 1] + memo[i - 1, j - weights[i - 1]]);
                    }
                }
            }
        }

        // Backtracking to find the items included in the optimal solution
        int maxValue = memo[n, capacity];
        int[] itemsIncluded = new int[n];
        int remainingCapacity = capacity;

        for (int i = n; i > 0 && maxValue > 0; i--)
        {
            if (maxValue != memo[i - 1, remainingCapacity])
            {
                itemsIncluded[i - 1] = 1; // item i-1 is included
                maxValue -= values[i - 1];
                remainingCapacity -= weights[i - 1];
            }
        }

        return (memo[n, capacity], string.Join("", itemsIncluded));
    }

    internal static (int TotalDistance, int[] BestRoute) TSP(int[][] distances)
    {
        int n = distances.Length;
        var memo = new Dictionary<string, int>();
        var parent = new Dictionary<string, int>();
        var path = new List<int>();

        bool IsOver(int visited, int n) => visited == (1 << n) - 1;
        bool IsVisited(int visited, int node) => (visited & (1 << node)) == 0;
        int AddNode(int visited, int node) => visited | (1 << node);

        int TSPFinder(int currentNode, int visited)
        {
            string key = $"{currentNode}-{visited}";

            if (IsOver(visited, n))
            {
                return distances[currentNode][0];
            }

            if (memo.TryGetValue(key, out int value))
            {
                return value;
            }

            int minDistance = int.MaxValue;
            int bestNextNode = -1;

            for (int nextNode = 0; nextNode < n; nextNode++)
            {
                if (IsVisited(visited, nextNode))
                {
                    int distance = distances[currentNode][nextNode] + TSPFinder(nextNode, AddNode(visited, nextNode));
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        bestNextNode = nextNode;
                    }
                }
            }

            memo[key] = minDistance;
            parent[key] = bestNextNode;

            return minDistance;
        }

        void ReconstructPath()
        {
            int current = 0;
            int visited = 1;
            while (!IsOver(visited, n))
            {
                path.Add(current);
                string key = $"{current}-{visited}";
                current = parent[key];
                visited = AddNode(visited, current);
            }
            path.Add(current);
            path.Add(0); // Return to the starting city
        }

        int minDistanceResult = TSPFinder(0, 1);
        ReconstructPath();

        return (minDistanceResult, [.. path]);
    }
}


public class Mode2
{
    public static (int TotalDistance, int[] BestRoute, int TotalValue, string Solution) Start(int[][] distances, int capacity, int[] weights, int[] values)
    {
        (int TotalValue, string Solution) = Mode1.Knapsack(capacity, weights, values);

        Dictionary<int, int> test = [];
        var newDistances = TrimDistances();

        (int TotalDistace, int[] BestRoute) = Mode1.StartTSP(newDistances);
        int[] CorrecttBestRoute = new int[BestRoute.Length];
        for (int i = 0; i < BestRoute.Length; i++)
        {
            CorrecttBestRoute[i] = test[BestRoute[i]];
        }
        return (TotalDistace, CorrecttBestRoute, TotalValue, Solution);


        int[][] TrimDistances()
        {
            Array? array = null;
            int counter = 0;
            for (int i = 0; i < Solution.Length; i++)
            {
                if (Solution[i] == '0')
                {
                    array = Erase(i, i, distances);
                }
                else
                {
                    test[counter++] = i;
                }
            }
            return array is null ? distances: (int[][])array;

            static T[][] Erase<T>(int x, int y, T[][] matrix)
            {
                int size = matrix.GetLength(0) - 1; //Get the new size.
                int row = 0;
                int column = 0;
                int counter = 0;
                T[][] erasedMatrix = new T[size][]; //Create the new array.
                for (int i = 0; i < matrix.Length; i++)
                { //Loop through the original array.
                    if (i == x) goto skip;
                    erasedMatrix[counter++] = new T[size];
                    for (int j = 0; j < matrix[i].Length; j++)
                    { //If we are the at the same row that we want to erase goto the next row.
                        if (j == y) continue; //If we are at the same column that we want to erase then go to the next column
                        else
                        {//Otherwise copy the original element to the new one.
                            erasedMatrix[row][column] = matrix[i][j]; //Assign using row and column.
                            Reset(ref row, ref column, size); //Then reset them.
                        }
                    }
                skip:;
                }
                return erasedMatrix;
            }

            static void Reset(ref int row, ref int column, int size)
            {
                //If we didn't reach the end of the row increase column's index.
                if (column + 1 != size) column++;
                else
                {//If we did reach the end of the row.
                    row++; //Increase the index of the row.
                    column = 0; //Reset the column index.
                }
            }
        }
    }
}
