using Bashe.Models;

namespace Bashe;

internal static class Program
{
    private static void Main()
    {
        new BasheProcessor()
            .Start(new GameConfiguration(1, 3, 15), BasheDifficulty.Medium);
    }
}