﻿using Albatross.Expression.Documentation;
using Albatross.Expression.Documentation.Attributes;
using Albatross.Expression.Tokens;
using System;

namespace Albatross.Expression.Operations
{
    [FunctionDoc(Group.Date, "{token}()",
@"### Returns the date and time of current moment in the tenant time zone.
#### Inputs:
- No inputs
#### Outputs:
- Date with time."
    )]
    [ParserOperation]
    public class Now : PrefixOperationToken
    {
        public override string Name { get { return "Now"; } }
        public override int MinOperandCount { get { return 0; } }
        public override int MaxOperandCount { get { return 0; } }
        public override bool Symbolic { get { return false; } }

        public override object EvalValue(Func<string, object> context)
        {
            var now = DateTime.Now;

            if (Config.NowFunction.DateTimeKind == DateTimeKind.Utc)
                now = DateTime.UtcNow;

            if (Config.NowFunction.Normalizer != null)
                now = Config.NowFunction.Normalizer(now);

            var hour = now.Hour;
            var hourInterval = Config.NowFunction.HourInterval;
            if (hourInterval <= 0)
                hour = 0;
            else
                hour = (hour / hourInterval) * hourInterval;

            var minute = now.Minute;
            var minuteInterval = Config.NowFunction.MinuteInterval;
            if (minuteInterval <= 0)
                minute = 0;
            else
                minute = (minute / minuteInterval) * minuteInterval;

            var second = now.Second;
            var secondInterval = Config.NowFunction.SecoundInterval;
            if (secondInterval <= 0)
                second = 0;
            else
                second = (second / secondInterval) * secondInterval;

            return DateTime.SpecifyKind(new DateTime(now.Year, now.Month, now.Day, hour, minute, second), now.Kind);
        }
    }
}
