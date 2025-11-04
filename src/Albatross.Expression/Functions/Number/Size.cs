using Albatross.Expression.Documentation.Attributes;
using Albatross.Expression.Exceptions;
using Albatross.Expression.Tokens;
using ByteSizeLib;
using System;

namespace Albatross.Expression.Functions.Number
{
    /// <summary>
    /// <para>Prefix operation that converts bytes into megabytes</para>
    /// <para>Operand Count: 1</para>
    /// <para>Operand Type: number</para>
    /// <para>Output Type: double</para>
    /// 
    /// <para>Usage: Size(1048576)</para>
    /// <para>Note: If the input is null, the result will be null.</para>
    /// </summary>
    [FunctionDoc(Documentation.Group.Number, "{token}()",
        @"### Convert Bytes to Megabytes
#### Inputs:
- value: Number of bytes
#### Outputs:
- Converted value in megabytes as double."
    )]
    [ParserOperation]
    public class Size : PrefixOperationToken
    {
        public override string Name => "Size";

        public override int MinOperandCount => 1;

        public override int MaxOperandCount => 1;

        public override bool Symbolic => false;

        public override object EvalValue(Func<string, object> context)
        {
            var operand = Operands[0];
            object value = operand.EvalValue(context);
            try
            {
                double bytes = Convert.ToDouble(value);
                var megaBytes = ByteSize.FromBytes(bytes).MegaBytes;
                return megaBytes;
            }
            catch (InvalidCastException)
            {
                throw new UnexpectedTypeException();
            }
        }
    }
}