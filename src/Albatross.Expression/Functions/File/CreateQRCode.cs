using Albatross.Expression.Documentation;
using Albatross.Expression.Documentation.Attributes;
using Albatross.Expression.QRCodes;
using Albatross.Expression.Tokens;
using System;

namespace Albatross.Expression.Functions.File
{
    [FunctionDoc(Group.File, "{token}( , , )",
        @"### Returns a QR code in image format.
     #### Inputs:
     - input: String (mandatory). Accepts record fields or plain text.
     - ecc: Error correction level (optional). Default is 'M'. Supported: 'L', 'M', 'Q', 'H'.
    - size: Width and height of the image (optional, in pixels). Default is 256.
#### Outputs:
    - File: The generated QR code image."
    )]
    [ParserOperation]
    public class CreateQRCode : PrefixOperationToken
    {
        public override string Name => "CreateQRCode";
        public override bool Symbolic => false;

        public override int MinOperandCount => 1;

        public override int MaxOperandCount => 3;

        public override object EvalValue(Func<string, object> context)
        {
            var operands = GetOperands(context);
            if (operands.Count < 1)
                throw new ArgumentException("Input is required");

            string input = operands.GetOperandsAt(0)?.ToString();
            string ecc = operands.Count > 1 ? operands.GetOperandsAt(1)?.ToString() ?? "M" : "M";
            int size = operands.Count > 2 && int.TryParse(operands.GetOperandsAt(2)?.ToString(), out int parsedSize) 
                ? parsedSize 
                : 256;

            if (string.IsNullOrWhiteSpace(input))
                return null;

            var generator = new QRCodeGenerator();
            byte[] qrCodeBytes = generator.Generate(input, ecc, size);

            return qrCodeBytes;
        }
    }
}