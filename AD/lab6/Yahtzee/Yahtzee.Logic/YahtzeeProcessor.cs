using Yahtzee.Logic.Events;
using Yahtzee.Logic.Helpers;
using Yahtzee.Logic.MiniMax;
using Yahtzee.Logic.Models;

namespace Yahtzee.Logic;

public class YahtzeeProcessor
{
    private static readonly Random Randomizer = new();

    private readonly GameProgressTracking _userProgressTracker;
    private readonly GameProgressTracking _pcProgressTracker;

    private readonly PointsCounterProcessor _pointsCountProcessor = new();
    private readonly MiniMaxProcessor _miniMaxProcessor = new();

    public readonly List<Die> Dice;

    private int[] DiceValuesArray => Dice.Select(x => x.Value).ToArray();

    private int _stepRolls = 3;
    public event EventHandler<PcStepDoneEventArgs> PcStepDone;
    public event EventHandler<SectionCompleteEventArgs> SectionComplete;
    public event EventHandler<GameOverEventArgs> GameOver;

    public YahtzeeProcessor()
    {
        Dice = new List<Die>(5)
        {
            new(), new(), new(), new(), new()
        };

        _userProgressTracker = new GameProgressTracking();
        _pcProgressTracker = new GameProgressTracking();
    }

    public int[] RollDice()
    {
        Dice.ForEach(die =>
        {
            if (!die.IsHold)
                die.Value = Randomizer.Next(1, 7);
        });

        return DiceValuesArray;
    }

    public int Step(YahtzeeStep yahtzeeStep)
    {
        var arr = new List<int>(6);
        for (var i = 0; i < 6; i++)
        {
            arr.Add(Dice.Count(x => x.Value == i + 1));
        }

        Dice.ForEach(x => x.IsHold = false);

        var points = yahtzeeStep.IsFirstSection()
            ? _pointsCountProcessor.CountFirstSection(arr, yahtzeeStep)
            : _pointsCountProcessor.CountSecondSection(arr, yahtzeeStep);

        _userProgressTracker.Steps[yahtzeeStep] = points;

        NotifyIfSectionComplete(true);

        return points;
    }

    private void NotifyIfSectionComplete(bool isHuman)
    {
        var progressTracker = isHuman ? _userProgressTracker : _pcProgressTracker;
        List<YahtzeeStep> requiredCombinations = new();
        var validatingSectionNumber = 0;

        if (!progressTracker.IsFirstSectionComplete)
        {
            validatingSectionNumber = 1;
            requiredCombinations = YahtzeeStepHelper.GetFirstSectionCombinations();
        }
        else if (!progressTracker.IsSecondSectionComplete)
        {
            validatingSectionNumber = 2;
            requiredCombinations = YahtzeeStepHelper.GetSecondSectionCombinations();
        }

        var sectionComplete = requiredCombinations.Any() &&
                              requiredCombinations.All(x => progressTracker.Steps[x] != null);
        if (!sectionComplete) return;

        switch (validatingSectionNumber)
        {
            case 1:
                progressTracker.IsFirstSectionComplete = true;
                break;
            case 2:
                progressTracker.IsSecondSectionComplete = true;
                break;
        }

        var (points, bonus) = _pointsCountProcessor
            .CountSectionTotal(validatingSectionNumber, progressTracker);

        SectionComplete.Invoke(this, new SectionCompleteEventArgs
        {
            SectionNumber = validatingSectionNumber,
            SectionTotal = points,
            SectionBonus = bonus,
            IsHuman = isHuman
        });
    }

    #region Pc

    public void PcStep()
    {
        YahtzeeStep? combination;
        do
        {
            RollDice();
            combination = GetNextCombination(!_pcProgressTracker.IsFirstSectionComplete);
            var valToHold = (int) combination + 1;

            Dice.ForEach(x =>
            {
                if (x.Value == valToHold)
                    x.IsHold = true;
            });

            _stepRolls--;
        } while (_stepRolls > 0);

        var points = combination.Value.IsFirstSection()
            ? _pointsCountProcessor.CountFirstSection(GetDiceFrequent(), combination.Value)
            : _pointsCountProcessor.CountSecondSection(GetDiceFrequent(), combination.Value);

        ResetInternalStateTrackers();

        _pcProgressTracker.Steps[combination.Value] = points;
        NotifyIfSectionComplete(false);

        PcStepDone.Invoke(this, new PcStepDoneEventArgs
        {
            Points = points,
            YahtzeeStep = combination.Value,
            YahtzeeBonus = combination.Value is YahtzeeStep.Yahtzee && points != 0 ? 100 : 0
        });

        if (_pcProgressTracker.IsAllCompleted)
        {
            GameOver.Invoke(this, new GameOverEventArgs());
        }
    }

    private YahtzeeStep GetNextCombination(bool isFirstSection)
    {
        if (isFirstSection)
        {
            var diceOrderedByAmount = GetDiceFrequent()
                .Select((amount, index) => new KeyValuePair<int, int>(amount, index))
                .OrderByDescending(x => x.Key)
                .Where(x =>
                {
                    var c = (YahtzeeStep) x.Value;
                    return _pcProgressTracker.Steps[c] is null;
                });
            var (_, mostPopularDieIndex) = diceOrderedByAmount.First();
            return (YahtzeeStep) mostPopularDieIndex;
        }

        var availableCombinations = YahtzeeStepHelper.GetSecondSectionCombinations()
            .Where(x => _pcProgressTracker.Steps[x] is null)
            .Select(x => x)
            .ToList();

        var holdIndexes = new List<int>();
        for (var i = 0; i < Dice.Count; i++)
        {
            if (Dice[i].IsHold)
            {
                holdIndexes.Add(i);
            }
        }

        var startNode = new MiniMaxNode(availableCombinations, GetDiceFrequent(), holdIndexes);

        var bestMiniMaxNode = _miniMaxProcessor.GetBestNode(2, startNode, true);
        bestMiniMaxNode.DiceIndexesToHold.ForEach(x => Dice[x].IsHold = true);

        return bestMiniMaxNode.TargetCombination!.Value;
    }

    #endregion

    #region Common/Utils

    private List<int> GetDiceFrequent()
    {
        var frequentList = new List<int>(6);
        for (var i = 0; i < 6; i++)
        {
            frequentList.Add(Dice.Count(x => x.Value == i + 1));
        }

        return frequentList;
    }

    private void ResetInternalStateTrackers()
    {
        _stepRolls = 3;
        Dice.ForEach(x => x.IsHold = false);
    }

    #endregion
}