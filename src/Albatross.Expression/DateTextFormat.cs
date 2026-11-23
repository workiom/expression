using System;
using System.Globalization;
using System.Threading;

namespace Albatross.Expression
{
    /// <summary>
    /// How Plus (+), Concat and Text turn a date into text during one calculation.
    /// Pass it with <see cref="Extensions.EvalValue(Tokens.IToken, Func{string, object}, DateTextFormat)"/>.
    /// Without one, dates become text as before (the current culture's "G" format).
    /// </summary>
    public sealed class DateTextFormat
    {
        private static readonly AsyncLocal<DateTextFormat> current = new AsyncLocal<DateTextFormat>();

        /// <summary>Format for a date with no time of day (00:00:00), e.g. "dd/MM/yyyy". Falls back to <see cref="DateTimeFormat"/>.</summary>
        public string DateFormat { get; set; }

        /// <summary>Format for a date with a time of day, e.g. "dd/MM/yyyy HH:mm". Falls back to <see cref="DateFormat"/>.</summary>
        public string DateTimeFormat { get; set; }

        public IFormatProvider FormatProvider { get; set; } = CultureInfo.InvariantCulture;

        internal static IDisposable Use(DateTextFormat format)
        {
            var previous = current.Value;
            current.Value = format;

            return new DisposeAction(() => current.Value = previous);
        }

        /// <summary>The value as Plus, Concat and Text should write it: a date in the calculation's format, anything else unchanged.</summary>
        internal static object ForText(object value)
        {
            var format = current.Value;
            if (format == null || !(value is DateTime date)) { return value; }

            var pattern = date.TimeOfDay == TimeSpan.Zero
                ? format.DateFormat ?? format.DateTimeFormat
                : format.DateTimeFormat ?? format.DateFormat;

            return pattern == null ? value : date.ToString(pattern, format.FormatProvider);
        }
    }
}
