// Created by Laxale 15.11.2016
//
//


namespace Freengy.CommonResources.Converters 
{
    using System;
    using System.Windows;
    using System.Windows.Data;
    using System.Globalization;


    [ValueConversion(typeof(ResizeMode), typeof(Visibility))]
    public class ResizeModeToVisibilityConverter : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
        {
            var castedValue = (ResizeMode)value;

            return castedValue == ResizeMode.NoResize ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) 
        {
            throw new NotImplementedException();
        }
    }
}