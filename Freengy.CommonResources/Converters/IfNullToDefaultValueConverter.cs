// Created by Laxale 28.11.2016
//
//


namespace Freengy.CommonResources.Converters 
{
    using System;
    using System.Windows;
    using System.Windows.Data;
    using System.Globalization;


    /// <summary>
    /// Converts value to default incoming parameter if values is null or empty
    /// </summary>
    [ValueConversion(typeof(object), typeof(object))]
    public class IfNullToDefaultValueConverter : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string stringValue = value as string;

            if (stringValue != null)
            {
                if (string.IsNullOrWhiteSpace(stringValue)) return parameter;
            }

            return value ?? parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) 
        {
            throw new NotImplementedException();
        }
    }
}