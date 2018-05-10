// Created by Laxale 10.06.2018
//
//

using System;
using System.Globalization;
using System.Windows.Data;


namespace Freengy.Chatter.Converters 
{
    /// <summary>
    /// Преобразует название аккаунта в значение флага - мой ли это аккаунт?
    /// </summary>
    [ValueConversion(typeof(string), typeof(bool))]
    internal class MessageAutorToBoolConverter : IValueConverter 
    {
        /// <summary>Converts a value. </summary>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
        {
            var accountName = (string) value;
            var myAccountName = (string) parameter;

            if(string.IsNullOrWhiteSpace(accountName)) throw new ArgumentNullException(nameof(accountName));
            if(string.IsNullOrWhiteSpace(myAccountName)) throw new ArgumentNullException(nameof(myAccountName));

            return accountName == myAccountName;
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
