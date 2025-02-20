using Albatross.Expression.Barcodes;
using Albatross.Expression.Documentation;
using Albatross.Expression.Documentation.Attributes;
using Albatross.Expression.Exceptions;
using Albatross.Expression.Tokens;
using System;

namespace Albatross.Expression.Functions.File
{
    [FunctionDoc(Group.File, "{token}( , )",
        @"### Returns a barcode in image format.
#### Inputs:
- input: String (mandatory)
- standard: String (mandatory). Supported Standards: Codabar, Code11, Code128, Code128A, Code128B, Code128C, Code39, Code39E, Code93, EAN13, EAN8.
- width: Number (optional), default is 160
- hight: Number (optional), default is 80

#### Outputs:
- File: The generated barcode image"
    )]
    [ParserOperation]
    public class CreateBarcode : PrefixOperationToken
    {
        public override string Name
        {
            get { return "CreateBarcode"; }
        }

        public override int MinOperandCount
        {
            get { return 2; }
        }

        public override int MaxOperandCount
        {
            get { return 4; }
        }

        public override bool Symbolic
        {
            get { return false; }
        }

        public override object EvalValue(Func<string, object> context)
        {
            var operands = GetOperands(context);
            if (operands.Count <= 0)
                return null;

            object value1 = operands.GetOperandsAt(0);
            object value2 = operands.GetOperandsAt(1);
            object value3 = operands.GetOperandsAt(2);
            object value4 = operands.GetOperandsAt(3);


            var input = value1?.ToString();
            var type = BarcodeType.Code128;
            int width = 160;
            int hight = 80;

            if (value2 != null && !Enum.TryParse(value2.ToString(), out type))
                throw new UnexpectedTypeException(value2.GetType());

            if (value3 != null && !int.TryParse(value3.ToString(), out width))
                throw new UnexpectedTypeException(value3.GetType());

            if (value4 != null && !int.TryParse(value4.ToString(), out hight))
                throw new UnexpectedTypeException(value4.GetType());

            var barcode = new BarcodeGenerator()
                .Generate(input, type, width, hight);

            return barcode;
        }
    }
}