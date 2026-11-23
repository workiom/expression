using System;
using System.Globalization;
using System.Threading;

namespace Albatross.Expression
{
    /// <summary>
    /// How Plus (+), Concat and Text turn a date into text during one calculation.
    /// Pass it with <see cref="Extensions.EvalValue(Tokens.IToken, Func{string, object}, DateTextFormat)"/>.
    /// Without one, dates become text as before (the current culture's "G" format).
    /// <para>The patterns are .NET custom date and time format strings (e.g. "dd/MM/yyyy HH:mm"), not moment-style ones.</para>
    /// <para>A date is written as given: convert values to the time zone they should show in before the calculation.
    /// A value at exactly 00:00:00 counts as a date with no time of day, whatever field it came from.</para>
    /// <para>A calculation started inside another one without its own format uses the outer one.</para>
    /// </summary>
    public sealed class DateTextFormat
    {
        private static readonly AsyncLocal<DateTextFormat> current = new AsyncLocal<DateTextFormat>();

        /// <param name="dateFormat">For a date with no time of day, e.g. "dd/MM/yyyy". Null: <paramref name="dateTimeFormat"/>.</param>
        /// <param name="dateTimeFormat">For a date with a time of day, e.g. "dd/MM/yyyy HH:mm". Null: <paramref name="dateFormat"/>.</param>
        /// <param name="formatProvider">Culture for names and separators. Null: the invariant culture.</param>
        public DateTextFormat(string dateFormat, string dateTimeFormat, IFormatProvider formatProvider = null)
        {
            DateFormat = dateFormat;
            DateTimeFormat = dateTimeFormat;
            FormatProvider = formatProvider ?? CultureInfo.InvariantCulture;
        }

        public string DateFormat { get; }

        public string DateTimeFormat { get; }

        public IFormatProvider FormatProvider { get; }

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
