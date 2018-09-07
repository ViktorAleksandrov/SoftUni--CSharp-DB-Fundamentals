using System;

namespace TeamBuilder.App.Utilities
{
    public static class Check
    {
        public static void CheckLength(int expectingLength, string[] array)
        {
            if (expectingLength != array.Length)
            {
                throw new FormatException(Constants.ErrorMessages.InvalidArgumentsCount);
            }
        }
    }
}
