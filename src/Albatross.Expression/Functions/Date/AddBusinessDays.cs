using Albatross.Expression.Documentation;
using Albatross.Expression.Documentation.Attributes;
using Albatross.Expression.Tokens;
using System;
using System.Collections.Generic;

namespace Albatross.Expression.Functions.Date
{
    [FunctionDoc(Group.Date, "{token}( , )",
@"### Add or Subtract Business Days
#### Inputs:
- `date`: Date (mandatory) - The starting date.
- `count`: Number (mandatory) - Number of business days to add (positive for future, negative for past).
- `startOfWeek`: Start of the week as a value from 1 (Monday) to 7 (Sunday) (Optional, default: 1).
- `weekendLength`: Integer (1 or 2) indicating the length of the weekend (Optional, default: 2).
#### Outputs:
- A calculated `DateTime`, adjusted by the number of business days."
    )]
    [ParserOperation]
    public class AddBusinessDays : PrefixOperationToken
    {
        public override string Name => "AddBusinessDays";

        public override bool Symbolic => false;

        public override int MinOperandCount => 2;

        public override int MaxOperandCount => 4;

        public override object EvalValue(Func<string, object> context)
        {
            var date = (DateTime)Convert.ChangeType(Operands[0].EvalValue(context), typeof(DateTime));
            var count = (int)Convert.ChangeType(Operands[1].EvalValue(context), typeof(int));

            var startOfWeek = DayOfWeek.Monday;
            if (Operands.Count > 2)
                startOfWeek = (DayOfWeek)(((int)Convert.ChangeType(Operands[2].EvalValue(context), typeof(int)) - 1) % 7);

            var weekendLength = 2;
            if (Operands.Count > 3)
                weekendLength = (int)Convert.ChangeType(Operands[3].EvalValue(context), typeof(int));

            var adjustedDate = AdjustBusinessDays(date, count, startOfWeek, weekendLength);
            return adjustedDate.ToString("yyyy-MM-dd");
        }

        private DateTime AdjustBusinessDays(DateTime date, int count, DayOfWeek startOfWeek, int weekendLength)
        {
            var weekendDays = CalculateWeekendDays(startOfWeek, weekendLength);

            var step = count > 0 ? 1 : -1;
            var daysMoved = 0;

            if (!weekendDays.Contains(date.DayOfWeek))
                daysMoved++;

            while (daysMoved < Math.Abs(count))
            {
                date = date.AddDays(step);

                if (!weekendDays.Contains(date.DayOfWeek))
                    daysMoved++;
            }

            return date;
        }

        private HashSet<DayOfWeek> CalculateWeekendDays(DayOfWeek startOfWeek, int weekendLength)
        {
            var weekendDays = new HashSet<DayOfWeek>();

            var endOfWeek = ((int)startOfWeek + 7 - 1) % 7; 
            for (var i = 0; i < weekendLength; i++)
            {
                var weekendDayIndex = (endOfWeek - i + 7) % 7; 
                weekendDays.Add((DayOfWeek)weekendDayIndex);
            }

            return weekendDays;
        }
    }
}