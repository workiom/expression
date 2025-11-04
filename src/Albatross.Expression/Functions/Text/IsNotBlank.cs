using Albatross.Expression.Documentation;
using Albatross.Expression.Documentation.Attributes;
using Albatross.Expression.Helpers;
using Albatross.Expression.Tokens;
using System;
using System.Linq;

namespace Albatross.Expression.Functions.Text
{
    /// <summary>
    /// <para>Prefix operation to check if the input IsNotBlank</para>
    /// <para>Operand Type: any</para>
    /// <para>Returns true if the value is NOT null, NOT an empty string, and NOT whitespace-only</para>
    /// </summary>
    [FunctionDoc(Group.Text, "{token}( )",
        @"### Returns boolean value, true if the value is **not** blank.  
A blank is defined as null, empty string, or string with only white space.

#### Inputs:
- value: String

#### Outputs:
- Boolean"
    )]
    [ParserOperation]
    public class IsNotBlank : PrefixOperationToken
    {
        public override string Name => "IsNotBlank";
        public override int MinOperandCount => 1;
        public override int MaxOperandCount => 1;
        public override bool Symbolic => false;

        public override object EvalValue(Func<string, object> context)
        {
            var value = GetOperands(context).First();
            return BlankHelper.IsNotBlank(value);
        }
    }
}