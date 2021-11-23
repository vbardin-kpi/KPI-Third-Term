using Bashe.Models;

namespace Bashe.Internal.Helpers;

internal static class ConsoleHelper
{
    internal static int GetUserInput(GameConfiguration config, int itemsLeft)
    {
        Console.WriteLine("It's your step now.");
        do
        {
            Console.Write($"Please enter a number from {config.StepMin} to {config.StepMax}: ");

            var isNumber = int.TryParse(Console.ReadLine(), out var userInput);
            if (!isNumber) continue;

            var numNotInRange = !(userInput >= config.StepMin && userInput <= config.StepMax);
            if (numNotInRange)
            {
                Console.WriteLine($"Your number should be in range: [{config.StepMin}; {config.StepMax}]");
                continue;
            }

            var userInputValid = itemsLeft >= userInput;
            if (userInputValid) return userInput;
                
            Console.WriteLine("Sorry, you can't take more items that left. " +
                              $"Left {itemsLeft} {(itemsLeft != 1 ? "items" : "item")}.");
        } while (true);
    }
}