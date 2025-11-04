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
            [TestCase("createBarcode(\"test-12345\",\"code128\")", true)] // we take the default one

            [TestCase("createBarcode(\"test-12345\")", false)]
            [TestCase("createBarcode(\"\",\"Code128\")", false)]
            
            [TestCase("createQRCode(\"test-12345\", \"M\", 1000)", true)]
            [TestCase("createQRCode(\"https://example.com\", \"Q\", 300)", true)]
            [TestCase("createQRCode(\"test-12345\", \"L\", 128)", true)] 
            [TestCase("createQRCode(\"test-12345\", \"H\", 512)", true)]
            [TestCase("createQRCode(\"test-12345\")", true)] 
            [TestCase("createQRCode(\"test-12345\", \"INVALID\", 256)", false)] 
            [TestCase("createQRCode(\"\", \"M\", 256)", false)]
            [TestCase("createQRCode(\"  \", \"M\", 256)", false)] 
            [TestCase("createQRCode(null, \"M\", 256)", false)] 
            [TestCase("createQRCode(\"test-12345\", \"M\", -1)", false)] 

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