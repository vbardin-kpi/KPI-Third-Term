namespace Yahtzee.Logic.MiniMax;

public class MiniMaxNode
{
    public double NodeWeight { get; }
    public YahtzeeStep? TargetCombination { get; }
    public List<MiniMaxNode> Children { get; }
    public List<int> DiceIndexesToHold { get; }

    public MiniMaxNode(List<YahtzeeStep> availableCombinations, List<int> diceFrequent, List<int> holdIndexes)
    {
        DiceIndexesToHold = new List<int>();
        Children = new List<MiniMaxNode>();
        TargetCombination = availableCombinations.FirstOrDefault();
        if (TargetCombination is null)
        {
            NodeWeight = 0;
            return;
        }

        var diceLeft = 5 - holdIndexes.Count;
        var totalDice = 5;

        if (diceLeft > 1)
        {
            DiceIndexesToHold.Add(diceLeft - 1);
        }
        
        var xp2Sum = 0d;

        for (var i = 0; i < totalDice; i++)
        {
            xp2Sum += 1d / (diceFrequent[i] + diceLeft) *
                      Math.Pow(diceFrequent[i] / diceLeft - diceLeft / totalDice, 2);
        }

        NodeWeight = (int) TargetCombination * Math.Abs(diceLeft * totalDice * xp2Sum);
        availableCombinations.Remove(TargetCombination.Value);

        if (availableCombinations.Any())
        {
            GenerateChildren(availableCombinations, diceFrequent, holdIndexes);
        }
    }

    private void GenerateChildren(List<YahtzeeStep> availableCombinations, List<int> diceFrequent,
        List<int> holdIndexes)
    {
        foreach (var combination in availableCombinations)
        {
            Children.Add(new MiniMaxNode(new List<YahtzeeStep> {combination},
                diceFrequent, holdIndexes));
        }
    }
}