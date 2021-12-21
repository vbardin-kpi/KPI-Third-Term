namespace Yahtzee.Logic.Models;

public class GameProgressTracking
{
    public bool IsFirstSectionComplete { get; set; }
    public bool IsSecondSectionComplete { get; set; }
    public bool IsAllCompleted => IsFirstSectionComplete && IsSecondSectionComplete;

    public Dictionary<YahtzeeStep, int?> Steps { get; }

    public GameProgressTracking()
    {
        IsFirstSectionComplete = false;
        IsSecondSectionComplete = false;

        Steps = new Dictionary<YahtzeeStep, int?>();
        
        Enum.GetValues<YahtzeeStep>().ToList().ForEach(x =>
        {
            Steps.Add(x, null);
        });
    }
}