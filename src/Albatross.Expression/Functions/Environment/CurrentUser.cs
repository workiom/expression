using Albatross.Expression.Documentation;
using Albatross.Expression.Documentation.Attributes;
using Albatross.Expression.Tokens;
using System;

namespace Albatross.Expression.Operations
{
    /// <summary>
    /// <para>Prefix operation that returns the name of the current user, as supplied by the host application</para>
    /// <para>The host supplies the name through the evaluation context under <see cref="ContextVariableName"/>. Nothing is read from the operating system, so the function behaves the same on every platform.</para>
    /// <para>Operand Count: 0</para>
    /// <para>Output Type: string</para>
    /// </summary>
    [FunctionDoc(Group.Environment, "{token}()",
@"### Returns the name of the current user.
#### Inputs:
- No inputs
#### Outputs:
- String"
    )]
    [ParserOperation]
    public class CurrentUser : PrefixOperationToken
    {
        /// <summary>
        /// The name this function requests from the evaluation context. It is not a valid variable name, so an expression can only reach it through CurrentUser().
        /// </summary>
        public const string ContextVariableName = "$CurrentUser";

        public override string Name { get { return "CurrentUser"; } }
        public override int MinOperandCount { get { return 0; } }
        public override int MaxOperandCount { get { return 0; } }
        public override bool Symbolic { get { return false; } }

        public override object EvalValue(Func<string, object> context)
        {
            return context?.Invoke(ContextVariableName)?.ToString();
        }
    }
}
