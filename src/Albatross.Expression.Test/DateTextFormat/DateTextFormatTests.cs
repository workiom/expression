using Albatross.Expression.Tokens;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.Expression.Test
{
    [TestFixture]
    [Category("DateTextFormat")]
    public class DateTextFormatTests
    {
        static readonly DateTextFormat WorkspaceFormat = new DateTextFormat
        {
            DateFormat = "dd/MM/yyyy",
            DateTimeFormat = "dd/MM/yyyy HH:mm",
        };

        static object Eval(string expression, DateTextFormat format)
        {
            var values = new Dictionary<string, object>
            {
                ["date"] = new DateTime(2025, 9, 2),
                ["dateTime"] = new DateTime(2025, 9, 5, 17, 30, 0),
            };
            IToken token = Factory.Instance.Create().Compile(expression);
            return token.EvalValue(name => values[name], format);
        }

        [TestCase("\"Date: \" + date", ExpectedResult = "Date: 02/09/2025")]
        [TestCase("date + \" is the date\"", ExpectedResult = "02/09/2025 is the date")]
        [TestCase("\"At: \" + dateTime", ExpectedResult = "At: 05/09/2025 17:30")]
        [TestCase("Text(date)", ExpectedResult = "02/09/2025")]
        [TestCase("Text(dateTime)", ExpectedResult = "05/09/2025 17:30")]
        [TestCase("Concat(\"From \", date, \" to \", dateTime)", ExpectedResult = "From 02/09/2025 to 05/09/2025 17:30")]
        [TestCase("\"N: \" + Day(date) + \" / \" + 1.5", ExpectedResult = "N: 2 / 1.5")]
        [TestCase("Text(5)", ExpectedResult = "5")]
        public object DatesBecomeTextInTheGivenFormat(string expression)
        {
            return Eval(expression, WorkspaceFormat);
        }

        [TestCase("Day(date)", ExpectedResult = 2D)]
        [TestCase("Year(date + 1)", ExpectedResult = 2025D)]
        public object DateValuesStayDates(string expression)
        {
            return Eval(expression, WorkspaceFormat);
        }

        [Test]
        public void DateTimeFormatIsUsedForADateWhenNoDateFormatIsGiven()
        {
            var format = new DateTextFormat { DateTimeFormat = "yyyy-MM-dd HH:mm" };

            Assert.AreEqual("2025-09-02 00:00", Eval("Text(date)", format));
        }

        [Test]
        public void DateFormatIsUsedForADateTimeWhenNoDateTimeFormatIsGiven()
        {
            var format = new DateTextFormat { DateFormat = "yyyy-MM-dd" };

            Assert.AreEqual("2025-09-05", Eval("Text(dateTime)", format));
        }

        [TestCase("\"Date: \" + date")]
        [TestCase("Text(dateTime)")]
        [TestCase("Concat(\"From \", date, \" to \", dateTime)")]
        public void WithoutAFormatDatesBecomeTextAsBefore(string expression)
        {
            var values = new Dictionary<string, object>
            {
                ["date"] = new DateTime(2025, 9, 2),
                ["dateTime"] = new DateTime(2025, 9, 5, 17, 30, 0),
            };
            IToken token = Factory.Instance.Create().Compile(expression);

            var before = token.EvalValue(name => values[name]);

            Assert.AreEqual(before, token.EvalValue(name => values[name], null));
            Assert.AreEqual(before, Eval(expression, new DateTextFormat()));
            StringAssert.Contains(new DateTime(2025, 9, 2).ToString(CultureInfo.CurrentCulture), Convert.ToString(Eval("Text(date)", null)));
        }

        [Test]
        public void TheFormatOnlyAppliesToItsOwnCalculation()
        {
            IToken token = Factory.Instance.Create().Compile("Text(date)");
            var date = new DateTime(2025, 9, 2);

            using (var entered = new ManualResetEventSlim())
            using (var otherDone = new ManualResetEventSlim())
            {
                var other = Task.Run(() =>
                {
                    entered.Wait();
                    var value = token.EvalValue(_ => date);
                    otherDone.Set();
                    return value;
                });
                var result = token.EvalValue(name =>
                {
                    entered.Set();
                    otherDone.Wait();
                    return date;
                }, WorkspaceFormat);

                Assert.AreEqual("02/09/2025", result);
                Assert.AreEqual(Convert.ToString(date), other.Result, "Another calculation must not see this calculation's format");
            }

            Assert.AreEqual(Convert.ToString(date), token.EvalValue(_ => date), "The format must not outlive its calculation");
        }

        [Test]
        public void ANestedCalculationKeepsTheOuterFormatAfterwards()
        {
            IToken outer = Factory.Instance.Create().Compile("Text(date) + \" | \" + inner");
            IToken inner = Factory.Instance.Create().Compile("Text(date)");
            var date = new DateTime(2025, 9, 2);
            var iso = new DateTextFormat { DateFormat = "yyyy-MM-dd" };

            var result = outer.EvalValue(name => name == "inner" ? inner.EvalValue(_ => date, iso) : (object)date, WorkspaceFormat);

            Assert.AreEqual("02/09/2025 | 2025-09-02", result);
            Assert.AreEqual("02/09/2025 | 02/09/2025", outer.EvalValue(name => name == "inner" ? inner.EvalValue(_ => date) : (object)date, WorkspaceFormat));
        }
    }
}
