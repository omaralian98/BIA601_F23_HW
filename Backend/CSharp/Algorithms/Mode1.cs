namespace Algorithms;

public class Mode1
{
    public static (int TotalDistance, int[] BestRoute, int TotalValue, int[] IncludedItems) Start(int[][] distances, int capacity, int[] weights, int[] values, Settings? settings = null)
    {
        (int TotalValue, int[] IncludedItems) = StartKnapsack(capacity, weights, values, settings?.AlgorithmForKnapsack, settings?.SettingsForGeneticKnapsack);
        (int TotalDistace, int[] BestRoute) = StartTSP(distances, settings?.AlgorithmForTSP, settings?.SettingsForGeneticTSP);
        return (TotalDistace, BestRoute, TotalValue, IncludedItems);
    }

    internal static (int TotalDistance, int[] BestRoute) StartTSP(int[][] distances, Algorithm? algorithm = null, SettingsForGenetic? settingsForGenetic = null)
    {
        Random rand = new();
        int n = distances.Length;
        switch (algorithm)
        {
            case Algorithm.Brute_Force:
                if (n > 9)
                    throw new Exception("Brute Force is Expensive try reducing the number of cities or using another algorithm");
                return Brute_Force_TSP();
            case Algorithm.Greedy:
                return Greedy_TSP();
            case Algorithm.Branch_And_Bound:
                break;
            case Algorithm.Dynamic:
                return Dynamic_TSP();
            case Algorithm.Genetic:
                return Genetic_TSP();
            case null:
                if (n <= 10)
                    goto case Algorithm.Dynamic;
                else 
                    goto case Algorithm.Genetic;
        }
        throw new Exception("No algorithm was found");

        (int TotalDistance, int[] BestRoute) Brute_Force_TSP()
        {
            int n = distances.Length;
            int[] cities = Enumerable.Range(0, n).ToArray();
            var permutations = GetPermutations(cities, n).ToArray();
            int minDistance = int.MaxValue;
            int[] bestRoute = null;

            foreach (var perm in permutations)
            {
                int currentDistance = CalculateRouteDistance(perm, distances);
                if (currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                    bestRoute = [.. perm];
                }
            }

            // Local function to calculate the distance of a given route
            int CalculateRouteDistance(int[] route, int[][] distances)
            {
                int totalDistance = distances[route[^1]][route[0]]; // Return to the starting point
                for (int i = 0; i < route.Length - 1; i++)
                {
                    totalDistance += distances[route[i]][route[i + 1]];
                }
                return totalDistance;
            }

            // Local function to generate permutations of the cities
            IEnumerable<int[]> GetPermutations(int[] list, int length)
            {
                if (length == 1) 
                    return list.Select(city => new int[] { city });

                return GetPermutations(list, length - 1)
                    .SelectMany(t => list.Where(e => !t.Contains(e)), (t1, t2) => t1.Concat(new int[] { t2 }).ToArray());
            }

            return (minDistance, bestRoute?.Append(bestRoute[0]).ToArray() ?? []);
        }
        (int TotalDistance, int[] BestRoute) Greedy_TSP()
        {
            bool[] visited = new bool[n];
            int totalDistance = 0;
            int[] BestRoute = new int[n + 1];

            var city = rand.Next(n);
            visited[city] = true;
            BestRoute[0] = BestRoute[^1] = city;

            for (int i = 1; i < n; i++)
            {
                int index = -1;
                int distance = int.MaxValue;
                for (int j = 0; j < n; j++)
                {
                    if (!visited[j] && distances[BestRoute[i - 1]][j] < distance)
                    {
                        index = j;
                        distance = distances[BestRoute[i - 1]][j];
                    }
                }
                BestRoute[i] = index;
                totalDistance += distance;
                visited[index] = true;
            }
            totalDistance += distances[BestRoute[^2]][BestRoute[^1]];
            return (totalDistance, BestRoute);
        }
        (int TotalDistance, int[] BestRoute) Dynamic_TSP()
        {
            int n = distances.Length;
            var memo = new Dictionary<string, (int, int)>();
            var path = new List<int>();

            bool IsOver(int visited, int n) => visited == (1 << n) - 1;
            bool IsVisited(int visited, int node) => (visited & (1 << node)) != 0;
            int AddNode(int visited, int node) => visited | (1 << node);

            int TSPFinder(int currentNode, int visited)
            {
                string key = $"{currentNode}-{visited}";

                if (IsOver(visited, n))
                {
                    return distances[currentNode][0];
                }

                if (memo.TryGetValue(key, out var value))
                {
                    return value.Item1;
                }

                int minDistance = int.MaxValue;
                int bestNextNode = -1;

                for (int nextNode = 0; nextNode < n; nextNode++)
                {
                    if (!IsVisited(visited, nextNode))
                    {
                        int distance = distances[currentNode][nextNode] + TSPFinder(nextNode, AddNode(visited, nextNode));
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            bestNextNode = nextNode;
                        }
                    }
                }

                memo[key] = (minDistance, bestNextNode);

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
                    current = memo[key].Item2;
                    visited = AddNode(visited, current);
                }
                path.Add(current);
                path.Add(0); // Return to the starting city
            }

