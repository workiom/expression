﻿using Albatross.Expression.Documentation;
using Albatross.Expression.Documentation.Attributes;
using Albatross.Expression.Tokens;
using System;

namespace Albatross.Expression.Operations
{
    /// <summary>
    /// <para>Infix AND operation.</para>
    /// <para>Operand Count: 2</para>
    /// <list type="number">
    ///		<listheader>
    ///		<description>Operands</description>
    ///		</listheader>
    ///		<item><description>Operand1 : any</description></item>
    ///		<item><description>Operand2 : any</description></item>
    /// </list>
    /// 
    /// <para>The input operands are converted to boolean.  See the <see cref="Albatross.Expression.Extensions.ConvertToBoolean(object)"/> method for the conversion logic</para>
    /// <para>Output Type: Boolean</para>
    /// <para>Usage: 3 > 2 and 2 > 1</para>
    /// <para>Precedance: 30</para>
    /// </summary>
    [OperationDoc(Group.Boolean, "{token}", @"### Logical AND operation")]
    [ParserOperation(Group = "Logical")]
    public class And : InfixOperationToken
    {

        public override string Name { get { return "And"; } }
        public override bool Symbolic { get { return true; } }
        public override int Precedence { get { return 30; } }


        public override object EvalValue(Func<string, object> context)
        {
            base.EvalValue(context);

            object value = Operand1.EvalValue(context);

            if (!value.ConvertToBoolean())
            {
                return false;
            }
            else
            {
                value = Operand2.EvalValue(context);
                return value.ConvertToBoolean();
            }
        }
    }
}
