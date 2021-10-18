using System;
using System.Collections.Generic;
using System.Linq;

using Knapsack.GeneticSolution.Models;

namespace Knapsack.GeneticSolution.Extensions
{
    internal static class GaOperators
    {
        private static readonly Random Randomizer = new();

        internal static Population Selection(this IEnumerable<Population> populations)
        {
            var bestPopulation = populations
                .OrderByDescending(p => p.Worth)
                .ThenBy(p => p.TotalWeight)
                .First();
            return bestPopulation;
        }

        internal static Population SinglePointCrossbreeding(this Population lhs, Population rhs, Backpack backpack)
        {
            var halfElementsAmount = lhs.SelectedItems.Length / 2;
            var firstCross = new Population(
                backpack,
                lhs.SelectedItems
                    .Skip(halfElementsAmount)
                    .Concat(rhs.SelectedItems.Take(halfElementsAmount))
                    .ToArray());

            var secondCross = new Population(
                backpack,
                lhs.SelectedItems
                    .Take(halfElementsAmount)
                    .Concat(rhs.SelectedItems.Skip(halfElementsAmount))
                    .ToArray());

            return firstCross.Worth > secondCross.Worth ? firstCross : secondCross;
        }

        internal static Population ProbabilisticMutation(
            this Population population,
            Backpack backpack,
            int mutationChance)
        {
            if (Randomizer.Next(0, 100) > mutationChance) return population;

            var n = Randomizer.Next(0, population.SelectedItems.Length);
            population.SelectedItems[n] = population.SelectedItems[n] == false;
            population = new Population(backpack, population.SelectedItems);
            if (population.Worth == 0)
            {
                population.SelectedItems[n] = population.SelectedItems[n] == false;
            }

            return population;
        }

        internal static Population ChooseBestLocalUpgrade(
            this Population population,
            Backpack backpack,
            int iterationNumber,
            int dropSelected)
        {
            // Find and replace an item with a minimum weight, max value and value >= weight that still wasn't added
            // Each dropSelected - 1 iterations this items will be changed
            if (!backpack.Items.Select(i => i.Selected).Contains(false))
                return population;

            var bestItem = backpack.Items.FirstOrDefault(i => i.Selected == false && i.Weight <= i.Value)
                           ?? backpack.Items.First(i => i.Selected == false);

            var index = Array.IndexOf(backpack.Items, bestItem);
            backpack.Items[index].Selected = true;
            population.SelectedItems[index] = true;

            if (iterationNumber % dropSelected != 0) return population;

            foreach (var item in backpack.Items)
                item.Selected = false;

            return population;
        }
    }
}