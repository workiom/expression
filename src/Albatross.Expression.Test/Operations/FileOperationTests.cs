namespace Albatross.Expression.Test.Operations
{
    using NUnit.Framework;

    namespace Albatross.Expression.Test.Operations
    {
        [TestFixture]
        [Category("Operations")]
        public class FileOperationTests
        {
            [TestCase("createBarcode(\"test-12345\",\"Code128\",200,100)", true)]
            [TestCase("createBarcode(\"abc12313_123123!@#\",\"Code128\",300,100)", true)]
            [TestCase("createBarcode(\"abc12313_123123!@#\",\"Code128\")", true)]
            [TestCase("createBarcode(\"abc12313_123123!@#\",\"Code128\",\"\",200)", true)]
            [TestCase("createBarcode(\"abc12313_123123!@#\",\"Code128\",400,\"\")", true)]
            [TestCase("createBarcode(\"abc12313_123123!@#\",\"Code128\",0,0)", true)]
            [TestCase("createBarcode(\"abc12313_123123!@#\",\"Code128\",\"\",0)", true)]
            [TestCase("createBarcode(\"abc12313_123123!@#\",\"Code128\",0,\"\")", true)]
            [TestCase("createBarcode(\"abc12313_123123!@#\",\"Code128\",-1,0)", true)]
            [TestCase("createBarcode(\"test-12345\",\"XXX\")", true)] // we take the default one

            [TestCase("createBarcode(\"test-12345\")", false)]
            [TestCase("createBarcode(\"\",\"Code128\")", false)]
            public void OperationsTesting(string expression, bool succeed)
            {
                try
                {
                    var result = Factory.Instance.Create().Compile(expression).EvalValue(null);

                    var isBytes = result != null && result.GetType() == typeof(byte[]);

                    Assert.AreEqual(isBytes, succeed);
                }
                catch (System.Exception)
                {
                    Assert.IsFalse(succeed);
                }
            }
        }
    }
}