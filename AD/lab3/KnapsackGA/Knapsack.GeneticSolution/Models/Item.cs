namespace Knapsack.GeneticSolution.Models
{
    public class Item
    {
        public int Value { get; }
        public int Weight { get; }
        public bool Selected { get; set; }

        public Item(int value, int weight)
        {
            Value = value;
            Weight = weight;
        }
    }
}