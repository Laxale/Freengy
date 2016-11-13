// Created by Laxale 12.11.2016
//
//


namespace Freengy.CommonResources.Converters 
{
    using System;
    using System.Windows;
    using System.Windows.Data;
    using System.Globalization;


    [ValueConversion(typeof(bool), typeof(bool))]
    public class ReverseBooleanConverter : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
        {
            bool boolValue = (bool)value;

            return !boolValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) 
        {
            throw new NotImplementedException();
        }
    }
}