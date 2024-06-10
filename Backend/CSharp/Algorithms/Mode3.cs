using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace Algorithms;

public class Mode3
{
    public static (int TotalDistance, int[][] BestRoutes, int TotalValue, int[][] IncludedItems) Start(int[][] distances, int[] capacities, int[] weights, int[] values, Settings? settings = null)
    {
        var (TotalValue, IncludedItems) = StartKnapsack(capacities, weights, values, settings?.AlgorithmForKnapsack, settings?.SettingsForGeneticKnapsack);
        int TotalDistace = 0;
        int[][] BestRoutes = new int[capacities.Length][];
        int counter = 0;
        for (int j = 0; j < IncludedItems.Length; j++)
        {
            if (IncludedItems.Length == 1)
            {
                BestRoutes[counter++] = IncludedItems[j];
                continue;
            }
            var (NewDistances, Correctness) = Mode2.TrimDistances(IncludedItems[j], distances);

            (int Distace, int[] BestRoute) = Mode1.StartTSP(NewDistances, settings?.AlgorithmForTSP, settings?.SettingsForGeneticTSP);

            if (Correctness is not null)
            {
                BestRoutes[counter++] = new int[BestRoute.Length];

                for (int i = 0; i < BestRoute.Length; i++)
                {
                    BestRoutes[counter - 1][i] = Correctness[BestRoute[i]];
                }
            }
            else
            {
                BestRoutes[counter - 1] = BestRoute;
            }
            TotalDistace += Distace;
        }
        return (TotalDistace, BestRoutes, TotalValue, IncludedItems);
    }

    internal class Solution(int n, int[] capacities)
    {
        public Knapsack[] Knapsacks { get; set; } = new Knapsack[n];

        // Key: The column, Value: The row
        public Dictionary<int, int> TakenItems { get; set; } = [];

        public int Fitness { get; set; }

        public void PopulateTheKnapsacks(int size)
        {
            for (int i = 0; i < Knapsacks.Length; i++)
            {
                Knapsacks[i] = new Knapsack(size, capacities[i]);
                var taken = Knapsacks[i].PopulateTheKnapsack();
                for (int j = 0; j < taken.Length; j++)
                {
                    if (TakenItems.TryGetValue(taken[j], out int knapsackIndex))
                    {
                        Knapsacks[knapsackIndex].Change(taken[j]);
                    }
                    TakenItems[taken[j]] =  i;
                }
            }
        }

        public Solution Copy()
        {
            var copy = new Solution(n, capacities)
            {
                TakenItems = new Dictionary<int, int>(TakenItems),
                Knapsacks = new Knapsack[n]
            };

            for (int i = 0; i < Knapsacks.Length; i++)
            {
                copy.Knapsacks[i] = new Knapsack(Knapsacks[i].Chromosome.Length, capacities[i]);
                Array.Copy(Knapsacks[i].Chromosome, copy.Knapsacks[i].Chromosome, Knapsacks[i].Chromosome.Length);
            }

            return copy;
        }

        public void Mutate(int crossoverPoints)
        {
            Random rand = new();
            if (n == crossoverPoints)
                crossoverPoints--;

            for (int knapsackIndex = 0; knapsackIndex < Knapsacks.Length; knapsackIndex++)
            {
                HashSet<int> indices = [];
                while (indices.Count < crossoverPoints)
                {
                    indices.Add(rand.Next(n));
                }
                foreach (var itemIndex in indices)
                {
                    if (TakenItems.TryGetValue(itemIndex, out int takenKnapsackIndex))
                    {
                        Knapsacks[takenKnapsackIndex].Change(itemIndex);
                    }
                    TakenItems[itemIndex] = knapsackIndex;
                    Knapsacks[knapsackIndex].Change(itemIndex);
                }
            }
        }

