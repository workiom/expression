using Albatross.Expression.Documentation.Attributes;
using Albatross.Expression.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.Expression.Operations
{
    [FunctionDoc(
        Documentation.Group.Text,
        "{token}( , , )",
        @"### Format
#### Inputs:
- data: Object
- format: String
- extra (optional): Any value (ignored)

#### Outputs:
- formatted string.

The third parameter is optional and does not affect the formatting behavior."
    )]
    [ParserOperation]
    public class Format : PrefixOperationToken
    {
        public override string Name => "Format";

        public override int MinOperandCount => 2;
        public override int MaxOperandCount => 3;

        public override bool Symbolic => false;

        public override object EvalValue(Func<string, object> context)
        {
            List<object> list = GetOperands(context);

            string format = "{0:" + list[1] + "}";
            var result = string.Format(format, list[0]);

            return result;
        }
    }
}