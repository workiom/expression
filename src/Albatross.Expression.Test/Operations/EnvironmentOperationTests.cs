using Albatross.Expression.Exceptions;
using Albatross.Expression.Operations;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Albatross.Expression.Test.Operations
{
    [TestFixture]
    [Category("Operations")]
    public class EnvironmentOperationTests
    {
        [TestCase("CurrentUser()", "Jane Doe", ExpectedResult = "Jane Doe")]
        [TestCase("currentuser()", "Jane Doe", ExpectedResult = "Jane Doe")]
        [TestCase("Concat(\"Created by \", CurrentUser())", "Jane Doe", ExpectedResult = "Created by Jane Doe")]
        [TestCase("If(CurrentUser() = \"Jane Doe\", 1, 2)", "Jane Doe", ExpectedResult = 1)]
        [TestCase("CurrentUser()", "", ExpectedResult = "")]
        [TestCase("CurrentUser()", null, ExpectedResult = null)]
        public object CurrentUserReturnsTheHostValue(string expression, string currentUser)
        {
            return Evaluate(expression, name => name == CurrentUser.ContextVariableName ? currentUser : null);
        }

        [Test]
        public void CurrentUserRequestsOnlyItsContextVariable()
        {
            var requested = new List<string>();

            Evaluate("CurrentUser()", name =>
            {
                requested.Add(name);
                return "Jane Doe";
            });

            CollectionAssert.AreEqual(new[] { CurrentUser.ContextVariableName }, requested);
        }

        [Test]
        public void CurrentUserWithoutContextReturnsNull()
        {
            Assert.IsNull(Evaluate("CurrentUser()", null));
        }

        [Test]
        public void CurrentUserInValidationModeReturnsTheHostValue()
        {
            using (ExpressionMode.BeginValidationMode())
            {
                Assert.AreEqual("Jane Doe", Evaluate("CurrentUser()", name => "Jane Doe"));
            }
        }

        [Test]
        public void CurrentUserContextVariableCannotBeWrittenInAnExpression()
        {
            Assert.Throws<TokenParsingException>(() => Factory.Instance.Create().Compile(CurrentUser.ContextVariableName));
        }

        [Test]
        public void CurrentUserFromExecutionContext()
        {
            var context = new DictionaryExecutionContextFactory(Factory.Instance.Create()).Create();
            context.SetValue(CurrentUser.ContextVariableName, "Jane Doe");

            Assert.AreEqual("Jane Doe", context.Eval("CurrentUser()", null, null));
        }

        [Test]
        public void CurrentUserMissingFromExecutionContextNamesTheVariable()
        {
            var context = new DictionaryExecutionContextFactory(Factory.Instance.Create()).Create();

            var exception = Assert.Throws<MissingVariableException>(() => context.Eval("CurrentUser()", null, null));
            StringAssert.Contains(CurrentUser.ContextVariableName, exception.Message);
        }

        private static object Evaluate(string expression, Func<string, object> context)
        {
            return Factory.Instance.Create().Compile(expression).EvalValue(context);
        }
    }
}
