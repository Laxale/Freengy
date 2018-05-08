// Created by Laxale 08.05.2018
//
//

using System;
using System.Windows;
using System.Windows.Media;


namespace Freengy.CommonResources 
{
    /// <summary>
    /// Экспозит наружу ресурсы сборки. Полезно для получения ресурсов из кода.
    /// </summary>
    public class CommonResourceExposer 
    {
        public const string FirstMainBrushKey = "FirstMainBrush";
        public const string LightGrayBrushKey = "LightGrayBrush";

        private readonly string brushesDictionaryPath = "pack://application:,,,/Freengy.CommonResources;component/Styles/Brushes.xaml";


        public SolidColorBrush GetBrush(string brushKey) 
        {
            var dictionary = new ResourceDictionary
            {
                Source = new Uri(brushesDictionaryPath)
            };
            
            switch (brushKey)
            {
                case FirstMainBrushKey:
                    return (SolidColorBrush)dictionary[FirstMainBrushKey];

                case LightGrayBrushKey:
                    return (SolidColorBrush)dictionary[LightGrayBrushKey];

                default:
                    throw new InvalidOperationException($"Brush key '{ brushKey }' is not expected");
            }
        }
    }
}