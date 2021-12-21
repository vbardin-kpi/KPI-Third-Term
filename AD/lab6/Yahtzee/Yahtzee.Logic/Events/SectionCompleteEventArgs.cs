namespace Yahtzee.Logic.Events;

public class SectionCompleteEventArgs : EventArgs
{
    public int SectionNumber { get; init; }
    public int SectionTotal { get; init; }
    public int? SectionBonus { get; init; }
    public bool IsHuman { get; set; }
}