        public static Solution Crossover(Solution a, Solution b, int crossoverPoints, int[] capacities)
        {
            int n = a.Knapsacks[0].Chromosome.Length;
            Solution child = new(capacities.Length, capacities);
            child.PopulateTheKnapsacks(n);

            Random rand = new();
            for (int i = 0; i < child.Knapsacks.Length; i++)
            {
                SortedSet<int> indices = [n];
                while (indices.Count < crossoverPoints + 1)
                {
                    indices.Add(rand.Next(n));
                }

                bool takeFromA = true;
                int previousPoint = 0;
                foreach (int point in indices)
                {
                    for (int j = previousPoint; j < point; j++)
                    {
                        child.Knapsacks[i].Chromosome[j] = takeFromA ? a.Knapsacks[i].Chromosome[j] : b.Knapsacks[i].Chromosome[j];
                    }
                    takeFromA = !takeFromA;
                    previousPoint = point;
                }
            }
            child.ResetTakenItems();
            return child;
        }


        public int CalculateFitness(int[] weights, int[] values)
        {
            int answer = 0;
            for (int i = 0; i < capacities.Length; i++)
            {
                Knapsacks[i].CalculateFitness(weights, values);
                answer += Knapsacks[i].Fitness;
            }
            Fitness = answer;
            return answer;
        }

        private void ResetTakenItems()
        {
            TakenItems.Clear();
            for (int i = 0; i < Knapsacks.Length; i++)
            {
                for (int j = 0; j < Knapsacks[i].Chromosome.Length; j++)
                {
                    if (Knapsacks[i].Chromosome[j])
                    {
                        if (TakenItems.TryGetValue(j, out int knapsackIndex))
                        {
                            Knapsacks[knapsackIndex].Change(j);
                        }
                        TakenItems[j] = i;
                    }
                }
            }
        }

        public int[][] GetIncludedItems()
        {
            int[][] includedItems = new int[n][];
            for (int i = 0; i < n; i++)
            {
                includedItems[i] = Knapsacks[i].GetIncludedItems();
            }
            return includedItems;
        }
        
        public override string ToString()
        {
            StringBuilder st = new();
            foreach (var knapsack in Knapsacks)
            {
                st.AppendLine(knapsack.ToString());
            }
            return st.ToString();
        }
    }

    internal struct Knapsack(int size, int capacity)
    {
        public bool[] Chromosome { get; set; } = new bool[size];
        public int Fitness { get; set; }
        public int Capacity { get; set; } = capacity;

        public readonly int[] PopulateTheKnapsack()
        {
            Random rand = new();
            List<int> taken = [];
            for (int i = 0; i < Chromosome.Length; i++)
            {
                Chromosome[i] = Convert.ToBoolean(rand.Next(2));
                if (Chromosome[i])
                {
                    taken.Add(i);
                }
            }
            return [.. taken];
        }

        public readonly void Change(int index) => Chromosome[index] = !Chromosome[index];

        public void CalculateFitness(int[] weights, int[] values)
        {
            int weight = 0;
            int value = 0;
            for (int i = 0; i < size; i++)
            {
                if (Chromosome[i])
                {
                    weight += weights[i];
                    value += values[i];
                }
            }
            Fitness = weight > capacity ? capacity - weight : value;
        }

        public readonly int[] GetIncludedItems()
        {
            List<int> items = [];
            for (int i = 0; i < size; i++)
            {
                if (Chromosome[i]) items.Add(i);
            }
            return [.. items];
        }

        public override readonly string ToString()
        {
            return string.Join("", Chromosome.Select(bit => bit ? "1" : "0"));
        }
    }

