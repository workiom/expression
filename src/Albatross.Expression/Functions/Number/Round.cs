﻿using Albatross.Expression.Exceptions;
using Albatross.Expression.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albatross.Expression.Functions.Number
{
    [ParserOperation]
    public class Round : PrefixOperationToken
    {
        public override string Name { get { return "Round"; } }
        public override int MinOperandCount { get { return 1; } }
        public override int MaxOperandCount { get { return 1; } }
        public override bool Symbolic { get { return false; } }

        public override object EvalValue(Func<string, object> context)
        {
            var input = Operands.Select(item => (decimal)Convert.ChangeType(item.EvalValue(context), typeof(decimal))).ToArray();

            if (input.Length == 1)
                return Math.Round(input[0]);

            if (input.Length == 2)
                return Math.Round(input[0], (int)input[1]);

            throw new UnexpectedTypeException();
        }
    }
}
