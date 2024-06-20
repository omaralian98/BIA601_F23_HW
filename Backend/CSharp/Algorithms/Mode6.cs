using System;
using System.Reflection;
using System.Text;

namespace Algorithms;

public class Mode6
{
    public static (int TotalDistance, int[][] BestRoutes, int TotalValue, int[][] IncludedItems) Start(
        int[][] distances,
        int[] indicesOfStartingPoints,
        int[] indicesOfEndingPoints,
        int[] indicesOfPickingUpPoints,
        int[] indicesOfDroppingOffPoints,
        int[] capacities,
        int[] weights,
        int[] values,
        int[] pickUpPenalties,
        int[] dropOffPenalties,
        Settings? settings = null)
    {
        var (TotalValue, AllCombinations) = Mode4.StartKnapsack(capacities, weights, values, settings?.AlgorithmForKnapsack, settings?.SettingsForGeneticKnapsack);

        List<Location> locations = [];
        for (int i = 0; i < indicesOfPickingUpPoints.Length; i++)
        {
            locations.AddRange
            ([
            new Location
            {
                Id = indicesOfPickingUpPoints[i],
                Distances = distances[indicesOfPickingUpPoints[i]],
                Penalty = pickUpPenalties[i],
            },
            new Location
            {
                Id = indicesOfDroppingOffPoints[i],
                Distances = distances[indicesOfDroppingOffPoints[i]],
                Penalty = dropOffPenalties[i],
            }
            ]);
        }
        List<Item[][]> Included = [];
        for (int i = 0; i < AllCombinations.Count; i++)
        {
            var combination = AllCombinations[i];
            Item[][] items = new Item[combination.Length][];
            for (int j = 0; j < combination.Length; j++)
            {
                Console.WriteLine("H");
                foreach (var item in combination[j])
                {
                    Console.WriteLine(string.Join(", ", item));
                }

                items[j] = combination[j].Select(index => new Item
                {
                    Value = values[index],
                    Weight = weights[index],
                    PickUpLocation = new Location 
                    { 
                        Id = indicesOfPickingUpPoints[index],
                        Distances = distances[indicesOfPickingUpPoints[index]],
                        Penalty = pickUpPenalties[index],
                    },
                    DropOffLocation = new Location 
                    { 
                        Id = indicesOfDroppingOffPoints[index],
                        Distances = distances[indicesOfDroppingOffPoints[index]],
                        Penalty = dropOffPenalties[index],
                    }
                }).ToArray();
            }
            Included.Add(items);
        }
        var (TotalDistance, BestRoutes, IncludedItems) = StartTSP(distances, Included, indicesOfStartingPoints, indicesOfEndingPoints, capacities, locations, settings?.AlgorithmForTSP, settings?.SettingsForGeneticTSP);
        TotalValue = 0;
        foreach (var item in IncludedItems)
        {
            foreach (var item1 in item)
            {
                TotalValue += values[item1];
            }
        }
        return (TotalDistance, BestRoutes, TotalValue, IncludedItems);
    }

    internal static (int TotalDistance, int[][] BestRoutes, int[][] IncludedItems) StartTSP(int[][] distances, List<Item[][]> allCombinations, int[] indicesOfStartingPoints, int[] indicesOfEndingPoints, int[] capacities, List<Location> locations, Algorithm? algorithm = null, SettingsForGenetic? settingsForGenetic = null)
    {
        Random rand = new();
        int n = distances.Length;
        switch (algorithm)
        {
            case Algorithm.Brute_Force:
                throw new NotImplementedException();
            case Algorithm.Greedy:
                throw new NotImplementedException();
            case Algorithm.Branch_And_Bound:
                throw new NotImplementedException();
            case Algorithm.Dynamic:
                throw new NotImplementedException();
            case Algorithm.Genetic:
                return Genetic_TSP();
            case null:
                goto case Algorithm.Genetic;
        }
        throw new Exception("No algorithm was found");

        (int TotalDistance, int[][] BestRoutes, int[][] IncludedItems) Genetic_TSP()
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
            for (int i = 0; i < distances.Length; i++)
            {
                if (!locations.Any(x => x.Id == i))
                {
                    locations.Add(new Location
                    {
                        Id = i,
                        Distances = distances[i]
                    });
                }

            }
            int chosen = rand.Next(allCombinations.Count);
            List<Truck> tru = [];
            for (int i = 0; i < capacities.Length; i++)
            {
                if (allCombinations[chosen][i].Length == 0) continue;
                tru.Add(new Truck
                {
                    Capacity = capacities[i],
                    StartingLocation = locations.First(x => x.Id == indicesOfStartingPoints[i]),
                    EndingLocation = locations.First(x => x.Id == indicesOfEndingPoints[i]),
                    IncludedItems = [.. allCombinations[chosen][i]],
                });
            }

            trucks = [.. tru];
            bool fast = settingsForGenetic.Fast;
            int populationSize = settingsForGenetic.PopulationSize;
            double mutationProbability = settingsForGenetic.MutationProbability;
            double eliteRate = settingsForGenetic.EliteRate;
            int maxIterations = settingsForGenetic.MaxIterations;
            var population = Initialize(populationSize);

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
                if (bestFitness > individualScores[0].fitness || bestSolution is null)
                {
                    bestSolution = individualScores[0].solution.Copy(); // Ensure a deep copy
                    bestSolution.CalculateFitness();
                    bestFitness = bestSolution.Fitness;
                }
                if (i + 1 == maxIterations || (fast && noChangeCounter > maxIterations / 2))
                {
                    break;
                }
            }

