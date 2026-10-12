using System;
using System.Linq;
using System.Collections;
using Albatross.Expression.Documentation.Attributes;
using Albatross.Expression.Tokens;

namespace Albatross.Expression.Functions.Text
{
    [FunctionDoc(Documentation.Group.Text, "{token}( , )",
        "### Check if operand1 is not contained within operand2 or vice versa\n"+
        "#### Inputs:\n"+
        "- operand1: an array of values or a single value to be checked\n"+
        "- operand2: an array of values or a single value\n"+
        "#### Outputs:\n"+
        "- false if any value in operand1 is included in operand2, or any value in operand2 is included in operand1; true otherwise."
    )]
    [ParserOperation]
    public class NotIn : PrefixOperationToken
    {
        public override string Name => "NotIn";
        public override int MinOperandCount => 2;
        public override int MaxOperandCount => 2;
        public override bool Symbolic => false;

        public override object EvalValue(Func<string, object> context)
        {
            var operands = GetOperands(context);
            var operand1List = NormalizeToStringList(operands[0]);
            var operand2List = NormalizeToStringList(operands[1]);

            return !operand1List.Any(operand2List.Contains) && !operand2List.Any(operand1List.Contains);
        }

        private static string[] NormalizeToStringList(object operand)
        {
            if (operand is string str)
            {
                return str.Trim('[', ']').Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                          .Select(s => s.Trim())
                          .ToArray();
            }

            if (operand is IEnumerable enumerable)
                return enumerable.Cast<object>().Select(o => o?.ToString()?.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToArray();

            return new[] { operand?.ToString().Trim() };
        }
    }
}
