using System.Collections.Generic;
using System.Linq;

namespace Knapsack.GeneticSolution.Models
{
    public class Population
    {
        public bool[] SelectedItems { get; }
        public int TotalWeight { get; }
        public int Worth { get; }
        public double WorthPercentage { get; }
        public int Iteration { get; set; }

        public Population(Backpack backpack, bool[] selectedItems)
        {
            SelectedItems = selectedItems;
            Worth = backpack.Items.Where((_, i) => selectedItems[i]).Sum(t => t.Value);

            for (var i = 0; i < backpack.Items.Length; i++)
                if (selectedItems[i])
                    TotalWeight += backpack.Items[i].Weight;

            if (TotalWeight > backpack.Capacity)
                Worth = 0;

            WorthPercentage = (double)TotalWeight / Worth * 100;
        }

        public static void AddAndDelete(List<Population> populations, Population added)
        {
            populations.Add(added);
            var minWorth = populations.Select(p => p.Worth).Min();
            for (var i = 0; i < populations.Count; i++)
                if (populations[i].Worth == minWorth)
                    populations.RemoveAt(i);
        }
    }
}