            bestSolution?.CalculateFitness();
            return (bestSolution.Fitness, bestSolution.GetRoutes(), bestSolution.GetIncludedItems());

            List<Chromosome> Initialize(int size)
            {
                List<Chromosome> population = new();
                for (int i = 0; i < size; i++)
                {
                    population.Add(new Chromosome(trucks, [..locations], allCombinations));
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
                return chromosome.Mutate();
            }

            Chromosome Crossover(Chromosome a, Chromosome b)
            {
                return Chromosome.Crossover(a, b, settingsForGenetic.NumberOfCrossOverPoints);
            }
        }
    }


    public struct Item(int value, int weight, Location pickUpLocation, Location dropOffLocation)
    {
        public int Value { get; set; } = value;
        public int Weight { get; set; } = weight;
        public Location PickUpLocation { get; set; } = pickUpLocation;
        public Location DropOffLocation { get; set; } = dropOffLocation;

        public override readonly string ToString()
        {
            return $"{DropOffLocation.Id}";
        }
    }

    public class Chromosome
    {
        public int Fitness { get; set; }
        public List<Route> Routes { get; set; }

        private Truck[] trucks;
        private readonly Location[] locations;
        private readonly List<Item[][]> allCombination;

        public Chromosome(Truck[] trucks, Location[] locations, List<Item[][]> all)
        {
            this.trucks = trucks;
            this.locations = locations;
            this.allCombination = all;
            InitializeRoutes();
        }

        public Chromosome(Truck[] trucks, Location[] locations, List<Route> routes, List<Item[][]> all) : this(trucks, locations, all)
        {
            Routes = routes;
        }

        public void InitializeRoutes()
        {
            Routes = [];
            foreach (var truck in trucks)
            {
                var route = new Route(truck);
                route.Shuffle();
                Routes.Add(route);
            }
        }

        public void CalculateFitness()
        {
            Fitness = 0;
            int max = 0;
            foreach (var route in Routes)
            {
                int fit = route.CalculateFitness();
                if (fit == int.MaxValue)
                {
                    max++;
                    continue;
                }
                Fitness += fit;
            }
            if (max > 0)
            {
                Fitness = int.MaxValue;
            }
        }

        public Chromosome Mutate()
        {
            Random rand = new();
            var newChromosome = Copy();
            newChromosome.CreateNewTrucks();
            var route = newChromosome.Routes[rand.Next(newChromosome.Routes.Count)];
            if (route.RouteNodes.Count > 3)
            {
                int idx1 = rand.Next(1, route.RouteNodes.Count - 1);
                int idx2 = rand.Next(1, route.RouteNodes.Count - 1);
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
                var parentARoute = a.Routes.First(route => route.Truck == truck).RouteNodes.ToArray();
                var parentBRoute = b.Routes.First(route => route.Truck == truck).RouteNodes.ToArray();

                var startLocation = truck.StartingLocation;
                var endLocation = truck.EndingLocation;

                if (crossoverPoints > parentARoute.Length - 3)
                {
                    crossoverPoints = parentARoute.Length - 3;
                }

                SortedSet<int> crossoverIndices = [];
                while (crossoverIndices.Count < crossoverPoints && parentARoute.Length > 3)
                {
                    int index = rand.Next(1, parentARoute.Length - 2);
                    crossoverIndices.Add(index);
                }

                List<Location> childRoute = [];
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
                finalRoute.SetRoute([.. childRoute]);
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
                    x.SetRoute([.. r.RouteNodes]);
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
                    IncludedItems = [.. allCombination[chosen][i]]
                };
            }
            for (int i = 0; i < Routes.Count; i++)
            {
                Routes[i] = new Route(newTrucks[i]);
            }
            trucks = newTrucks;
            return newTrucks;
        }

        public int[][] GetIncludedItems()
        {
            int[][] includedItems = new int[trucks.Length][];
            for (int i = 0; i < includedItems.Length; i++)
            {
                includedItems[i] = trucks[i].IncludedItems.Select(item => item.DropOffLocation.Id).ToArray();
            }
            return includedItems;
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
        public int Penalty { get; set; } = 0;

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
            if (a.Id != b.Id) return false;
            for (int i = 0; i < a.Distances.Length; i++)
            {
                if (a.Distances[i] != b.Distances[i]) return false;
            }
            return true;
        }

        public static bool operator !=(Location a, Location b) => !(a == b);

        public override readonly string ToString()
        {
            string x = $"Id: {Id}, Penalty: {Penalty}, Distances: ";
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
        public List<Item> IncludedItems { get; set; } = [];

        public void AddItems(List<Item> items)
        {
            IncludedItems = items;
        }

        public override string ToString()
        {
            return $"Capacity: {Capacity}, IncludedItems: {string.Join(", ", IncludedItems)},\nStarting Location: {StartingLocation}\nEnding Location: {EndingLocation}";
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
        public List<Location> RouteNodes { get; set; }

        public Route(Truck truck)
        {
            Truck = truck ?? throw new Exception("Truck shouldn't be null");
            if (truck.IncludedItems == null)
            {
                throw new Exception("Included Items shouldn't be null");
            }

            RouteNodes = [truck.StartingLocation];
            HashSet<int> visited = [];
            foreach (var item in truck.IncludedItems)
            {
                if (!visited.Contains(item.PickUpLocation.Id))
                {
                    RouteNodes.Add(item.PickUpLocation);
                    visited.Add(item.PickUpLocation.Id);
                }

                if (!visited.Contains(item.DropOffLocation.Id))
                {
                    RouteNodes.Add(item.DropOffLocation);
                    visited.Add(item.DropOffLocation.Id);
                }
            }
            RouteNodes.Add(truck.EndingLocation);
        }

        public void SetRoute(Location[] locations)
        {
            RouteNodes = [.. locations];
        }

        public void Shuffle()
        {
            Random rand = new();
            var correctRoute = RouteNodes.Skip(1).Take(RouteNodes.Count - 2).ToList();
            correctRoute = [.. correctRoute.OrderBy(x => rand.Next())];
            for (int i = 1; i < RouteNodes.Count - 1; i++)
            {
                RouteNodes[i] = correctRoute[i - 1];
            }
        }

        public int CalculateFitness(bool yes = false)
        {
            int fitness = 0;
            if (Truck is null || RouteNodes is null)
            {
                throw new Exception("Truck or RouteNodes shouldn't be null");
            }
            if (RouteNodes[0] != Truck.StartingLocation || RouteNodes[^1] != Truck.EndingLocation)
            {
                return int.MaxValue; //If the solution doesn't start with the startinglocation or doesn't end with the endinglocation 
            }
            if (Truck.IncludedItems.Count == 0)
            {
                return 0; //If the truck doesn't have any included Items
            }
            foreach (var item in Truck.IncludedItems)
            {
                Dictionary<int, bool> pickedUp = [];
                foreach (var inc in Truck.IncludedItems)
                {
                    pickedUp[inc.DropOffLocation.Id] = false;
                }
                for (int i = 1; i < RouteNodes.Count - 1; i++)
                {
                    if (RouteNodes[i] == item.PickUpLocation)
                    {
                        pickedUp[item.DropOffLocation.Id] = true;
                    }
                    else if (RouteNodes[i] == item.DropOffLocation)
                    {
                        if (!pickedUp[RouteNodes[i].Id])
                        {
                            return int.MaxValue; // Drop-off location visited before pick-up location
                        }
                        break;
                    }
                }
                // If we reached this part of the code that means The route is valid
                // The only thing left is the penalty of picking this item up and dropping it off
                fitness += item.PickUpLocation.Penalty;
                fitness += item.DropOffLocation.Penalty;
            }
            fitness += Location.Traverse([.. RouteNodes]);
            return fitness;
        }

        public int[] GetRoute()
        {
            if (RouteNodes.Count == 2)
            {
                return [];
            }
            int skip = 0;
            if (RouteNodes[0] == RouteNodes[1]) skip++;
            return RouteNodes.Skip(skip).Select(node => node.Id).ToArray();
        }

        public override string ToString()
        {
            return string.Join(" --> ", RouteNodes.Select(x => x.Id));
        }
    }
}