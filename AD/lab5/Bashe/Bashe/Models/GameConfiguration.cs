namespace Bashe.Models;

public class GameConfiguration
{
    public int StepMin { get; init; }
    public int StepMax { get; init; }
    public int ItemsAmount { get; init; }
    
    public GameConfiguration()
    {
        StepMin = 1;
        StepMax = 3;
        ItemsAmount = 15;
    }

    public GameConfiguration(int stepMin, int stepMax, int itemsAmount)
    {
        StepMin = stepMin;
        StepMax = stepMax;
        ItemsAmount = itemsAmount;
    }
}