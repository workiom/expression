﻿using Albatross.Expression.Documentation;
using Albatross.Expression.Documentation.Attributes;
using Albatross.Expression.Tokens;
using System;

namespace Albatross.Expression.Functions.Date
{
    [FunctionDoc(Group.Date, "{token}( , )",
@"### Subtracts the given date values to return the number of days between them.
#### Inputs:
- date1: date
- date2: date
#### Outputs:
- The number of days between the two dates."
    )]
    [ParserOperation]
    public class SubtractDate : PrefixOperationToken
    {
        public override string Name { get { return "SubtractDate"; } }
        public override int MinOperandCount { get { return 2; } }
        public override int MaxOperandCount { get { return 2; } }
        public override bool Symbolic { get { return false; } }

        public override object EvalValue(Func<string, object> context)
        {
            DateTime date1 = (DateTime)Convert.ChangeType(Operands[0].EvalValue(context), typeof(DateTime));
            DateTime date2 = (DateTime)Convert.ChangeType(Operands[1].EvalValue(context), typeof(DateTime));

            var result = date1.Subtract(date2).TotalDays;
            return result;
        }
    }
}
