using System;
using System.Linq;
using Albatross.Expression.Documentation.Attributes;
using Albatross.Expression.Tokens;
using System.Collections;

namespace Albatross.Expression.Functions.Text
{
    [FunctionDoc(Documentation.Group.Text, "{token}( , )",
        "### Check if operand1 exists in operand2 or vice versa\n" +
        "#### Inputs:\n" +
        "- operand1: a single value or an array of values to be checked\n" +
        "- operand2: a single value or an array of values\n" +
        "#### Outputs:\n" +
        "- true if **any** of the values in operand1 are included in operand2, or any of operand2 are in operand1, false otherwise."
    )]
    [ParserOperation]
    public class In : PrefixOperationToken
    {
        public override string Name => "In";
        public override int MinOperandCount => 2;
        public override int MaxOperandCount => 2;
        public override bool Symbolic => false;

        public override object EvalValue(Func<string, object> context)
        {
            var operands = GetOperands(context);
            var operand1List = NormalizeToStringList(operands[0]);
            var operand2List = NormalizeToStringList(operands[1]);

            return operand1List.Any(operand2List.Contains) || operand2List.Any(operand1List.Contains);
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
