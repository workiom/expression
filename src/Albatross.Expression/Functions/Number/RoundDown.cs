﻿using Albatross.Expression.Documentation;
using Albatross.Expression.Documentation.Attributes;
using Albatross.Expression.Exceptions;
using Albatross.Expression.Tokens;
using System;
using System.Linq;

namespace Albatross.Expression.Functions.Number
{
    [FunctionDoc(Group.Number, "{token}( )",
@"### Returns larget integer less than or equal the decimal number.
#### Inputs:
- number: The decimal number to round.
#### Outputs:
- Integer."
    )]
    [ParserOperation]
    public class RoundDown : PrefixOperationToken
    {
        public override string Name { get { return "RoundDown"; } }
        public override int MinOperandCount { get { return 1; } }
        public override int MaxOperandCount { get { return 1; } }
        public override bool Symbolic { get { return false; } }

        public override object EvalValue(Func<string, object> context)
        {
            var input = Operands.Select(item => (decimal)Convert.ChangeType(item.EvalValue(context), typeof(decimal))).ToArray();

            if (input.Length == 1)
                return Math.Floor(input[0]);

            throw new UnexpectedTypeException();
        }
    }
}
