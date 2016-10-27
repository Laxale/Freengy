// Created by Laxale 27.10.2016
//
//


namespace Freengy.CommonResources.Converters 
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;


    [ValueConversion(typeof(string), typeof(ImageSource))]
    public class StringUriToImageSourceConverter : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
        {
            var stringValue = (string)value;

            ImageSource result = ConvertStringToBitmap(stringValue);

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) 
        {
            throw new NotImplementedException();
        }


        private static ImageSource ConvertStringToBitmap(string uri) 
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(uri);
            bitmapImage.EndInit();

            return bitmapImage;
        }
    }
}