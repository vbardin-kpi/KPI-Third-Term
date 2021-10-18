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

        public static void Main()
        {
            var items = GenerateItems().ToArray();

            ExecuteWithIterations(iterations: 1, items);
            ExecuteWithIterations(iterations: 10, items);
            ExecuteWithIterations(iterations: 100, items);
            ExecuteWithIterations(iterations: 1000, items);
            ExecuteWithIterations(iterations: 10000, items);
        }

        private static void ExecuteWithIterations(int iterations, IEnumerable<Item> items)
        {
            var knapsack = new Backpack(items, KnapsackCapacity);

            List<Population> bestPopulations = new();
            for (var iterationNumber = 1; iterationNumber <= iterations; iterationNumber++)
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

            Console.WriteLine();
            Console.WriteLine();
        }


        private static IEnumerable<Item> GenerateItems() =>
            Enumerable.Range(0, ItemsAmount)
                .Select(_ => new Item(
                    Randomizer.Next(MinItemsWorth, MaxItemsWorth),
                    Randomizer.Next(MinItemsWeight, MaxItemsWeight)));
    }
}