    internal static (int TotalValue, int[][] IncludedItems) StartKnapsack(int[] capacities, int[] weights, int[] values, Algorithm? algorithm = null, SettingsForGenetic? settingsForGenetic = null)
    {
        Random rand = new();
        int n = weights.Length;

        switch (algorithm)
        {
            case Algorithm.Brute_Force:
                if (weights.Length > 20)
                    throw new Exception("Brute Force is Expensive try reducing the items or using another algorithm");
                return Brute_Force_Knapsack();
            case Algorithm.Greedy:
                throw new Exception("0/1 Knapsack doesn't have a greedy algorithm");
            case Algorithm.Branch_And_Bound:
                throw new NotImplementedException();
            case Algorithm.Dynamic:
                return Dynamic_Knapsack();
            case Algorithm.Genetic:
                return Genetic_Knapsack();
            case null:
                goto case Algorithm.Dynamic;
        }
        throw new Exception("No algorithm was found");

        (int TotalValue, int[][] IncludedItems) Brute_Force_Knapsack()
        {
            int n = weights.Length;
            int m = capacities.Length;

            int maxTotalValue = 0;
            List<int>[] bestCombination = new List<int>[m];
            for (int i = 0; i < m; i++)
            {
                bestCombination[i] = [];
            }

            void Recurse(int itemIndex, int[] currentWeights, int currentValue, List<int>[] currentCombination)
            {
                if (itemIndex == n)
                {
                    if (currentValue > maxTotalValue)
                    {
                        maxTotalValue = currentValue;
                        for (int k = 0; k < m; k++)
                        {
                            bestCombination[k] = new List<int>(currentCombination[k]);
                        }
                    }
                    return;
                }

                // Try not including the current item in any knapsack
                Recurse(itemIndex + 1, currentWeights, currentValue, currentCombination);

                // Try including the current item in each knapsack
                for (int k = 0; k < m; k++)
                {
                    if (currentWeights[k] >= weights[itemIndex])
                    {
                        currentWeights[k] -= weights[itemIndex];
                        currentCombination[k].Add(itemIndex);
                        Recurse(itemIndex + 1, currentWeights, currentValue + values[itemIndex], currentCombination);
                        currentCombination[k].RemoveAt(currentCombination[k].Count - 1);
                        currentWeights[k] += weights[itemIndex];
                    }
                }
            }

            // Initialize the current combination array
            List<int>[] currentCombination = new List<int>[m];
            for (int i = 0; i < m; i++)
            {
                currentCombination[i] = [];
            }

            Recurse(0, (int[])capacities.Clone(), 0, currentCombination);

            int[][] result = new int[m][];
            for (int i = 0; i < m; i++)
            {
                result[i] = [.. bestCombination[i]];
            }

            return (maxTotalValue, result);
        }
        (int TotalValue, int[][] IncludedItems) Dynamic_Knapsack()
        {
            int n = weights.Length;
            int[] lengths = new int[capacities.Length + 1];
            lengths[0] = n + 1;
            for (int i = 0; i < capacities.Length; i++)
            {
                lengths[i + 1] = capacities[i] + 1;
            }

            var memo = Array.CreateInstance(typeof(int), lengths);
            var includedItemsMemo = Array.CreateInstance(typeof(List<int>[]), lengths);

            // Initialize the included items memo table
            void InitializeIncludedItemsMemo(int knapsackIndex, int[] indices)
            {
                if (knapsackIndex == capacities.Length)
                {
                    var includedItemsArray = new List<int>[capacities.Length];
                    for (int i = 0; i < capacities.Length; i++)
                    {
                        includedItemsArray[i] = [];
                    }
                    includedItemsMemo.SetValue(includedItemsArray, indices);
                    return;
                }
                for (int w = 0; w <= capacities[knapsackIndex]; w++)
                {
                    indices[knapsackIndex + 1] = w;
                    InitializeIncludedItemsMemo(knapsackIndex + 1, indices);
                }
            }

            InitializeIncludedItemsMemo(0, new int[capacities.Length + 1]);

            // Local function to traverse DP
            void TraverseDP(int itemIndex1Based)
            {
                void TraverseDPHelper(int knapsackIndex, int[] indices)
                {
                    if (knapsackIndex == capacities.Length)
                    {
                        indices[0] = itemIndex1Based;
                        int[] prevIndexes = (int[])indices.Clone();
                        prevIndexes[0] = itemIndex1Based - 1;
                        int maxValue = (int)memo.GetValue(prevIndexes);
                        List<int>[] bestIncludedItems = (List<int>[])includedItemsMemo.GetValue(prevIndexes);

                        for (int k = 0; k < capacities.Length; k++)
                        {
                            if (weights[itemIndex1Based - 1] <= indices[k + 1])
                            {
                                int[] newIndexes = (int[])prevIndexes.Clone();
                                newIndexes[k + 1] -= weights[itemIndex1Based - 1];
                                int newValue = values[itemIndex1Based - 1] + (int)memo.GetValue(newIndexes);

                                if (newValue > maxValue)
                                {
                                    maxValue = newValue;
                                    bestIncludedItems = (List<int>[])includedItemsMemo.GetValue(newIndexes);
                                    bestIncludedItems = (List<int>[])bestIncludedItems.Clone();
                                    bestIncludedItems[k] = new List<int>(bestIncludedItems[k])
                                    {
                                        itemIndex1Based - 1
                                    };
                                }
                            }
                        }

                        memo.SetValue(maxValue, indices);
                        includedItemsMemo.SetValue(bestIncludedItems, indices);
                        return;
                    }

                    for (int w = 0; w <= capacities[knapsackIndex]; w++)
                    {
                        indices[knapsackIndex + 1] = w;
                        TraverseDPHelper(knapsackIndex + 1, indices);
                    }
                }

                TraverseDPHelper(0, new int[capacities.Length + 1]);
            }

            for (int itemIndex1Based = 1; itemIndex1Based <= n; itemIndex1Based++)
            {
                TraverseDP(itemIndex1Based);
            }

            int[] GetIndices()
            {
                int[] indices = new int[capacities.Length + 1];
                indices[0] = n;
                for (int i = 0; i < capacities.Length; i++)
                {
                    indices[i + 1] = capacities[i];
                }
                return indices;
            }

            int[] finalIndices = GetIndices();
            int maxValue = (int)memo.GetValue(finalIndices);
            List<int>[] includedItems = (List<int>[])includedItemsMemo.GetValue(finalIndices);
            int[][] includedItemsResult = new int[capacities.Length][];

            for (int i = 0; i < capacities.Length; i++)
            {
                includedItemsResult[i] = includedItems[i]?.ToArray() ?? [];
            }

            return (maxValue, includedItemsResult);
        }
        (int TotalValue, int[][] IncludedItems) Genetic_Knapsack()
        {
            int knapsacksCounter = capacities.Length;
            settingsForGenetic ??= new SettingsForGenetic
            {
                Fast = true,
                PopulationSize = 250,
                MutationProbability = 0.36,
                EliteRate = 0.125,
                MaxIterations = 100,
                NumberOfCrossOverPoints = 1
            };
            bool fast = settingsForGenetic.Fast;
            int populationSize = settingsForGenetic.PopulationSize;
            double mutationProbability = settingsForGenetic.MutationProbability;
            double eliteRate = settingsForGenetic.EliteRate;
            int maxIterations = settingsForGenetic.MaxIterations;


            List<Solution> population = Initialize(populationSize);

            int noChangeCounter = 0;
            int topElite = (int)(eliteRate * populationSize);
            int mutationCount = 0;
            int matingCount = 0;

            int bestFitness = int.MinValue;
            Solution? bestSolution = null;


            for (int i = 0; i < maxIterations; i++)
            {
                List<(int fitness, Solution solution)> individualScores = population.Select(ch => (Fitness(ch), ch)).ToList();
                individualScores.Sort((a, b) => b.fitness.CompareTo(a.fitness));

                population = Select(individualScores, topElite);

                while (population.Count < populationSize)
                {
                    int c1 = rand.Next(0, topElite);
                    int c2 = rand.Next(0, topElite);
                    population.Add(CrossOver(individualScores[c1].solution, individualScores[c2].solution));
                    matingCount++;
                    if (rand.NextDouble() < mutationProbability)
                    {
                        int c = rand.Next(0, topElite);
                        population.Add(Mutate(individualScores[c].solution));
                        mutationCount++;
                    }
                }

                noChangeCounter = individualScores[0].fitness == bestFitness ? noChangeCounter + 1 : 0;

                if (bestFitness < individualScores[0].fitness)
                {
                    bestFitness = individualScores[0].fitness;
                    bestSolution = individualScores[0].solution;
                }

                if (i + 1 == maxIterations || (fast && noChangeCounter > maxIterations / 2))
                {
                    break;
                }
            }
            return (bestFitness, bestSolution.GetIncludedItems());


            List<Solution> Initialize(int size)
            {
                var population = new List<Solution>();
                for (int i = 0; i < size; i++)
                {
                    Solution chromosome = new(knapsacksCounter, capacities);
                    chromosome.PopulateTheKnapsacks(n);
                    population.Add(chromosome);
                }
                return population;
            }

            int Fitness(Solution chrmomsome) => chrmomsome.CalculateFitness(weights, values);

            List<Solution> Select(List<(int fitness, Solution solution)> individuals, int topElite)
            {
                return individuals.Roulette(topElite);
            }

            Solution Mutate(Solution chromosome)
            {
                var copy = chromosome.Copy();
                copy.Mutate(settingsForGenetic?.NumberOfCrossOverPoints ?? 1);
                return copy;
            }

            Solution CrossOver(Solution a, Solution b)
            {
                return Solution.Crossover(a, b, settingsForGenetic?.NumberOfCrossOverPoints ?? 1, capacities);
            }
        }
    }



}




