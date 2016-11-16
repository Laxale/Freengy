// Created by Laxale 16.11.2016
//
//


namespace Freengy.CommonResources.Converters 
{
    using System;
    using System.Windows;
    using System.Windows.Data;
    using System.Globalization;


    [ValueConversion(typeof(string), typeof(bool))]
    public class StringToBoolConverter : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
        {
            string castedValue = (string)value;

            bool stringHasText = !string.IsNullOrWhiteSpace(castedValue);

            return stringHasText;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) 
        {
            throw new NotImplementedException();
        }
    }
}