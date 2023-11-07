using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Yachthafen
{
    public class ByteArrayToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is byte[] byteArray)
            {
                BitmapImage imageSource = new BitmapImage();
                imageSource.BeginInit();
                imageSource.StreamSource = new System.IO.MemoryStream(byteArray);
                imageSource.EndInit();
                return imageSource;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte[] imageBytes;

            using (MemoryStream ms = new MemoryStream())
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder(); // You can choose a different encoder based on your image format
                encoder.Frames.Add(BitmapFrame.Create((BitmapImage)value));
                encoder.Save(ms);

                imageBytes = ms.ToArray();
            }
            return imageBytes;
        }
    }
}