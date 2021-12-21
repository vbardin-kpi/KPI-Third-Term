namespace Yahtzee.Logic.Helpers;

public static class YahtzeeStepHelper
{
    public static bool IsFirstSection(this YahtzeeStep step)
    {
        return step switch
        {
            YahtzeeStep.Ones => true,
            YahtzeeStep.Twos => true,
            YahtzeeStep.Threes => true,
            YahtzeeStep.Fours => true,
            YahtzeeStep.Fives => true,
            YahtzeeStep.Sixes => true,

            _ => false
        };
    }

    public static List<YahtzeeStep> GetFirstSectionCombinations()
    {
        return new List<YahtzeeStep>(6)
        {
            YahtzeeStep.Ones,
            YahtzeeStep.Twos,
            YahtzeeStep.Threes,
            YahtzeeStep.Fours,
            YahtzeeStep.Fives,
            YahtzeeStep.Sixes
        };
    }
    
    public static List<YahtzeeStep> GetSecondSectionCombinations()
    {
        return new List<YahtzeeStep>(6)
        {
            YahtzeeStep.ThreeOfAKind,
            YahtzeeStep.FourOfAKind,
            YahtzeeStep.SmStraight,
            YahtzeeStep.LgStraight,
            YahtzeeStep.FullHouse,
            YahtzeeStep.Yahtzee,
            YahtzeeStep.Chance
        };
    }
}