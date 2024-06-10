using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;
using static Algorithms.Mode4;

namespace Algorithms;

public class Mode4
{
    public static void Start(int[][] distances, int[] indicesOfStartingPoints, int[] indicesOfEndingPoints, int[] capacities, int[] weights, int[] values, Settings? settings = null)
    {
        {
            //static int[][] SetStartingAndEndingPoint(int[][] distances, int start, int end)
            //{
            //    bool returnToStartingPoint = start == end;
            //    if (returnToStartingPoint)
            //    {
            //        end = distances.Length;
            //    }
            //    List<List<int>> list = [];
            //    for (int i = 0; i < distances.Length; i++)
            //    {
            //        int additional = (i == start || i == end) ? 0 : -1;
            //        if (returnToStartingPoint)
            //        {
            //            list.Add([.. distances[i].Append(0).Append(additional)]);
            //        }
            //        else
            //        {
            //            list.Add([.. distances[i].Append(additional)]);
            //        }
            //    }

            //    if (returnToStartingPoint)
            //    {
            //        Console.WriteLine(start);
            //        list.Add(new List<int>(list[start]));
            //    }

            //    List<int> dummy = [];
            //    for (int i = 0; i < list[0].Count - 1; i++)
            //    {
            //        int val = (i == start || i == end) ? 0 : -1;
            //        dummy.Add(val);
            //    }

            //    dummy.Add(0);
            //    list.Add(dummy);
            //    return list.Select(x => x.ToArray()).ToArray();
            //}

            //int n = weights.Length;

            //int TotalDistace = 0;
            //int[][] BestRoutes = new int[capacities.Length][];
            //int counter = 0;

            //var (TotalValue, IncludedItems) = Mode3.StartKnapsack(capacities, weights, values, settings?.AlgorithmForKnapsack, settings?.SettingsForGeneticKnapsack);
            //for (int i = 0; i < IncludedItems.Length; i++)
            //{
            //    var (NewDistances, Correctness) = Mode2.TrimDistances(IncludedItems[i], distances);

            //    foreach (var item in Correctness)
            //    {
            //        Console.WriteLine(item);
            //    }
            //    int correctedEndingIndex = Correctness.Keys.ElementAt(Correctness.Values.First(x => x == indicesOfEndingPoints[i]));
            //    int correctedStartingIndex = Correctness.Keys.ElementAt(Correctness.Values.First(x => x == indicesOfStartingPoints[i]));
            //    NewDistances = SetStartingAndEndingPoint(NewDistances, correctedStartingIndex, correctedEndingIndex);
            //    (int Distace, int[] BestRoute) = Mode1.StartTSP(NewDistances, /*settings?.AlgorithmForTSP*/ Algorithm.Brute_Force, settings?.SettingsForGeneticTSP, correctedStartingIndex);
            //    if (indicesOfStartingPoints[i] == indicesOfEndingPoints[i])
            //    {
            //        BestRoute = BestRoute[..^3].Reverse().ToArray();
            //    }
            //    else
            //    {
            //        BestRoute = BestRoute[..^2];
            //    }
            //    if (BestRoute[0] != indicesOfStartingPoints[i])
            //    {
            //        BestRoute = BestRoute.Reverse().ToArray();
            //    }
            //    BestRoute = BestRoute.Append(BestRoute[0]).ToArray();
            //    if (Correctness is not null)
            //    {
            //        BestRoutes[counter++] = new int[BestRoute.Length];

            //        for (int j = 0; j < BestRoute.Length; j++)
            //        {
            //            BestRoutes[counter - 1][j] = Correctness[BestRoute[j]];
            //        }
            //    }
            //    else
            //    {
            //        BestRoutes[counter - 1] = BestRoute;
            //    }
            //    TotalDistace += Distace;
            //}
        }
        var (TotalValue, AllCombinations) = StartKnapsack(capacities, weights, values, settings?.AlgorithmForKnapsack, settings?.SettingsForGeneticKnapsack);
        var (TotalDistance, BestRoutes) = StartTSP(distances, AllCombinations, indicesOfStartingPoints, indicesOfEndingPoints, capacities, settings?.AlgorithmForTSP, settings?.SettingsForGeneticTSP);

        Console.WriteLine($"All Count: {AllCombinations.Count}\n TotalValue: {TotalValue}");
        Console.WriteLine($"Total Distance: {TotalDistance}");
        foreach (var item in BestRoutes)
        {
            foreach (var item1 in item)
            {
                Console.Write($"{item1}, ");
            }
            Console.WriteLine();
        }
    }

