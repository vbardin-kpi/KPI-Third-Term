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

        /// <summary>
        /// <b> Single point crossover operator. </b> <br/>
        /// The new vector X' gets the first 50 coordinates from vector X1,
        /// and the remaining 50 from vector X2.
        /// </summary>
        internal static Population SinglePointCrossbreeding(
            this Population lhs,
            Population rhs,
            Backpack backpack)
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

        /// <summary>
        /// <b> Uniform crossing operator. </b> <br/>
        /// A new solution in each coordinate is obtained with a probability of 0.5 the value of one of the parents.
        /// </summary>
        internal static Population UniformCrossbreeding(
            this Population first,
            Population second,
            Backpack knapsack)
        {
            var n = first.SelectedItems.Length;
            var array = new[] {first, second};
            var rnd = new Random();
            var firstItemsSelected = new bool[n];
            var secondItemsSelected = new bool[n];
            for (int i = 0; i < n; i++)
            {
                var select = rnd.Next(2);
                firstItemsSelected[i] = array[select].SelectedItems[i];
                select = select == 0 ? 1 : 0;
                secondItemsSelected[i] = array[select].SelectedItems[i];
            }

            var firstCross = new Population(knapsack, firstItemsSelected);
            var secondCross = new Population(knapsack, secondItemsSelected);
            return firstCross.Worth > secondCross.Worth ? firstCross : secondCross;
        }

        /// <summary>
        /// <b> Multipoint crossbreeding operator. </b> <br/>
        /// Divides populations by points into equal segments
        /// and in turn selects line segments. At the output, we have two populations - choose the best one.
        /// </summary>
        internal static Population MultipointCrossbreeding(
            this Population first,
            Population second,
            Backpack knapsack,
            int pointNumbers)
        {
            var n = first.SelectedItems.Length / (pointNumbers + 1);
            var sum = 0;
            var partsOfFirstArray = new List<bool[]>();
            var partsOfSecondArray = new List<bool[]>();
            for (var i = 0; i < pointNumbers + 1; i++)
            {
                if (i % 2 == 0)
                {
                    partsOfFirstArray.Add(first.SelectedItems.Skip(sum).Take(n).ToArray());
                    partsOfSecondArray.Add(second.SelectedItems.Skip(sum).Take(n).ToArray());
                }
                else
                {
                    partsOfFirstArray.Add(second.SelectedItems.Skip(sum).Take(n).ToArray());
                    partsOfSecondArray.Add(first.SelectedItems.Skip(sum).Take(n).ToArray());
                }
                sum += n;
            }

            var firstFinalPop = new bool[first.SelectedItems.Length];
            var secondFinalPop = new bool[first.SelectedItems.Length];
            var num = 0;
            for (var i = 0; i < partsOfFirstArray.Count; i++)
            {
                for (var j = 0; j < partsOfFirstArray[i].Length; j++)
                {
                    firstFinalPop[j + num] = partsOfFirstArray[i][j];
                    secondFinalPop[j + num] = partsOfSecondArray[i][j];
                }

                num += partsOfFirstArray[0].Length;
            }
            var firstCross = new Population(knapsack, firstFinalPop);
            var secondCross = new Population(knapsack, secondFinalPop);

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

        /// <summary>
        /// <b> Inverse mutation operator </b> <br/>
        /// Elements from both ends are swapped in pairs on a random segment
        /// </summary>
        internal static Population InversionMutation(
            this Population population,
            Backpack knapsack,
            int chance)
        {
            if (Randomizer.Next(0, 100) > chance) return population;

            var segment = new[]
            {
                Randomizer.Next(0, population.SelectedItems.Length),
                Randomizer.Next(0, population.SelectedItems.Length)
            };
            var start = segment.Min();
            var end = segment.Max();
            for (int i = start, j = end; i < j; i++, j--)
            {
                (population.SelectedItems[i], population.SelectedItems[j])
                    = (population.SelectedItems[j], population.SelectedItems[i]);
            }

            population = new Population(knapsack, population.SelectedItems);

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

        internal static Population ChooseBestLocalUpgrade2(
            this Population population,
            Backpack backpack,
            int iterationNumber,
            int dropSelected)
        {
            if (!backpack.Items.Select(i => i.Selected).Contains(false))
                return population;

            // Select an item that wasn't selected yet and has the minimum weight and maximum value
            var bestItem = backpack.Items.Aggregate((lhs, rhs)
                => !lhs.Selected &&
                   !rhs.Selected &&
                   lhs.Weight < rhs.Weight &&
                   lhs.Value > rhs.Value
                    ? lhs : rhs);

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