            int minDistanceResult = TSPFinder(0, 1);
            ReconstructPath();

            return (minDistanceResult, [..path]);

        }
        (int TotalDistance, int[] BestRoute) Genetic_TSP()
        {
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
            var population = Initialize(populationSize);
            List<(int fitness, int[] solution)> ind = [.. population.Select(ch => (Fitness(ch), ch))];
            ind.Sort((a, b) => a.fitness.CompareTo(b.fitness));

            int noChangeCounter = 0;
            int topElite = (int)(eliteRate * populationSize);
            int mutationCount = 0;
            int matingCount = 0;

            int bestFitness = int.MaxValue;
            int[] bestSolution = null;




            for (int i = 0; i < maxIterations; i++)
            {
                List<(int fitness, int[] solution)> individualScores = population.Select(ch => (Fitness(ch), ch)).ToList();

                individualScores.Sort((a, b) => a.fitness.CompareTo(b.fitness));


                var rankedIndividuals = individualScores.Select(sv => sv.solution).ToList();

                population = [.. rankedIndividuals.Take(topElite)];

                while (population.Count < populationSize)
                {
                    if (rand.NextDouble() < mutationProbability)
                    {
                        int c = rand.Next(0, topElite);
                        population.Add(Mutate(rankedIndividuals[c]));
                        mutationCount++;
                    }
                    else
                    {
                        int c1 = rand.Next(0, topElite);
                        int c2 = rand.Next(0, topElite);
                        population.Add(CrossOver(rankedIndividuals[c1], rankedIndividuals[c2]));
                        matingCount++;
                    }
                }
                noChangeCounter = individualScores[0].fitness == bestFitness ? noChangeCounter + 1 : 0;

                if (bestFitness > individualScores[0].fitness)
                {
                    bestFitness = individualScores[0].fitness;
                    bestSolution = individualScores[0].solution;
                }
                if (i + 1 == maxIterations)
                {
                    break;
                }
            }

            return (bestFitness, [.. bestSolution?.Append(bestSolution[0])]);

            List<int[]> Initialize(int size)
            {
                List<int[]> poplation = [];
                for (int i = 0; i < size; i++)
                {
                    int[] chromosome = new int[n];
                    for (int j = 0; j < n; j++)
                    {
                        chromosome[j] = j;
                    }
                    rand.Shuffle(chromosome);
                    poplation.Add(chromosome);
                }
                return poplation;
            }

            int Fitness(int[] chromosome)
            {
                int total = distances[chromosome[^1]][chromosome[0]];
                for (int i = 1; i < n; i++)
                {
                    total += distances[chromosome[i - 1]][chromosome[i]];
                }
                return total;
            }

            int[] Mutate(int[] chromosome)
            {
                int numberOfCrossPoints = settingsForGenetic.NumberOfCrossOverPoints;
                if (int.IsOddInteger(numberOfCrossPoints)) numberOfCrossPoints++;
                int[] mutant = new int[n];
                chromosome.CopyTo(mutant, 0);

                HashSet<int> indices = [];
                while (indices.Count < numberOfCrossPoints)
                {
                    indices.Add(rand.Next(n));
                }

                var ind = indices.ToList();
                for (int i = 1; i < numberOfCrossPoints; i += 2)
                {
                    Swap(ind[i - 1], ind[i]);
                }

                void Swap(int x, int y)
                {
                    (mutant[x], mutant[y]) = (mutant[y], mutant[x]);
                }

                return mutant;
            }

            int[] CrossOver(int[] a, int[] b)
            {
                int numberOfCrossPoints = settingsForGenetic.NumberOfCrossOverPoints;
                int[] child = new int[n];

                a.CopyTo(child, 0);
                rand.Shuffle(child);

                //Array.Fill(child, -1);  // Initialize with invalid values


                //SortedSet<int> points = [n - 1];
                //while (points.Count < numberOfCrossPoints)
                //{
                //    points.Add(rand.Next(1, n));
                //}



                //bool swap = false;
                //int lastIndex = 0;
                //bool[] taken = new bool[n];
                //HashSet<int> indices = [];
                //foreach (var index in points)
                //{
                //    for (int i = lastIndex; i < index; i++)
                //    {
                //        if (swap && taken[b[i]]) continue;
                //        if (!swap && taken[a[i]]) continue;
                //        child[i] = swap ? b[i] : a[i];
                //        taken[child[i]] = true;
                //    }
                //    swap = !swap;
                //    lastIndex = index;
                //}

                //// Fill in any missing values
                //for (int j = 0; j < n; j++)
                //{
                //    if (child[j] == -1)
                //    {
                //        for (int k = 0; k < n; k++)
                //        {
                //            if (!taken[k])
                //            {
                //                child[j] = k;
                //                taken[k] = true;
                //                break;
                //            }
                //        }
                //    }
                //}

                return child;
            }
        }
    }

    internal static (int TotalValue, int[] IncludedItems) StartKnapsack(int capacity, int[] weights, int[] values, Algorithm? algorithm = null, SettingsForGenetic? settingsForGenetic = null) 
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

        (int TotalValue, int[] IncludedItems) Brute_Force_Knapsack()
        {
            bool IsIncluded(int combination, int currentItem) => (combination & (1 << currentItem)) != 0;
            int maxValue = 0;
            bool[] bestCombination = new bool[n];

            // Iterate through all possible combinations
            // Go to the end of the function if you don't understand this loop
            for (int combination = 0; combination < (1 << n); combination++)
            {
                int totalWeight = 0;
                int totalValue = 0;
                bool[] solution = new bool[n];

                // Check each item to see if it's included in the current combination
                for (int itemindex = 0; itemindex < n; itemindex++)
                {
                    if (IsIncluded(combination, itemindex))
                    {
                        totalWeight += weights[itemindex];
                        totalValue += values[itemindex];
                        solution[itemindex] = true;
                    }
                }

                // If the current combination is better, update the best combination
                if (totalWeight <= capacity && totalValue > maxValue)
                {
                    maxValue = totalValue;
                    bestCombination = (bool[])solution.Clone();
                }
            }
            List<int> IncludedItems = [];
            for (int i = 0; i < bestCombination.Length; i++)
            {
                if (bestCombination[i])
                {
                    IncludedItems.Add(i);
                }
            }
            return (maxValue, IncludedItems.ToArray());

            /*
                Let's say we have 10 item => n = 10.
                All the combinations go like this:
                0000000000 => 0
                0000000001 => 1
                0000000010 => 2
                0000000011 => 3
                0000000100 => 4
                0000000101 => 5
                0000000110 => 6
                ...
                ...
                ...
                1111111111 => 2^10 - 1
                So we have to loop from 0 to 1023 and test each one to find the best one.
                This is what this line does "for (int i = 0; i < (1 << n); i++)"
                (1 << n) = (1 << 10) = 10000000000 = 1024 the range is: [0, 1024[, and that's what we want
             */
        }
        (int TotalValue, int[] IncludedItems) Dynamic_Knapsack()
        {
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
            List<int> items = new List<int>();
            for (int i = 0; i < itemsIncluded.Length; i++)
            {
                if (itemsIncluded[i] == 1)
                {
                    items.Add(i);
                }
            }

            return (memo[n, capacity], items.ToArray());
        }
        {
            //(int TotalValue, string Solution) Branch_And_Bound_Knapsack()
            //{
            //    int maxValue = 0;
            //    bool[] bestCombination = new bool[n];

            //    var SortedItems = Enumerable.Range(0, n)
            //        .Select(index => (index, ratio: (double)values[index] / weights[index]))
            //        .OrderByDescending(x => x.ratio)
            //        .ToList();

            //    // Helper function to calculate the upper bound using the greedy approach on fractional knapsack approach
            //    double CalculateBound(int level, int weight, int value)
            //    {
            //        if (weight >= capacity) return 0;

            //        double bound = value;
            //        int totalWeight = weight;
            //        for (int i = level + 1; i < n; i++)
            //        {
            //            int index = SortedItems[i].index;
            //            if (totalWeight + weights[index] <= capacity)
            //            {
            //                totalWeight += weights[index];
            //                bound += values[index];
            //            }
            //            else
            //            {
            //                bound += (capacity - totalWeight) * ((double)values[index] / weights[index]);
            //                break;
            //            }
            //        }

            //        return bound;
            //    }

            //    // Priority queue to store the state as tuples (level, value, weight, bound, solution)
            //    var pq = new PriorityQueue<(int Level, int Value, int Weight, double Bound, bool[] Solution), double>();

            //    // Initial state
            //    var initialSolution = new bool[n];
            //    double initialBound = CalculateBound(-1, 0, 0);
            //    pq.Enqueue((-1, 0, 0, initialBound, initialSolution), -initialBound);

            //    while (pq.Count > 0)
            //    {
            //        var (level, currentValue, currentWeight, currentBound, currentSolution) = pq.Dequeue();

            //        if (level == n - 1 || currentBound <= maxValue)
            //            continue;

            //        // Branch to include the next item
            //        if (level + 1 < n)
            //        {
            //            var withItemSolution = (bool[])currentSolution.Clone();
            //            withItemSolution[level + 1] = true;
            //            int withItemWeight = currentWeight + weights[level + 1];
            //            int withItemValue = currentValue + values[level + 1];

            //            if (withItemWeight <= capacity && withItemValue > maxValue)
            //            {
            //                maxValue = withItemValue;
            //                bestCombination = (bool[])withItemSolution.Clone();
            //            }

            //            double withItemBound = CalculateBound(level + 1, withItemWeight, withItemValue);

            //            if (withItemBound > maxValue)
            //            {
            //                pq.Enqueue((level + 1, withItemValue, withItemWeight, withItemBound, withItemSolution), -withItemBound);
            //            }

            //            // Branch to exclude the next item
            //            var withoutItemSolution = (bool[])currentSolution.Clone();
            //            double withoutItemBound = CalculateBound(level + 1, currentWeight, currentValue);

            //            if (withoutItemBound > maxValue)
            //            {
            //                pq.Enqueue((level + 1, currentValue, currentWeight, withoutItemBound, withoutItemSolution), -withoutItemBound);
            //            }
            //        }
            //    }

            //    return (maxValue, string.Join("", bestCombination.Select(ch => ch ? '1' : '0')));
            //}
        }
        (int TotalValue, int[] IncludedItems) Genetic_Knapsack()
        {
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


            List<bool[]> population = Initialize(populationSize);

            int noChangeCounter = 0;
            int topElite = (int)(eliteRate * populationSize);
            int mutationCount = 0;
            int matingCount = 0;

            int bestFitness = int.MinValue;
            bool[]? bestSolution = null;

            for (int i = 0; i < maxIterations; i++)
            {
                List<(int fitness, bool[] solution)> individualScores = population.Select(v => (Fitness(v), v)).ToList();

                individualScores.Sort((a, b) => b.fitness.CompareTo(a.fitness));


                var rankedIndividuals = individualScores.Select(sv => sv.solution).ToList();

                population = [.. rankedIndividuals.Take(topElite)];

                while (population.Count < populationSize)
                {
                    if (rand.NextDouble() < mutationProbability)
                    {
                        int c = rand.Next(0, topElite);
                        population.Add(Mutate(rankedIndividuals[c]));
                        mutationCount++;
                    }
                    else
                    {
                        int c1 = rand.Next(0, topElite);
                        int c2 = rand.Next(0, topElite);
                        population.Add(CrossOver(rankedIndividuals[c1], rankedIndividuals[c2]));
                        matingCount++;
                    }
                }

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
            List<int> IncludedItems = [];
            for (int i = 0; i < bestSolution.Length; i++)
            {
                if (bestSolution[i])
                {
                    IncludedItems.Add(i);
                }
            }
            return (bestFitness, IncludedItems.ToArray());

            List<bool[]> Initialize(int size)
            {
                List<bool[]> chromosomes = [];
                chromosomes.Add(Special(n, true));
                chromosomes.Add(Special(n, false));
                for (int i = 2; i < size; i++)
                {
                    bool[] chromosome = new bool[n];
                    for (int j = 0; j < n; j++)
                    {
                        chromosome[j] = Convert.ToBoolean(rand.Next(2));
                    }
                    chromosomes.Add(chromosome);
                }
                return chromosomes;
                bool[] Special(int size, bool addAll)
                {
                    bool[] chromosome = new bool[size];
                    for (int j = 0; j < size; j++)
                    {
                        chromosome[j] = addAll;
                    }
                    return chromosome;
                }
            }

            int Fitness(bool[] chromosome)
            {
                int totalWeight = 0;
                int totalValue = 0;
                for (int i = 0; i < n; i++)
                {
                    if (chromosome[i])
                    {
                        totalWeight += weights[i];
                        totalValue += values[i];
                    }
                }
                int answer = totalWeight > capacity ? capacity - totalWeight : totalValue;
                return answer;
            }

            bool[] Mutate(bool[] chromosome)
            {
                int numberOfCrossPoints = settingsForGenetic.NumberOfCrossOverPoints;
                bool[] mutant = new bool[n];

                HashSet<int> indices = []; // Collection Without Duplicates
                while (indices.Count < numberOfCrossPoints)
                {
                    indices.Add(rand.Next(n));
                }

                for (int i = 0; i < n; i++)
                {
                    mutant[i] = indices.Contains(i) ? !chromosome[i] : chromosome[i];
                }

                return mutant;
            }

            bool[] CrossOver(bool[] a, bool[] b)
            {
                int numberOfCrossPoints = settingsForGenetic.NumberOfCrossOverPoints + 1;
                bool[] child = new bool[n];
                bool swap = false;


                SortedSet<int> points = [n - 1]; // Sorted Collection Without Duplicates
                while (points.Count < numberOfCrossPoints)
                {
                    points.Add(rand.Next(1, n));
                }

                HashSet<int> indices = new(); 
                while (indices.Count < numberOfCrossPoints)
                {
                    indices.Add(rand.Next(n));
                }
                List<int> point = indices.ToList();
                point.Sort();

                int lastIndex = 0;
                foreach (var index in points)
                {
                    for (int j = lastIndex; j < index; j++)
                    {
                        child[j] = swap ? b[j] : a[j];
                    }
                    swap = !swap;
                    lastIndex = index;
                }
                return child;
            }

            string Print(bool[] chromosome)
            {
                return string.Join("", chromosome.Select(bit => bit ? "1" : "0"));
            }
        }
    }
}