    internal static (int TotalValue, List<int[][]> AllCombinations) StartKnapsack(int[] capacities, int[] weights, int[] values, Algorithm? algorithm = null, SettingsForGenetic? settingsForGenetic = null)
    {
        Random rand = new();
        int n = weights.Length;
        int knapsacksCounter = capacities.Length;

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
                throw new NotImplementedException();
            case Algorithm.Genetic:
                return Genetic_Knapsack();
            case null:
                goto case Algorithm.Dynamic;
        }
        throw new Exception("No algorithm was found");

        (int TotalValue, List<int[][]> AllCombinations) Brute_Force_Knapsack()
        {
            int maxTotalValue = 0;
            List<List<int>[]> bestCombinations = [];

            void Recurse(int itemIndex, int[] currentWeights, int currentValue, List<int>[] currentCombination)
            {
                if (itemIndex == n)
                {
                    if (currentValue > maxTotalValue)
                    {
                        maxTotalValue = currentValue;
                        bestCombinations.Clear();
                        var bestCombinationCopy = new List<int>[knapsacksCounter];
                        for (int k = 0; k < knapsacksCounter; k++)
                        {
                            bestCombinationCopy[k] = new List<int>(currentCombination[k]);
                        }
                        bestCombinations.Add(bestCombinationCopy);
                    }
                    else if (currentValue == maxTotalValue)
                    {
                        var bestCombinationCopy = new List<int>[knapsacksCounter];
                        for (int k = 0; k < knapsacksCounter; k++)
                        {
                            bestCombinationCopy[k] = new List<int>(currentCombination[k]);
                        }
                        bestCombinations.Add(bestCombinationCopy);
                    }
                    return;
                }

                // Try not including the current item in any knapsack
                Recurse(itemIndex + 1, currentWeights, currentValue, currentCombination);

                // Try including the current item in each knapsack
                for (int k = 0; k < knapsacksCounter; k++)
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
            List<int>[] currentCombination = new List<int>[knapsacksCounter];
            for (int i = 0; i < knapsacksCounter; i++)
            {
                currentCombination[i] = [];
            }

            Recurse(0, (int[])capacities.Clone(), 0, currentCombination);

            List<int[][]> result = [];
            foreach (var combination in bestCombinations)
            {
                int[][] includedItems = new int[knapsacksCounter][];
                for (int i = 0; i < knapsacksCounter; i++)
                {
                    includedItems[i] = [.. combination[i]];
                }
                result.Add(includedItems);
            }

            return (maxTotalValue, result);
        }
        (int TotalValue, List<int[][]> AllCombinations) Genetic_Knapsack()
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

            List<Mode3.Solution> population = Initialize(populationSize);

            int noChangeCounter = 0;
            int topElite = (int)(eliteRate * populationSize);
            int mutationCount = 0;
            int matingCount = 0;

            int bestFitness = int.MinValue;
            Mode3.Solution bestSolution = null;

            HashSet<string> uniqueCombinations = [];
            List<int[][]> bestCombinations = [];

            for (int i = 0; i < maxIterations; i++)
            {
                List<(int fitness, Mode3.Solution solution)> individualScores = population.Select(ch => (Fitness(ch), ch)).ToList();
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
                    bestCombinations.Clear();
                    uniqueCombinations.Clear();
                }

                int counter = 0;
                while (true)
                {
                    if (individualScores[counter].fitness < bestFitness) break;
                    var items = individualScores[counter++].solution.GetIncludedItems();
                    string combinationKey = GetCombinationKey(items);
                    if (uniqueCombinations.Add(combinationKey))
                    {
                        bestCombinations.Add(items);
                    }
                }

                if (i + 1 == maxIterations || (fast && noChangeCounter > maxIterations / 2))
                {
                    break;
                }
            }
            return (bestFitness, bestCombinations);

