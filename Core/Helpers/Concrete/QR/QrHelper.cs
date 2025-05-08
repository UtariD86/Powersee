using QRCoder;
using System;
using System.Drawing;
using System.IO;

namespace Core.Helpers.Concrete.QR
{
    public static class QrHelper
    {
        public static byte[] QRKodOlustur(string icerik)
        {
            try
            {
                // QRCodeGenerator nesnesi oluşturuluyor
                QRCodeGenerator qrGenerator = new QRCodeGenerator();

                // QRCodeData nesnesi oluşturuluyor, burada QR kodu verisi oluşturuluyor
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(icerik, QRCodeGenerator.ECCLevel.Q);

                // QR kodu doğrudan bitmap'e dönüştürülüyor
                int size = 300; // QR kodunun boyutu
                var qrBitmap = new Bitmap(size, size);
                var graphics = Graphics.FromImage(qrBitmap);

                // QR kod verilerini çizme
                graphics.FillRectangle(Brushes.White, 0, 0, size, size); // Beyaz arka plan
                var pixelSize = size / qrCodeData.ModuleMatrix.Count; // Modüllerin büyüklüğü

                for (int i = 0; i < qrCodeData.ModuleMatrix.Count; i++)
                {
                    for (int j = 0; j < qrCodeData.ModuleMatrix[i].Count; j++)
                    {
                        if (qrCodeData.ModuleMatrix[i][j])
                        {
                            // Siyah kareler (QR kodunun siyah modülleri)
                            graphics.FillRectangle(Brushes.Black, j * pixelSize, i * pixelSize, pixelSize, pixelSize);
                        }
                    }
                }

                // QR kodu PNG formatında MemoryStream'e kaydediliyor
                using (MemoryStream ms = new MemoryStream())
                {
                    qrBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png); // PNG formatında kaydediyoruz
                    return ms.ToArray(); // Byte dizisi olarak QR kodunu döndürüyoruz
                }
            }
            catch (Exception ex)
            {
                throw new Exception("QR Code generation failed.", ex);
            }
        }
    }
}
