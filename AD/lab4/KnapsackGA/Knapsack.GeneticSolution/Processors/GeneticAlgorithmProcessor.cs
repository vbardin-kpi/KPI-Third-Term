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

        private Population _currentPopulation;

        public Population Process(
            Backpack knapsack,
            int iterations,
            int mutationChance,
            int dropSelected)
        {
            var bestPopulations = new List<Population>();
            var populations = GeneratePopulations(knapsack);
            for (var iterationNumber = 1; iterationNumber <= iterations; iterationNumber++)
            {
                var newPopulation = populations
                    .Selection()
                    .MultipointCrossbreeding(populations[Randomizer.Next(populations.Count)], knapsack, 5)
                    .InversionMutation(knapsack, mutationChance)
                    .ChooseBestLocalUpgrade2(knapsack, iterationNumber, dropSelected);

                var upgradedPopulation = new Population(knapsack, newPopulation.SelectedItems)
                    { Iteration = iterationNumber };

                if (_currentPopulation is null ||upgradedPopulation.Worth > _currentPopulation.Worth)
                    _currentPopulation = upgradedPopulation;

                Population.AddAndDelete(populations, upgradedPopulation);
                if (upgradedPopulation.Worth != 0 &&
                    !bestPopulations.Any(bp => bp.Worth == upgradedPopulation.Worth &&
                                               bp.TotalWeight == upgradedPopulation.TotalWeight))
                    // as an optimization we can store 2, 4 or 8 best populations
                {
                    bestPopulations.Add(upgradedPopulation);
                }
            }

            return _currentPopulation;
        }

        private static List<Population> GeneratePopulations(Backpack knapsack)
        {
            var itemsAmount = knapsack.Items.Length;
            var populations = new List<Population>();
            for (var i = 0; i < itemsAmount; i++)
            {
                var selectedItems = new bool[itemsAmount];
                selectedItems[i] = true;
                populations.Add(new Population(knapsack, selectedItems));
            }

            return populations;
        }
    }
}