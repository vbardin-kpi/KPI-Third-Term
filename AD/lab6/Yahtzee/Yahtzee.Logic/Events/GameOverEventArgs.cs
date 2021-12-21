namespace Yahtzee.Logic.Events;

public class GameOverEventArgs : EventArgs
{
    public bool IsHumanAWinner { get; set; }
}