//namespace Algorithms;

//public class Mode3
//{
//    public static (int TotalDistance, int[] BestRoute, int TotalValue, string Solution) Start(int[][] distances, int[] capacities, int[] weights, int[] values, Settings? settings = null)
//    {
//        int N_KnapSacks(int[] capacities, int[] weights, int[] values)
//        {
//            int n = weights.Length;
//            int[] lengths = new int[capacities.Length + 1];
//            lengths[0] = n + 1;
//            for (int i = 0; i < capacities.Length; i++)
//            {
//                lengths[i + 1] = capacities[i] + 1;
//            }

//            var memo = Array.CreateInstance(typeof(int), lengths);

//            // Local function to traverse DP
//            void TraverseDP(int itemIndex1Based)
//            {
//                void TraverseDPHelper(int knapsackIndex, int[] indices)
//                {
//                    // Base case: if we have processed all knapsacks
//                    if (knapsackIndex == capacities.Length)
//                    {
//                        // Update indexes array to reflect the current item being processed
//                        indices[0] = itemIndex1Based;

//                        // Create a copy of indexes for the previous item state
//                        int[] prevIndexes = (int[])indices.Clone();
//                        prevIndexes[0] = itemIndex1Based - 1;

//                        // Get the value from memo for excluding the current item
//                        int maxValue = (int)memo.GetValue(prevIndexes);

