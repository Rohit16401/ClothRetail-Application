using System;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using ZXing;
using ZXing.Common;
using ZXing.Windows.Compatibility;

namespace RetailUI.BarcodeServices
{
    public class BarcodeService
    {
        // Generates a barcode from the input data and returns it as a Base64 string.
        public static string GenerateBarcode(string data, int width = 300, int height = 150)
        {
            if (string.IsNullOrEmpty(data))
                throw new ArgumentException("Barcode data cannot be null or empty", nameof(data));

            // BarcodeWriter with no generic types - will generate a Bitmap by default
            var barcodeWriter = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128, // Commonly used barcode format
                Options = new EncodingOptions
                {
                    Width = width,
                    Height = height,
                    Margin = 10 // Optional, controls the whitespace around the barcode
                }
            };

            // Generate the barcode bitmap
            using (var bitmap = barcodeWriter.Write(data))
            {
                using (var stream = new MemoryStream())
                {
                    bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png); // Save the bitmap to MemoryStream
                    return Convert.ToBase64String(stream.ToArray()); // Return the Base64-encoded string
                }
            }
        }

        // Converts a Base64 barcode string back into a BitmapImage for display in WPF.
        public static BitmapImage GenerateBarcodeImage(string barcodeBase64)
        {
            if (string.IsNullOrEmpty(barcodeBase64))
                throw new ArgumentException("Base64 string cannot be null or empty", nameof(barcodeBase64));

            var image = new BitmapImage();
            using (var stream = new MemoryStream(Convert.FromBase64String(barcodeBase64)))
            {
                stream.Position = 0; // Ensure the stream is at the beginning
                image.BeginInit();
                image.StreamSource = stream;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                image.Freeze(); // Make the image thread-safe for WPF
            }
            return image;
        }
    }
}
