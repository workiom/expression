using System;
using System.Threading;

namespace Albatross.Expression
{
    public static class ExpressionMode
    {
        private static readonly AsyncLocal<bool> isValidationMode = new AsyncLocal<bool>();

        public static bool IsValidationMode => isValidationMode.Value;

        public static IDisposable BeginValidationMode()
        {
            var previous = isValidationMode.Value;
            isValidationMode.Value = true;

            return new DisposeAction(() => isValidationMode.Value = previous);
        }
    }
}
