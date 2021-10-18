using System.Collections.Generic;
using System.Linq;

namespace Knapsack.GeneticSolution.Models
{
    public class Backpack
    {
        public int Capacity { get; }
        public Item[] Items { get; }
        public Backpack(IEnumerable<Item> items, int capacity)
        {
            Capacity = capacity;
            Items = items.OrderBy(i => i.Weight).ThenByDescending(i => i.Value).ToArray();
        }
    }
}