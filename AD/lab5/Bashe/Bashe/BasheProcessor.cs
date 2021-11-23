using Bashe.Abstractions;
using Bashe.Internal;
using Bashe.Internal.Strategies;
using Bashe.Models;

namespace Bashe;

public class BasheProcessor : IBasheProcessor
{
    public void Start(GameConfiguration config, BasheDifficulty difficulty = BasheDifficulty.Easy)
    {
        IDifficultyStrategy handler = difficulty switch
        {
            BasheDifficulty.Easy => new EasyDifficultyStrategy(),
            BasheDifficulty.Medium => new MediumDifficultyStrategy(),
            BasheDifficulty.Hard => new HardDifficultyStrategy(),
            _ => throw new NotImplementedException()
        };

        new GameProcessor().Execute(config, handler);
    }
}
