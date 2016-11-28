// Created by Laxale 16.11.2016
//
//


namespace Freengy.CommonResources.Converters 
{
    using System;
    using System.Windows.Data;
    using System.Globalization;


    /// <summary>
    /// Converts value to false if it is null or empty
    /// </summary>
    [ValueConversion(typeof(object), typeof(bool))]
    public class IsNullToBoolConverter : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
        {
            string castedValue = value as string;

            if (castedValue != null)
            {
                return string.IsNullOrWhiteSpace(castedValue);
            }

            return value == null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) 
        {
            throw new NotImplementedException();
        }
    }
}