//                        // Loop through all knapsacks to check if the item can be included
//                        for (int k = 0; k < capacities.Length; k++)
//                        {
//                            // If the item fits in the current knapsack
//                            if (weights[itemIndex1Based - 1] <= indices[k + 1])
//                            {
//                                // Create a new state with the current item included in knapsack k
//                                int[] newIndexes = (int[])prevIndexes.Clone();
//                                newIndexes[k + 1] -= weights[itemIndex1Based - 1];

//                                // Update maxValue considering the current item
//                                maxValue = Math.Max(maxValue, values[itemIndex1Based - 1] + (int)memo.GetValue(newIndexes));
//                            }
//                        }

//                        // Set the calculated maxValue in the memo table for the current state
//                        memo.SetValue(maxValue, indices);
//                        return;
//                    }

//                    // Recursive case: explore all capacities for the current knapsack
//                    for (int w = 0; w <= capacities[knapsackIndex]; w++)
//                    {
//                        // Update the indexes array for the current knapsack
//                        indices[knapsackIndex + 1] = w;

//                        // Recurse to process the next knapsack
//                        TraverseDPHelper(knapsackIndex + 1, indices);
//                    }
//                }

//                // Initialize the recursion with the first knapsack and a fresh indexes array
//                TraverseDPHelper(0, new int[capacities.Length + 1]);
//            }


