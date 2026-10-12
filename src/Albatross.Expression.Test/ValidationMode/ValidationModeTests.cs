using NUnit.Framework;

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
    }
}
