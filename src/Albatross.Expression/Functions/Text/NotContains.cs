using Albatross.Expression.Documentation.Attributes;
using Albatross.Expression.Tokens;
using System;

namespace Albatross.Expression.Functions.Text
{
    [FunctionDoc(Documentation.Group.Text, "{token}(text, substring)",
        "### Checks if the `substring` is **not** present in the given `text`\n" +
        "#### Inputs:\n" +
        "- **text**: The full string in which to search\n" +
        "- **substring**: The string to look for within `text`\n" +
        "#### Output:\n" +
        "- `true` if `substring` is **not** found inside `text` (case-insensitive), `false` otherwise."
    )]
    [ParserOperation]
    public class NotContains : PrefixOperationToken
    {
        public override string Name => "NotContains";
        public override int MinOperandCount => 2;
        public override int MaxOperandCount => 2;
        public override bool Symbolic => false;

        public override object EvalValue(Func<string, object> context)
        {
            var operands = GetOperands(context);

            if (operands.Count != 2)
                throw new InvalidOperationException("NotContains requires exactly 2 operands.");

            var operand1 = operands[0]?.ToString();
            var operand2 = operands[1]?.ToString();

            if (string.IsNullOrEmpty(operand1) || string.IsNullOrEmpty(operand2))
                return true;

            return !operand1.ToLower().Contains(operand2.ToLower());
        }
    }
}