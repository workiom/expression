using Albatross.Expression.Documentation;
using Albatross.Expression.Documentation.Attributes;
using Albatross.Expression.Tokens;
using System;
using System.Linq;

namespace Albatross.Expression.Functions.Number
{
    [FunctionDoc(Group.Number, "{token}( , , )",
        @"### Check if a number or date is between two values

#### Inputs:
- @value: The number or date to check.
- @min: The minimum bound (number or date).
- @max: The maximum bound (number or date).

#### Behavior:
- Compares `value` against the `min` and `max` bounds.
- Returns `true` if `value` is between the bounds (inclusive).
- Automatically swaps bounds if `min > max`.
- Supports **either all numbers** or **all date values**.
- Throws an error if types are mixed or unrecognized.

#### Examples:
- `IsBetween(5, 1, 10)` → `true`
- `IsBetween(5, 10, 1)` → `true` (bounds swapped)
- `IsBetween('2025-09-22', '2025-09-20', '2025-09-25')` → `true`
- `IsBetween('2025-09-26', '2025-09-25', '2025-09-20')` → `false` (bounds swapped)"
    )]

    [ParserOperation]
    public class IsBetween : PrefixOperationToken
    {
        public override string Name => "IsBetween";
        public override int MinOperandCount => 3;
        public override int MaxOperandCount => 3;
        public override bool Symbolic => false;

        public override object EvalValue(Func<string, object> context)
        {
            var operands = Operands.Select(o => o.EvalValue(context)).ToArray();

            if (operands.Length != 3)
                throw new ArgumentException("IsBetween function requires exactly three operands.");

            if (operands.All(o => DateTime.TryParse(o.ToString(), out _)))
            {
                var dates = operands.Select(o => DateTime.Parse(o.ToString())).ToArray();
                var value = dates[0].Date;
                var min = dates[1].Date < dates[2].Date ? dates[1].Date : dates[2].Date;
                var max = dates[1].Date > dates[2].Date ? dates[1].Date : dates[2].Date;
                return value >= min && value <= max;
            }

            if (operands.All(o => o is double))
            {
                var numbers = operands.Cast<double>().ToArray();
                var value = numbers[0];
                var min = Math.Min(numbers[1], numbers[2]);
                var max = Math.Max(numbers[1], numbers[2]);
                return value >= min && value <= max;
            }

            throw new ArgumentException("Operands must be either all doubles or all parsable dates.");
        }
    }
}
