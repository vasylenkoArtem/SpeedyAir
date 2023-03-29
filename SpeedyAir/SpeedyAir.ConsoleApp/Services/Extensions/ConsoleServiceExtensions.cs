using SpeedyAir.ConsoleApp.Exceptions;

namespace SpeedyAir.ConsoleApp.Services.Extensions;

public static class ConsoleServiceExtensions
{
    public static void ThrowInputValidationError(string fieldName = null)
    {
        var fieldNameText = !string.IsNullOrEmpty(fieldName) ? $" in the {fieldName} field" : string.Empty;

        throw new ConsoleAppLogicException($"Wrong input{fieldNameText}");
    }
}