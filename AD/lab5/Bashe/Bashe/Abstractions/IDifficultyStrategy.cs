using Bashe.Models;

namespace Bashe.Abstractions;
 
public interface IDifficultyStrategy
{
    int GetItemsAmount(GameConfiguration config, int itemsLeft);
}