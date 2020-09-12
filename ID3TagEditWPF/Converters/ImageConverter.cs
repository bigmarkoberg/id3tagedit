using System;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace ID3TagEditWPF.Converters
{
    [ValueConversion(typeof(System.Drawing.Image), typeof(System.Windows.Media.ImageSource))]
    public sealed class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            const int LARGEST = 75;
            var image = (System.Drawing.Image)value;
            var w = image.Width;
            var h = image.Height;

            if (w >= h)
            {
                // w/h = LARGEST/nh
                h = LARGEST * h / w;
                w = LARGEST;
            }
            else
            {
                // w/h = mw/LARGEST
                w = LARGEST * w / h;
                h = LARGEST;
            }

            image = image.GetThumbnailImage(w, h, null, IntPtr.Zero);
            var bitmap = new System.Windows.Media.Imaging.BitmapImage();

            bitmap.BeginInit();
            MemoryStream memoryStream = new MemoryStream();
            image.Save(memoryStream, ImageFormat.Png);
            memoryStream.Seek(0, SeekOrigin.Begin);
            bitmap.StreamSource = memoryStream;
            bitmap.EndInit();

            return bitmap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
