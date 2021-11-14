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

        private const int KnapsackCapacity = 500;
        private const int ItemsAmount = 100;
        private const int MinItemsWorth = 2;
        private const int MaxItemsWorth = 30;
        private const int MinItemsWeight = 1;
        private const int MaxItemsWeight = 20;

        private const int MinIterations = 2;
        private const int IterationsIncreaseFactor = 3;
        private const int MaxIterations = 100;

        public static void Main()
        {
            var items = GenerateItems().ToArray();

            var results = new List<(double, int, int)>();

            for (int k = 0; k < 10; k++)
            {
                var populations = new List<(Population, int)>();
                var bestMutation = 1;

                for (var mutationChance = 1; mutationChance <= 100; mutationChance++)
                {
                    populations.Add((ExecuteWithIterations(MinIterations, mutationChance, items), mutationChance));
                }

                var bestPopulationByMutation = populations.OrderBy(x => x.Item1.WorthPercentage).First();
                bestMutation = bestPopulationByMutation.Item2;
                populations.Clear();

                for (var i = MinIterations; i < MaxIterations; i *= IterationsIncreaseFactor)
                {
                    populations.Add((ExecuteWithIterations(i, bestMutation, items), i));
                }

                var bestIterations = populations.OrderBy(x => x.Item1.WorthPercentage).First().Item2;

                var bestPopulation = ExecuteWithIterations(bestIterations, bestMutation, items);

                results.Add((bestPopulation.WorthPercentage, bestMutation, bestIterations));
            }

            foreach (var result in results.OrderBy(x => x.Item1))
            {
                Console.WriteLine("The best found parameters were: iterations -- {0},  mutation chance -- {1}. " +
                                  "After executing an algorithm with these parameters worth percentage was: {2:####.####}%",
                    result.Item3, result.Item2, result.Item1);
            }
        }

        private static Population ExecuteWithIterations(int iterations, int mutation, IEnumerable<Item> items)
        {
            var knapsack = new Backpack(items, KnapsackCapacity);

            List<Population> bestPopulations = new();
            var geneticProcessor = new GeneticAlgorithmProcessor();

            for (var iterationNumber = 1; iterationNumber <= iterations; iterationNumber++)
            {
                var population = geneticProcessor.Process(
                    knapsack: knapsack,
                    iterations: 100,
                    mutationChance: mutation,
                    dropSelected: 71);
                bestPopulations.Add(population);
            }

            return bestPopulations.OrderByDescending(p => p.WorthPercentage).First();
        }

        private static IEnumerable<Item> GenerateItems() =>
            Enumerable.Range(0, ItemsAmount)
                .Select(_ => new Item(
                    Randomizer.Next(MinItemsWorth, MaxItemsWorth),
                    Randomizer.Next(MinItemsWeight, MaxItemsWeight)));
    }
}
