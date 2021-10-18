using System;
using System.Collections.Generic;
using System.Linq;

using Knapsack.GeneticSolution.Extensions;
using Knapsack.GeneticSolution.Models;

namespace Knapsack.GeneticSolution.Processors
{
    internal class GeneticAlgorithmProcessor
    {
        private static readonly Random Randomizer = new();

        private static Backpack Backpack { get; set; } = null!;
        public Population CurrentPopulation { get; }

        public GeneticAlgorithmProcessor(Backpack backpack,
            int iterations,
            int mutationChance,
            int dropSelected)
        {
            Backpack = backpack;
            CurrentPopulation = new Population(backpack, new bool[Backpack.Items.Length]);

            var bestPopulations = new List<Population>();
            var populations = GeneratePopulations();
            for (var iterationNumber = 1; iterationNumber <= iterations; iterationNumber++)
            {
                var newPopulation = populations
                    .Selection()
                    .SinglePointCrossbreeding(populations[Randomizer.Next(populations.Count)], backpack)
                    .ProbabilisticMutation(backpack, mutationChance)
                    .ChooseBestLocalUpgrade(backpack, iterationNumber, dropSelected);

                var upgradedPopulation = new Population(backpack, newPopulation.SelectedItems)
                    { Iteration = iterationNumber };

                if (upgradedPopulation.Worth > CurrentPopulation.Worth)
                    CurrentPopulation = upgradedPopulation;

                Population.AddAndDelete(populations, upgradedPopulation);
                if (upgradedPopulation.Worth != 0 &&
                    !bestPopulations.Any(bp => bp.Worth == upgradedPopulation.Worth &&
                                               bp.TotalWeight == upgradedPopulation.TotalWeight))
                    // as an optimization we can store 2, 4 or 8 best populations
                {
                    bestPopulations.Add(upgradedPopulation);
                }
            }
        }

        private static List<Population> GeneratePopulations()
        {
            var itemsAmount = Backpack.Items.Length;
            var populations = new List<Population>();
            for (var i = 0; i < itemsAmount; i++)
            {
                var selectedItems = new bool[itemsAmount];
                selectedItems[i] = true;
                populations.Add(new Population(Backpack, selectedItems));
            }

            return populations;
        }
    }
}