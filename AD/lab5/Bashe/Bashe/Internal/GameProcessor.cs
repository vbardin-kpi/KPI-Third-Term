using Bashe.Abstractions;
using Bashe.Internal.Helpers;
using Bashe.Models;

namespace Bashe.Internal;

public class GameProcessor
{
    private static readonly Random Randomizer = new();

    private int _itemsLeft;
    private bool _aiStep;

    public void Execute(GameConfiguration config, IDifficultyStrategy strategy)
    {
        _itemsLeft = config.ItemsAmount;
        _aiStep = Randomizer.Next(0, 2) == 0;

        while (_itemsLeft > 0)
        {
            if (!_aiStep)
            {
                // user makes a step
                var userAmount = ConsoleHelper.GetUserInput(config, _itemsLeft);

                _itemsLeft -= userAmount;
                Console.WriteLine($"You take {userAmount} items. Items left: {_itemsLeft}");
                if (_itemsLeft == 0 || _itemsLeft < config.StepMin)
                {
                    Console.WriteLine("Congrats! You win!");
                    return;
                }
            }

            var aiAmount = strategy.GetItemsAmount(config, _itemsLeft);
            _itemsLeft -= aiAmount;
            Console.WriteLine($"AI takes: {aiAmount}. Items left {_itemsLeft}");
            if (_itemsLeft == 0 || _itemsLeft < config.StepMin)
            {
                Console.WriteLine("AI win!");
                return;
            }

            _aiStep = false;
        }
    }
}