// Created 20.10.2016
//
//


namespace Freengy.CommonResources.Converters 
{
    using System;
    using System.Globalization;
    using System.Windows.Data;


    public class PropertyNameToBindingConverter : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
        {
            var propertyName = value as string;

            if (propertyName == null) throw new ArgumentException("value must be a string");


            var bindong = new Binding(propertyName) { Mode = BindingMode.OneTime };

            return bindong;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) 
        {
            throw new NotImplementedException();
        }
    }
}