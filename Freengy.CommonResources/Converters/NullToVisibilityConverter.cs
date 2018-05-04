// Created by Laxale 18.04.2018
//
//

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace Freengy.CommonResources.Converters 
{
    /// <summary>
    /// Конвертирует видимость контрола из нулевого объекта.
    /// </summary>
    public class NullToVisibilityConverter : IValueConverter 
    {
        /// <summary>Converts a value. </summary>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
        {
            if (value is string stringValue)
            {
                return !string.IsNullOrWhiteSpace(stringValue) ? Visibility.Visible : Visibility.Collapsed;
            }

            return value != null ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>Converts a value. </summary>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) 
        {
            throw new NotImplementedException();
        }
    }
}