using System.Diagnostics.CodeAnalysis;

namespace Yahtzee.Logic.Helpers;

public class Throw
{
    [DoesNotReturn]
    public void ImpossibleStepException(YahtzeeStep step) =>
        throw new InvalidOperationException($"Step {step} was already dome!");
}