// Created by Laxale 14.05.2018
//
//

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;


namespace Freengy.CommonResources.Converters 
{
    /// <summary>
    /// Преобразует входное число в строку, разделённую параметром по 3 символа.
    /// </summary>
    [ValueConversion(typeof(int), typeof(string))]
    public class NumberToSplitFormatConverter : IValueConverter 
    {
        /// <summary>Преобразовать входное число в строку, разделённую параметром по 3 символа.</summary>
        /// <param name="value">Входнео число для разбиения по 3 символа.</param>
        /// <param name="targetType">Не исопльзуется.</param>
        /// <param name="parameter">Строка-параметр, который будет разделять группы по 3 символа.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
        {
            // ugly fuck. cast uint to int fails
            string input = int.Parse(value.ToString()).ToString();
            string splitter = (string) parameter ?? ".";

            var charCount = input.Length;
            var builder = new StringBuilder();

            for (int charIndex = charCount - 1; charIndex >= 0; charIndex--)
            {
                builder.Append(input[charIndex]);

                if (charIndex != 0 && charIndex % 3 == 0 && charIndex < charCount)
                {
                    builder.Append(splitter);
                }
            }

            string result = string.Join(string.Empty, builder.ToString().Reverse());

            return result;
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