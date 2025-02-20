﻿using NUnit.Framework;

namespace Albatross.Expression.Test
{
    [TestFixture]
    [Category("Operations")]
    public class OperationTests
    {
        [TestCase("coalesce(2, 1)", ExpectedResult = 2)]
        [TestCase("coalesce(null, 1)", ExpectedResult = 1)]

        [TestCase("max(1, 2, 3)", ExpectedResult = 3)]
        [TestCase("max(1, 2, null)", ExpectedResult = 2)]

        [TestCase("min(1, 2, 3)", ExpectedResult = 1)]
        [TestCase("min(null, 1, 3)", ExpectedResult = 1)]

        //other functions
        [TestCase("avg(3, 4, 5 )", ExpectedResult = 4)]
        [TestCase("avg(null, 4, 5 )", ExpectedResult = 4.5)]
        [TestCase("avg(0, 4, 5 )", ExpectedResult = 3)]
        [TestCase("avg(@(0, 4, 5))", ExpectedResult = 3)]
        
        // Size
        [TestCase("size(500000)", ExpectedResult = 0.5)]
        [TestCase("size(10000000)", ExpectedResult = 10)]
        [TestCase("size(50000000)", ExpectedResult = 50)]
        [TestCase("size(100000000)", ExpectedResult = 100)]
        [TestCase("size(500000000)", ExpectedResult = 500)]
        [TestCase("size(1000000000)", ExpectedResult = 1000)]
        [TestCase("size(5000000000)", ExpectedResult = 5000)]
        [TestCase("size(10000000000)", ExpectedResult = 10000)]
        
        // Random
        [TestCase("getRandom() > 0", ExpectedResult = true)]
        [TestCase("getRandom(10) > 0 and getRandom(10, 100) < 100", ExpectedResult = true)]
        [TestCase("getRandom(10, 100) > 10 and getRandom(10, 100) < 100", ExpectedResult = true)]
        [TestCase("getRandom(10, 100) < 10 or getRandom(10, 100) > 100", ExpectedResult = false)]

        // IsBetween
        [TestCase("IsBetween(5, 1, 10)", ExpectedResult = true)]
        [TestCase("IsBetween(10, 1, 10)", ExpectedResult = true)]
        [TestCase("IsBetween(1, 1, 10)", ExpectedResult = true)]
        [TestCase("IsBetween(0, 1, 10)", ExpectedResult = false)]
        [TestCase("IsBetween(11, 1, 10)", ExpectedResult = false)]
        [TestCase("IsBetween(0, -10, 10)", ExpectedResult = true)]
        [TestCase("IsBetween(-10, -10, 10)", ExpectedResult = true)]
        [TestCase("IsBetween(10, -10, 10)", ExpectedResult = true)]
        [TestCase("IsBetween(-11, -10, 10)", ExpectedResult = false)]
        [TestCase("IsBetween(11, -10, 10)", ExpectedResult = false)]
        
        // Round
        [TestCase("round(1.4)", ExpectedResult = 1)]
        [TestCase("round(1.431412, 2)", ExpectedResult = 1.43)]
        [TestCase("round(1.4991412, 2)", ExpectedResult = 1.5)]

        [TestCase("roundUp(50.99)", ExpectedResult = 51)]
        [TestCase("roundDown(50.99)", ExpectedResult = 50)]

        [TestCase("Number(\"44\")", ExpectedResult = 44)]
        [TestCase("SubtractTime(Date(\"2022-10-10 10:20:10\"),Date(\"2022-10-10 10:10:13\"))", ExpectedResult = 10)]
        [TestCase("if(rand()=rand() ,1,0)", ExpectedResult = 0)]
        public object OperationsTesting(string expression)
        {
            return Factory.Instance.Create().Compile(expression).EvalValue(null);
        }
    }
}
