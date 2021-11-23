using Bashe.Abstractions;
using Bashe.Models;

namespace Bashe.Internal.Strategies;

public class EasyDifficultyStrategy : IDifficultyStrategy
{
    public int GetItemsAmount(GameConfiguration config, int itemsLeft)
    {
        var maxAmount = config.StepMax > itemsLeft ? itemsLeft : config.StepMax;
        for (var i = maxAmount; i >= config.StepMin; i--)
        {
            if (config.StepMax % i == 0)
                return i;
        }

        return config.StepMin;
    }
}