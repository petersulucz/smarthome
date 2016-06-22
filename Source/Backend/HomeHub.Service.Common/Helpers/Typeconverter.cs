namespace HomeHub.Service.Common.Helpers
{
    using System;

    using HomeHub.Common.Devices;
    using HomeHub.Common.Exceptions;

    /// <summary>
    /// The typeconverter.
    /// </summary>
    internal static class Typeconverter
    {
        public static object Convert(FunctionArgumentType argumentType, string argument)
        {
            switch (argumentType)
            {
                    case FunctionArgumentType.None:
                    if (false == String.IsNullOrWhiteSpace(argument))
                    {
                        throw new InvalidArgumentException($"The argument {argument} is invalid for type {argumentType}");
                    }
                    return null;

                    case FunctionArgumentType.Int:
                    {
                        int result;
                        if (false == int.TryParse(argument, out result))
                        {
                            throw new InvalidArgumentException($"The argument {argument} is invalid for type {argumentType}");
                        }

                        return result;
                    }

                    case FunctionArgumentType.Double:
                    {
                        double result;
                        if (false == double.TryParse(argument, out result))
                        {
                            throw new InvalidArgumentException($"The argument {argument} is invalid for type {argumentType}");
                        }

                        return result;
                    }

                default:
                    throw new NotImplementedException($"{argumentType} is not implemented");
            }
        }
    }
}
