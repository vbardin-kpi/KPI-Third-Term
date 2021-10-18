using System;
using System.Collections.Generic;
using System.Linq;

using Knapsack.GeneticSolution.Models;
using Knapsack.GeneticSolution.Processors;

namespace Knapsack.GeneticSolution
{
    internal static class Program
    {
        private static readonly Random Randomizer = new();

        private const int KnapsackCapacity = 250;
        private const int ItemsAmount = 100;
        private const int MinItemsWorth = 2;
        private const int MaxItemsWorth = 20;
        private const int MinItemsWeight = 1;
        private const int MaxItemsWeight = 10;
        private const int GeneticIterations = 1000;

        public static void Main()
        {
            var knapsack = new Backpack(GenerateItems(), KnapsackCapacity);

            List<Population> bestPopulations = new();
            for (var iterationNumber = 1; iterationNumber <= GeneticIterations; iterationNumber++)
            {
                var geneticProcessor = new GeneticAlgorithmProcessor(knapsack, 100, 5, 71)
                {
                    CurrentPopulation =
                    {
                        Iteration = iterationNumber
                    }
                };

                bestPopulations.Add(geneticProcessor.CurrentPopulation);
            }

            foreach (var population in bestPopulations)
            {
                Console.WriteLine("Iteration: " + population.Iteration);
                Console.WriteLine("Worth: " + population.Worth);
                Console.WriteLine("Weigh: " + population.TotalWeight);
                Console.WriteLine("Worth Percentage: {0:####.####}%\n", population.WorthPercentage);
            }

            var bestPopulation = bestPopulations.OrderByDescending(p => p.WorthPercentage).First();
            Console.WriteLine("Best Iteration: " + bestPopulation.Iteration +
                              "\nTotal weight: {0}" +
                              "\nTotal worth: {1}" +
                              "\nWorth Percentage: {2:####.####}%",
                bestPopulation.TotalWeight,
                bestPopulation.Worth,
                bestPopulation.WorthPercentage);

            Console.WriteLine("\nAverage Worth Percentage: {0:####.####}%",
                bestPopulations.Average(p => p.WorthPercentage));
        }

        private static IEnumerable<Item> GenerateItems() =>
            Enumerable.Range(0, ItemsAmount)
                .Select(_ => new Item(
                    Randomizer.Next(MinItemsWorth, MaxItemsWorth),
                    Randomizer.Next(MinItemsWeight, MaxItemsWeight)));
    }
}
