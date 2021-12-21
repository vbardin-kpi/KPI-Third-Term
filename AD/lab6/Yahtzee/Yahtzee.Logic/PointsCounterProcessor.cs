using Yahtzee.Logic.Helpers;
using Yahtzee.Logic.Models;

namespace Yahtzee.Logic;

public class PointsCounterProcessor
{
    private const int FirstSectionBonusRequirement = 35;
    private const int FirstSectionBonusAmount = 60;

    public int CountFirstSection(List<int> diceFrequent, YahtzeeStep step)
    {
        var yahtzeeStepIndex = (int) step;
        return diceFrequent[yahtzeeStepIndex] switch
        {
            3 => 0,
            _ => (diceFrequent[yahtzeeStepIndex] - 3) * (yahtzeeStepIndex + 1)
        };
    }

    public int CountSecondSection(List<int> diceFrequent, YahtzeeStep yahtzeeStep)
    {
        return yahtzeeStep switch
        {
            YahtzeeStep.ThreeOfAKind => GetPointsAmountForAmountCombinations(diceFrequent, 3),
            YahtzeeStep.FourOfAKind => GetPointsAmountForAmountCombinations(diceFrequent, 4),
            YahtzeeStep.FullHouse => GetFullHousePoints(diceFrequent),
            YahtzeeStep.SmStraight => GetSmStraightPoints(diceFrequent),
            YahtzeeStep.LgStraight => GetLgStraightPoints(diceFrequent),
            YahtzeeStep.Yahtzee => GetPointsAmountForAmountCombinations(diceFrequent, 5),
            YahtzeeStep.Chance => SumOfAllDice(diceFrequent),
            _ => throw new ArgumentOutOfRangeException(nameof(yahtzeeStep), yahtzeeStep, null)
        };
    }

    public (int points, int bonus) CountSectionTotal(int sectionNumber, GameProgressTracking progressTracking)
    {
        var requiredOptions = sectionNumber is 1
            ? YahtzeeStepHelper.GetFirstSectionCombinations()
            : YahtzeeStepHelper.GetSecondSectionCombinations();

        var totalPoints = requiredOptions.Select(x => progressTracking.Steps[x]!.Value).Sum();

        return sectionNumber switch
        {
            1 => totalPoints >= FirstSectionBonusRequirement
                ? (totalPoints, FirstSectionBonusAmount)
                : (totalPoints, 0),
            2 => (totalPoints, 0),
            _ => throw new ArgumentOutOfRangeException(nameof(sectionNumber), sectionNumber, null)
        };
    }

    private int GetPointsAmountForAmountCombinations(List<int> lst, int reqAmount)
    {
        var diceAmount = lst.Where(x => x >= reqAmount).OrderBy(x => x).FirstOrDefault();
        if (diceAmount == default) return 0;

        var dieIndex = lst.IndexOf(diceAmount);
        if (lst[dieIndex] != reqAmount) return 0;

        return reqAmount switch
        {
            3 => SumOfAllDice(lst),
            4 => SumOfAllDice(lst),
            5 => 50, // this option is for 'Yahtzee!' combination 
            _ => throw new ArgumentOutOfRangeException(nameof(reqAmount), reqAmount, null)
        };
    }

    private int GetFullHousePoints(List<int> lst)
    {
        var pairIndex = lst.IndexOf(2);
        var threeIndex = lst.IndexOf(3);
        var isFullHouse = pairIndex > 0 && threeIndex > 0;

        return !isFullHouse ? 0 : 25;
    }

    private int GetSmStraightPoints(List<int> lst)
    {
        return lst.Where((_, i) => i + 4 <= lst.Count && GetNotEmptyInRange(lst, i, i + 4) == 4).Any() ? 30 : 0;
    }

    private int GetLgStraightPoints(List<int> lst)
    {
        return lst.Where((_, i) => i + 5 <= lst.Count && GetNotEmptyInRange(lst, i, i + 5) == 5).Any() ? 40 : 0;
    }

    private int GetNotEmptyInRange(List<int> lst, int start, int end)
    {
        var notEmpty = 0;
        for (var i = start; i < end; i++)
        {
            if (lst[i] != 0)
                notEmpty++;
        }

        return notEmpty;
    }

    private int SumOfAllDice(List<int> lst)
    {
        return lst.Select((t, i) => (i + 1) * t).Sum();
    }
}