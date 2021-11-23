using Bashe.Abstractions;
using Bashe.Models;

namespace Bashe.Internal.Strategies;

public class HardDifficultyStrategy : IDifficultyStrategy
{
    public int GetItemsAmount(GameConfiguration config, int itemsLeft)
    {
        var maxAmount = config.StepMax > itemsLeft ? itemsLeft : config.StepMax;
        for (var i = maxAmount; i >= config.StepMin; i--)
        {
            if ((itemsLeft - i) % (config.StepMax + 1) == 0)
                return i;
        }

        return config.StepMin;
    }
}