            List<Mode3.Solution> Initialize(int size)
            {
                var population = new List<Mode3.Solution>();
                for (int i = 0; i < size; i++)
                {
                    Mode3.Solution chromosome = new(knapsacksCounter, capacities);
                    chromosome.PopulateTheKnapsacks(n);
                    population.Add(chromosome);
                }
                return population;
            }

            int Fitness(Mode3.Solution chrmomsome) => chrmomsome.CalculateFitness(weights, values);

            List<Mode3.Solution> Select(List<(int fitness, Mode3.Solution solution)> individuals, int topElite)
            {
                return individuals.Take(topElite).Select(ind => ind.solution).ToList();
            }

            Mode3.Solution Mutate(Mode3.Solution chromosome)
            {
                var copy = chromosome.Copy();
                copy.Mutate(settingsForGenetic.NumberOfCrossOverPoints);
                return copy;
            }

            Mode3.Solution CrossOver(Mode3.Solution a, Mode3.Solution b)
            {
                return Mode3.Solution.Crossover(a, b, settingsForGenetic.NumberOfCrossOverPoints, capacities);
            }

            string GetCombinationKey(int[][] items)
            {
                return string.Join(";", items.Select(knapsack => string.Join(",", knapsack.OrderBy(x => x))));
            }
        }

    }

    internal static (int TotalDistance, int[][] BestRoutes) StartTSP(int[][] distances,List<int[][]> allCombinations, int[] indicesOfStartingPoints, int[] indicesOfEndingPoints, int[] capacities, Algorithm? algorithm = null, SettingsForGenetic? settingsForGenetic = null)
    {
        Random rand = new();
        int n = distances.Length;
        switch (algorithm)
        {
            case Algorithm.Brute_Force:
                if (n > 9)
                    throw new Exception("Brute Force is Expensive try reducing the number of cities or using another algorithm");
                break;
                //return Brute_Force_TSP();
            case Algorithm.Greedy:
                //return Greedy_TSP();
                break;
            case Algorithm.Branch_And_Bound:
                break;
            case Algorithm.Dynamic:
                // return Dynamic_TSP();
                break;
            case Algorithm.Genetic:
                return Genetic_TSP();
            case null:
                goto case Algorithm.Genetic;
        }
        throw new Exception("No algorithm was found");

        (int TotalDistance, int[][] BestRoutes) Genetic_TSP()
        {
            settingsForGenetic ??= new SettingsForGenetic
            {
                Fast = true,
                PopulationSize = 250,
                MutationProbability = 0.56,
                EliteRate = 0.125,
                MaxIterations = 100,
                NumberOfCrossOverPoints = 1
            };

            Truck[] trucks;
            Location[] locations = new Location[distances.Length];
            for (int i = 0; i < distances.Length; i++)
            {
                locations[i] = new Location
                {
                    Id = i,
                    Distances = distances[i]
                };
            }

            int chosen = rand.Next(allCombinations.Count);
            List<Truck> tru = [];
            for (int i = 0; i < capacities.Length; i++)
            {
                if (allCombinations[chosen][i].Length == 0) continue;
                tru.Add(new Truck
                {
                    Capacity = capacities[i],
                    StartingLocation = locations[indicesOfStartingPoints[i]],
                    EndingLocation = locations[indicesOfEndingPoints[i]],
                    IncludedItems = allCombinations[chosen][i]
                });
            }

            trucks = [.. tru];
            bool fast = settingsForGenetic.Fast;
            int populationSize = settingsForGenetic.PopulationSize;
            double mutationProbability = settingsForGenetic.MutationProbability;
            double eliteRate = settingsForGenetic.EliteRate;
            int maxIterations = settingsForGenetic.MaxIterations;
            var population = Initialize(populationSize);
            List<(int fitness, Chromosome solution)> ind = population.Select(ch => (Fitness(ch), ch)).ToList();
            ind.Sort((a, b) => a.fitness.CompareTo(b.fitness));

            int noChangeCounter = 0;
            int topElite = (int)(eliteRate * populationSize);
            int mutationCount = 0;
            int matingCount = 0;

            int bestFitness = int.MaxValue;
            Chromosome bestSolution = null;

            for (int i = 0; i < maxIterations; i++)
            {
                List<(int fitness, Chromosome solution)> individualScores = population.Select(ch => (Fitness(ch), ch)).ToList();

                individualScores.Sort((a, b) => a.fitness.CompareTo(b.fitness));

                population = Select(individualScores, topElite);

                while (population.Count < populationSize)
                {
                    int c1 = rand.Next(0, topElite);
                    int c2 = rand.Next(0, topElite);
                    population.Add(Crossover(individualScores[c1].solution, individualScores[c2].solution));
                    matingCount++;
                    if (rand.NextDouble() < mutationProbability)
                    {
                        int c = rand.Next(0, topElite);
                        population.Add(Mutate(individualScores[c].solution));
                        mutationCount++;
                    }
                }

                noChangeCounter = individualScores[0].fitness == bestFitness ? noChangeCounter + 1 : 0;
                if (bestFitness > individualScores[0].fitness)
                {
                    bestSolution = individualScores[0].solution.Copy(); // Ensure a deep copy
                    bestSolution.CalculateFitness();
                    bestFitness = bestSolution.Fitness;
                }
                Console.WriteLine("Iteration: {0}", i + 1);
                Console.WriteLine(bestSolution);
                if (i + 1 == maxIterations || (fast && noChangeCounter > maxIterations / 2))
                {
                    break;
                }
            }

            bestSolution.CalculateFitness();
            return (bestSolution.Fitness, bestSolution.GetRoutes());

            List<Chromosome> Initialize(int size)
            {
                List<Chromosome> population = new();
                for (int i = 0; i < size; i++)
                {
                    population.Add(new Chromosome(trucks, locations, allCombinations));
                }
                return population;
            }

            List<Chromosome> Select(List<(int fitness, Chromosome solution)> individuals, int topElite)
            {
                return individuals.Roulette(topElite, lowestIsBetter: true);
            }

            int Fitness(Chromosome chromosome)
            {
                chromosome.CalculateFitness();
                return chromosome.Fitness;
            }

            Chromosome Mutate(Chromosome chromosome)
            {
                return chromosome.Mutate(settingsForGenetic.NumberOfCrossOverPoints);
            }

            Chromosome Crossover(Chromosome a, Chromosome b)
            {
                return Chromosome.Crossover(a, b, settingsForGenetic.NumberOfCrossOverPoints);
            }
        }
    }

    public class Chromosome
    {
        public int Fitness { get; set; }
        public List<Route> Routes { get; set; }

        private Truck[] trucks;
        private readonly Location[] locations;
        private readonly List<int[][]> allCombination;

        public Chromosome(Truck[] trucks, Location[] locations, List<int[][]> all)
        {
            this.trucks = trucks;
            this.locations = locations;
            allCombination = all;
            InitializeRoutes();
        }
        public Chromosome(Truck[] trucks, Location[] locations, List<Route> routes, List<int[][]> all) : this(trucks, locations, all)
        {
            Routes = routes;
        }

        public void InitializeRoutes()
        {
            Random rand = new();
            Routes = new();
            foreach (var truck in trucks)
            {
                var route = new Route(truck, locations);
                route.Shuffle();
                Routes.Add(route);
            }
        }

        public void CalculateFitness()
        {
            Fitness = 0;
            foreach (var route in Routes)
            {
                int fit = route.CalculateFitness();
                Fitness += fit;
            }
        }

        public Chromosome Mutate(int crossoverPoints)
        {
            Random rand = new();
            var newChromosome = Copy();
            newChromosome.CreateNewTrucks();
            var route = newChromosome.Routes[rand.Next(newChromosome.Routes.Count)];
            if (route.RouteNodes.Length > 3)
            {
                int idx1 = rand.Next(1, route.RouteNodes.Length - 1);
                int idx2 = rand.Next(1, route.RouteNodes.Length - 1);
                (route.RouteNodes[idx2], route.RouteNodes[idx1]) = (route.RouteNodes[idx1], route.RouteNodes[idx2]);
            }
            return newChromosome;
        }

        public int[][] GetRoutes()
        {
            int[][] routes = new int[Routes.Count][];
            for (int i = 0; i < routes.Length; i++)
            {
                routes[i] = Routes[i].GetRoute();
            }
            return routes;
        }

        public static Chromosome Crossover(Chromosome a, Chromosome b, int crossoverPoints)
        {
            Random rand = new();
            var childRoutes = new List<Route>();

            foreach (var truck in a.trucks)
            {
                var parentARoute = a.Routes.First(route => route.Truck == truck).RouteNodes;
                var parentBRoute = b.Routes.First(route => route.Truck == truck).RouteNodes;

                var startLocation = truck.StartingLocation;
                var endLocation = truck.EndingLocation;

                if (crossoverPoints > parentARoute.Length - 3)
                {
                    crossoverPoints = parentARoute.Length - 3;
                }

                SortedSet<int> crossoverIndices = new();
                while (crossoverIndices.Count < crossoverPoints && parentARoute.Length > 3)
                {
                    int index = rand.Next(1, parentARoute.Length - 2);
                    crossoverIndices.Add(index);
                }

                List<Location> childRoute = new();

                int currentParentIndex = 0;
                int previousIndex = 0;

                foreach (var crossoverIndex in crossoverIndices)
                {
                    int segmentSize = crossoverIndex - previousIndex;
                    var parentRoute = currentParentIndex % 2 == 0 ? parentARoute : parentBRoute;
                    childRoute.AddRange(parentRoute.Skip(previousIndex).Take(segmentSize));
                    currentParentIndex++;
                    previousIndex = crossoverIndex;
                }

                if (crossoverIndices.Count > 0)
                {
                    childRoute.AddRange(currentParentIndex % 2 == 0 ? parentARoute.Skip(crossoverIndices.Last()) : parentBRoute.Skip(crossoverIndices.Last()));
                }
                else
                {
                    childRoute.AddRange(currentParentIndex % 2 == 0 ? parentARoute : parentBRoute);
                }

                childRoute.Insert(0, startLocation);
                childRoute.Add(endLocation);

                Route finalRoute = new(truck);
                finalRoute.SetRoute(childRoute.ToArray());
                childRoutes.Add(finalRoute);
            }

            var childChromosome = new Chromosome(a.trucks, a.locations, childRoutes, a.allCombination);
            childChromosome.CreateNewTrucks();
            return childChromosome;
        }

        public Chromosome Copy()
        {
            Random rand = new();
            Chromosome newChromosome = new(trucks, locations, allCombination)
            {
                Routes = Routes.Select(r =>
                {
                    var x = new Route(r.Truck);
                    x.SetRoute(r.RouteNodes);
                    return x;
                }).ToList()
            };
            return newChromosome;
        }

        public Truck[] CreateNewTrucks()
        {
            Random rand = new();
            Truck[] newTrucks = new Truck[trucks.Length];
            int chosen = rand.Next(allCombination.Count);
            for (int i = 0; i < trucks.Length; i++)
            {
                newTrucks[i] = new Truck
                {
                    Capacity = trucks[i].Capacity,
                    StartingLocation = trucks[i].StartingLocation,
                    EndingLocation = trucks[i].EndingLocation,
                    IncludedItems = allCombination[chosen][i]
                };
            }
            for (int i = 0; i < Routes.Count; i++)
            {
                Routes[i] = new Route(newTrucks[i], locations);
            }
            trucks = newTrucks;
            return newTrucks;
        }

        public override string ToString()
        {
            CalculateFitness();
            StringBuilder str = new();
            str.AppendLine($"Fitness: {Fitness}");
            str.AppendLine("Trucks");
            foreach (var item in trucks)
            {
                str.AppendLine(item.ToString());
            }
            str.AppendLine("\nRoutes");
            foreach (var item in Routes)
            {
                str.AppendLine(item.ToString());
            }
            return str.ToString();
        }
    }

    public struct Location(int id, int[] distances)
    {
        public int Id { get; set; } = id;
        public int[] Distances { get; set; } = distances;

        public readonly int TravelTo(int index)
        {
            return Distances[index];
        }

        public static int Traverse(Location[] locations)
        {
            int total = 0;
            for (int i = 0; i < locations.Length - 1; i++)
            {
                total += locations[i].TravelTo(locations[i + 1].Id);
            }
            return total;
        }

        public static bool operator ==(Location a, Location b)
        {
            if (a.Distances.Length != b.Distances.Length) return false;
            for (int i = 0; i < a.Distances.Length; i++)
            {
                if (a.Distances[i] != b.Distances[i]) return false;
            }
            return true;
        }

        public static bool operator !=(Location a, Location b) => !(a == b);

        public override string ToString()
        {
            string x = $"Id: {Id} Distances: ";
            foreach (var item in Distances)
            {
                x += $"{item}, ";
            }
            return x.ToString();
        }
    }

    public class Truck
    {
        public int Capacity { get; set; }
        public Location StartingLocation { get; set; }
        public Location EndingLocation { get; set; }
        public int[]? IncludedItems { get; set; }

        public void AddItems(int[] items)
        {
            IncludedItems = items;
        }

        public override string ToString()
        {
            return $"Capacity: {Capacity}, IncludedItems: {string.Join(',', IncludedItems ?? [])}\nStarting Location: {StartingLocation}\nEnding Location: {EndingLocation}";
        }
        public static bool operator ==(Truck a, Truck b)
        {
            return a.Capacity == b.Capacity && a.StartingLocation == b.StartingLocation && a.EndingLocation == b.EndingLocation;
        }

        public static bool operator !=(Truck a, Truck b) => !(a == b);
    }

    public class Route
    {
        public Truck Truck { get; set; }
        public Location[] RouteNodes { get; set; }

        public Route(Truck truck)
        {
            Truck = truck;
            if (truck.IncludedItems is null)
                throw new Exception("Included Items is null");
        }

        public Route(Truck truck, Location[] locations) : this(truck)
        {
            List<Location> list = [truck.StartingLocation];
            for (int i = 1; i < truck.IncludedItems!.Length + 1; i++)
            {
                if (locations[truck.IncludedItems[i - 1]] == truck.StartingLocation ||
                    locations[truck.IncludedItems[i - 1]] == truck.EndingLocation) continue;
                list.Add(locations[truck.IncludedItems[i - 1]]);
            }
            list.Add(truck.EndingLocation);
            RouteNodes = [.. list];
        }

        public void SetRoute(Location[] locations)
        {
            RouteNodes = locations;
        }

        public void Shuffle()
        {
            Random rand = new();
            var correctRoute = RouteNodes[1..^1];
            rand.Shuffle(correctRoute);
            for (int i = 1; i < RouteNodes.Length - 1; i++)
            {
                RouteNodes[i] = correctRoute[i - 1];
            }
        }

        public int CalculateFitness()
        {
            if (Truck is null || RouteNodes is null)
            {
                throw new Exception("Shouldn't be null");
            }
            if (RouteNodes[0] != Truck.StartingLocation || RouteNodes[^1] != Truck.EndingLocation)
                return int.MaxValue;

            return Location.Traverse(RouteNodes);
        }

        public int[] GetRoute()
        {
            List<int> route = [];
            for (int i = 0; i < RouteNodes.Length; i++)
            {
                route.Add(RouteNodes[i].Id);
            }
            return [.. route];
        }

        public override string ToString()
        {
            return string.Join(" --> ", RouteNodes.Select(x => x.Id));
        }
    }
}
