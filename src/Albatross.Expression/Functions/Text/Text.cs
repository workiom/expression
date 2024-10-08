﻿using Albatross.Expression.Documentation;
using Albatross.Expression.Documentation.Attributes;
using Albatross.Expression.Tokens;
using System;
using System.Linq;

namespace Albatross.Expression.Operations
{
    [FunctionDoc(Group.Text, "{token}( )",
@"### Transforms any object into a text.
#### Inputs:
- object: Any
#### Outputs:
- String"
    )]
    [ParserOperation]
    public class Text : PrefixOperationToken
    {

        public override string Name { get { return "Text"; } }
        public override int MinOperandCount { get { return 1; } }
        public override int MaxOperandCount { get { return 1; } }
        public override bool Symbolic { get { return false; } }

        public override object EvalValue(Func<string, object> context)
        {
            object value = Operands.First().EvalValue(context);
            return Convert.ToString(value);
        }
    }
}
