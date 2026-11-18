using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.Expression.Test
{
    [TestFixture]
    [Category("Validation")]
    public class ValidationModeTests
    {
        [TestCase("Number(\"44\")", ExpectedResult = 1D)]
        [TestCase("GetWorkingDays(Date(\"2023-08-29\"), Date(\"2023-09-03\")) > 5", ExpectedResult = false)]
        [TestCase("GetWorkingDays(Date(\"2023-08-29\"), Date(\"2023-09-03\")) = 0", ExpectedResult = true)]
        public object ValidationModeTesting(string expression)
        {
            var result = (object)null;
            using (ExpressionMode.BeginValidationMode())
            {
                Assert.IsTrue(ExpressionMode.IsValidationMode, "Validation mode should be enabled");

                result = Factory.Instance.Create().Compile(expression).EvalValue(null);
            }
            Assert.IsFalse(ExpressionMode.IsValidationMode, "Validation mode should be disabled");

            return result;
        }

        [Test]
        public void ValidationModeIsNotSeenByOtherFlows()
        {
            using (var entered = new ManualResetEventSlim())
            {
                var otherFlow = Task.Run(() =>
                {
                    entered.Wait();
                    return ExpressionMode.IsValidationMode;
                });

                using (ExpressionMode.BeginValidationMode())
                {
                    entered.Set();
                    Assert.IsFalse(otherFlow.Result, "Validation mode must not be visible to other flows");
                }
            }
        }

        [Test]
        public void LeavingANestedValidationModeKeepsTheOuterOne()
        {
            using (ExpressionMode.BeginValidationMode())
            {
                using (ExpressionMode.BeginValidationMode())
                {
                    Assert.IsTrue(ExpressionMode.IsValidationMode);
                }

                Assert.IsTrue(ExpressionMode.IsValidationMode, "The outer validation mode should still be on");
            }

            Assert.IsFalse(ExpressionMode.IsValidationMode);
        }
    }
}
