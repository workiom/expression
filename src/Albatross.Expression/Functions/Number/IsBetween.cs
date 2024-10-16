using Albatross.Expression.Documentation;
using Albatross.Expression.Documentation.Attributes;
using Albatross.Expression.Tokens;
using System;
using System.Linq;

namespace Albatross.Expression.Functions.Number
{
    [FunctionDoc(Group.Number, "{token}( , , )",
        @"### Check if a number is between two values
#### Inputs:
- @val1: Minimum value of the range
- @val2: Maximum value of the range
- @number: Number to check if it is in the range
#### Outputs:
- True if the number is between the two values (inclusive), otherwise false."
    )]
    [ParserOperation]
    public class IsBetween : PrefixOperationToken
    {
        public override string Name { get { return "IsBetween"; } }
        public override int MinOperandCount { get { return 3; } }
        public override int MaxOperandCount { get { return 3; } }
        public override bool Symbolic { get { return false; } }

        public override object EvalValue(Func<string, object> context)
        {
            var input = Operands.Select(item => (int)Convert.ChangeType(item.EvalValue(context), typeof(int))).ToArray();
            if (input.Length != 3)
            {
                throw new ArgumentException("IsBetween function requires exactly three operands.");
            }

            var numberToCheck = input[0];
            var minValue = input[1];
            var maxValue = input[2];

            return minValue <= numberToCheck && numberToCheck <= maxValue;
        }
    }
}