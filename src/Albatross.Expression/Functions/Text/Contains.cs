using Albatross.Expression.Documentation.Attributes;
using Albatross.Expression.Tokens;
using System;

namespace Albatross.Expression.Functions.Text
{
    /// <summary>
    /// <para>Prefix operation that will take an input and C# format string and produced a formatted string</para>
    /// 
    /// </summary>
    [FunctionDoc(Documentation.Group.Text, "{token}(text, substring)",
        "### Checks if the `substring` is present in the given `text`\n" +
        "#### Inputs:\n" +
        "- **text**: The full string in which to search\n" +
        "- **substring**: The string to look for within `text`\n" +
        "#### Output:\n" +
        "- `true` if `substring` is found inside `text` (case-insensitive), `false` otherwise."
    )]
    [ParserOperation]
    public class Contains : PrefixOperationToken
    {

        public override string Name => "Contains";
        public override int MinOperandCount => 2;
        public override int MaxOperandCount => 2;
        public override bool Symbolic => false;


        public override object EvalValue(Func<string, object> context)
        {
            var operands = GetOperands(context);

            if (operands.Count != 2)
                throw new InvalidOperationException("Contains requires exactly 2 operands.");

            var operand1 = operands[0]?.ToString();
            var operand2 = operands[1]?.ToString();

            if (string.IsNullOrEmpty(operand1) || string.IsNullOrEmpty(operand2))
                return false;

            return operand1.ToLower().Contains(operand2.ToLower());
        }
    }
}