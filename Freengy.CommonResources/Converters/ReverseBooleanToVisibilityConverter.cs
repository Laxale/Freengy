// Created by Laxale 26.10.2016
//
//


namespace Freengy.CommonResources.Converters 
{
    using System;
    using System.Windows;
    using System.Windows.Data;
    using System.Globalization;


    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class ReverseBooleanToVisibilityConverter : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
        {
            bool boolValue = (bool)value;

            return boolValue ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) 
        {
            throw new NotImplementedException();
        }
    }
}