//            // Fill the DP table
//            for (int itemIndex1Based = 1; itemIndex1Based <= n; itemIndex1Based++)
//            {
//                // Loop through the first dimension 
//                TraverseDP(itemIndex1Based);
//            }

//            // Local function to get indexes for final result extraction
//            int[] GetIndices()
//            {
//                int[] indices = new int[capacities.Length + 1];
//                indices[0] = n;
//                for (int i = 0; i < capacities.Length; i++)
//                {
//                    indices[i + 1] = capacities[i];
//                }
//                return indices;
//            }

//            // The result is in memo[n, capacities[0], capacities[1], ..., capacities[m]]
//            int maxValue = (int)memo.GetValue(GetIndices());
//            return maxValue;
//        }
//        (int TotalValue, int[][] IncludedItems) N_KnapSacksWithPath(int[] capacities, int[] weights, int[] values)
//        {
//            int n = weights.Length;
//            int m = capacities.Length;
//            int[] lengths = new int[m + 1];
//            lengths[0] = n + 1;
//            for (int i = 0; i < m; i++)
//            {
//                lengths[i + 1] = capacities[i] + 1;
//            }

//            var memo = Array.CreateInstance(typeof((int, List<int>[])), lengths);

//            // Initialize the memo table
//            for (int i = 0; i <= n; i++)
//            {
//                int[] indices = new int[m + 1];
//                indices[0] = i;
//                for (int j = 0; j <= m; j++)
//                {
//                    indices[j] = 0;
//                }
//                var value = (0, Enumerable.Range(0, m).Select(_ => new List<int>()).ToArray());
//                memo.SetValue(value, indices);
//            }

//            // Local function to traverse DP
//            void TraverseDP(int itemIndex1Based)
//            {
//                void TraverseDPHelper(int knapsackIndex, int[] indices)
//                {
//                    // Base case: if we have processed all knapsacks
//                    if (knapsackIndex == m)
//                    {
//                        indices[0] = itemIndex1Based;
//                        int[] prevIndexes = (int[])indices.Clone();
//                        prevIndexes[0] = itemIndex1Based - 1;

//                        var prevValue = ((int, List<int>[]))memo.GetValue(prevIndexes);
//                        int maxValue = prevValue.Item1;
//                        var maxItems = prevValue.Item2.Select(list => new List<int>(list)).ToArray();

//                        // Loop through all knapsacks to check if the item can be included
//                        for (int k = 0; k < m; k++)
//                        {
//                            if (weights[itemIndex1Based - 1] <= indices[k + 1])
//                            {
//                                int[] newIndexes = (int[])prevIndexes.Clone();
//                                newIndexes[k + 1] -= weights[itemIndex1Based - 1];

//                                var newValue = ((int, List<int>[]))memo.GetValue(newIndexes);
//                                int newMaxValue = newValue.Item1 + values[itemIndex1Based - 1];
//                                if (newMaxValue > maxValue)
//                                {
//                                    maxValue = newMaxValue;
//                                    maxItems = newValue.Item2.Select(list => new List<int>(list)).ToArray();
//                                    maxItems[k].Add(itemIndex1Based - 1);
//                                }
//                            }
//                        }

//                        memo.SetValue((maxValue, maxItems), indices);
//                        return;
//                    }

//                    // Recursive case: explore all capacities for the current knapsack
//                    for (int w = 0; w <= capacities[knapsackIndex]; w++)
//                    {
//                        indices[knapsackIndex + 1] = w;
//                        TraverseDPHelper(knapsackIndex + 1, indices);
//                    }
//                }

//                TraverseDPHelper(0, new int[m + 1]);
//            }

//            // Fill the DP table
//            for (int itemIndex1Based = 1; itemIndex1Based <= n; itemIndex1Based++)
//            {
//                TraverseDP(itemIndex1Based);
//            }

//            // Local function to get indexes for final result extraction
//            int[] GetIndices()
//            {
//                int[] indices = new int[m + 1];
//                indices[0] = n;
//                for (int i = 0; i < m; i++)
//                {
//                    indices[i + 1] = capacities[i];
//                }
//                return indices;
//            }

