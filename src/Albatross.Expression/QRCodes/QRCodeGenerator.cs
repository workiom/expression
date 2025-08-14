using SkiaSharp;
using SkiaSharp.QrCode;
using System;
using System.IO;

namespace Albatross.Expression.QRCodes
{
    public class QRCodeGenerator
    {
        public byte[] Generate(string input, string ecc = "M", int size = 256)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            if (size < 1)
                size = 256;
            
            var errorCorrectionLevel = GetErrorCorrectionLevel(ecc);
            using (var generator = new SkiaSharp.QrCode.QRCodeGenerator())
            {
                var qr = generator.CreateQrCode(input, errorCorrectionLevel);
                var info = new SKImageInfo(size, size);
                using (var surface = SKSurface.Create(info))
                {
                    var canvas = surface.Canvas;
                    canvas.Render(qr, info.Width, info.Height);
                    using (var image = surface.Snapshot())
                    {
                        using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                        {
                            byte[] result;
                            using (var memoryStream = new MemoryStream())
                            {
                                data.SaveTo(memoryStream);
                                result = memoryStream.ToArray();
                            }
                            
                            return result;
                        }
                    }
                }
            }
        }

        private ECCLevel GetErrorCorrectionLevel(string ecc)
        {
            try
            {
                switch (ecc.ToUpper())
                {
                    case "L":
                        return ECCLevel.L;
                    case "M":
                        return ECCLevel.M;
                    case "Q":
                        return ECCLevel.Q;
                    case "H":
                        return ECCLevel.H;
                    default:
                        return ECCLevel.M;
                }
            }
            catch
            {
                return ECCLevel.M;
            }
            
        }
    }
}