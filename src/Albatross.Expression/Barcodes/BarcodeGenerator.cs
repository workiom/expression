using NetBarcode;

namespace Albatross.Expression.Barcodes
{
    public class BarcodeGenerator
    {
        public byte[] Generate(string data, BarcodeType type = BarcodeType.Code128,
            int width = 0, int height = 0, bool showLabel = true)
        {
            var barcode = width <= 0 ? new Barcode(data, MapType(type), showLabel) :
                new Barcode(data, MapType(type), showLabel, width, height);
            
            return barcode.GetByteArray(ImageFormat.Jpeg);
        }

        private static Type MapType(BarcodeType type)
        {
            if (!System.Enum.TryParse<Type>(type.ToString(), out var mappedType))
                return Type.Code128;

            return mappedType;
        }
    }
}