using Bashe.Models;

namespace Bashe.Abstractions;

public interface IBasheProcessor
{
    void Start(GameConfiguration config, BasheDifficulty difficulty = BasheDifficulty.Medium);
}
