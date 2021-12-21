using System;
using System.Diagnostics.CodeAnalysis;

namespace Yahtzee.UI.Helpers;

public static class Throw
{
    [DoesNotReturn]
    public static void InvalidUiElementCast(object element, Type destType)
    {
        throw new InvalidCastException($"Invalid UI element case exception. Can't cast {element} to {destType}");
    }
}