//            // Get the final result
//            var result = ((int, List<int>[]))memo.GetValue(GetIndices());
//            int[][] includedItems = result.Item2.Select(list => list.ToArray()).ToArray();
//            return (result.Item1, includedItems);
//        }


//        int KnapSackNormal(int capacity, int[] weights, int[] values)
//        {
//            int n = weights.Length;
//            int[,] memo = new int[n + 1, capacity + 1];

//            for (int i = 0; i <= n; i++)
//            {
//                for (int j = 0; j <= capacity; j++)
//                {
//                    if (i == 0 || j == 0)
//                    {
//                        memo[i, j] = 0;
//                    }
//                    else
//                    {
//                        memo[i, j] = memo[i - 1, j];

//                        if (weights[i - 1] <= j)
//                        {
//                            int included = values[i - 1] + memo[i - 1, j - weights[i - 1]];
//                            memo[i, j] = Math.Max(memo[i, j], included);
//                        }
//                    }
//                }
//            }
//            return memo[n, capacity];
//        }

//        int TwoKnapsacks(int capacity1, int capacity2, int[] weights, int[] values)
//        {
//            int n = weights.Length;
//            int[,,] memo = new int[n + 1, capacity1 + 1, capacity2 + 1];

//            for (int i = 0; i <= n; i++)
//            {
//                for (int j = 0; j <= capacity1; j++)
//                {
//                    for (int k = 0; k <= capacity2; k++)
//                    {
//                        if (i == 0 || j == 0 || k == 0)
//                        {
//                            memo[i, j, k] = 0;
//                        }
//                        else
//                        {
//                            memo[i, j, k] = memo[i - 1, j, k];

//                            if (weights[i - 1] <= j)
//                            {
//                                int includedInFirst = values[i - 1] + memo[i - 1, j - weights[i - 1], k];
//                                memo[i, j, k] = Math.Max(memo[i, j, k], includedInFirst);
//                            }

//                            if (weights[i - 1] <= k)
//                            {
//                                int includedInSecond = values[i - 1] + memo[i - 1, j, k - weights[i - 1]];
//                                memo[i, j, k] = Math.Max(memo[i, j, k], includedInSecond);
//                            }
//                        }
//                    }
//                }
//            }
//            return memo[n, capacity1, capacity2];
//        }

//        int ThreeKnapsacks(int capacity1, int capacity2, int capacity3, int[] weights, int[] values)
//        {
//            int n = weights.Length;
//            int[,,,] memo = new int[n + 1, capacity1 + 1, capacity2 + 1, capacity3 + 1];

//            for (int i = 0; i <= n; i++)
//            {
//                for (int j = 0; j <= capacity1; j++)
//                {
//                    for (int k = 0; k <= capacity2; k++)
//                    {
//                        for (int l = 0; l <= capacity3; l++)
//                        {
//                            if (i == 0 || j == 0 || k == 0 || l == 0)
//                            {
//                                memo[i, j, k, l] = 0;
//                            }
//                            else
//                            {
//                                memo[i, j, k, l] = memo[i - 1, j, k, l];

//                                if (weights[i - 1] <= j)
//                                {
//                                    int includedInFirst = values[i - 1] + memo[i - 1, j - weights[i - 1], k, l];
//                                    memo[i, j, k, l] = Math.Max(memo[i, j, k, l], includedInFirst);
//                                }

//                                if (weights[i - 1] <= k)
//                                {
//                                    int includedInSecond = values[i - 1] + memo[i - 1, j, k - weights[i - 1], l];
//                                    memo[i, j, k, l] = Math.Max(memo[i, j, k, l], includedInSecond);
//                                }

//                                if (weights[i - 1] <= l)
//                                {
//                                    int includedInThird = values[i - 1] + memo[i - 1, j, k, l - weights[i - 1]];
//                                    memo[i, j, k, l] = Math.Max(memo[i, j, k, l], includedInThird);
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//            return memo[n, capacity1, capacity2, capacity3];
//        }
//    }
//}
