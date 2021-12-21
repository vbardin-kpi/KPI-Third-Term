namespace Yahtzee.Logic.Events;

public class PcStepDoneEventArgs : EventArgs
{
    public YahtzeeStep YahtzeeStep { get; init; }
    public int Points { get; init; }
    public int? YahtzeeBonus { get; init; }
}