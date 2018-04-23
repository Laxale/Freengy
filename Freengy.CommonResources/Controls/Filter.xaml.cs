// Created by Laxale 18.04.2018
//
//

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Freengy.CommonResources.Controls 
{
    /// <summary>
    /// Текстовый фильтр. Скрываемый водяной знак (поясняющий назначение фильтрации текст) биндится через свойство <see cref="Filter.Tag"/>.
    /// </summary>
    public partial class Filter : UserControl 
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public Filter() 
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }


        public static readonly DependencyProperty WatermarkTextProperty = 
            DependencyProperty.Register(nameof(WatermarkText), typeof(string), typeof(Filter));

        public static readonly DependencyProperty FilterTextProperty = 
            DependencyProperty.Register(nameof(FilterText), typeof(string), typeof(Filter));


        public static readonly DependencyProperty MustFocusOnLoadedProperty = 
            DependencyProperty.Register(nameof(MustFocusOnLoaded), typeof(bool), typeof(Filter));


        /// <summary>
        /// Задаёт или возвращает значение флага - будет ли фильтр фокусироваться на своём поле ввода автоматически.
        /// </summary>
        public bool MustFocusOnLoaded 
        {
            get => (bool) GetValue(MustFocusOnLoadedProperty);
            set => SetValue(MustFocusOnLoadedProperty, value);
        }

        /// <summary>
        /// Получает или задаёт текст поиска.
        /// </summary>
        public string FilterText 
        {
            get => (string)GetValue(FilterTextProperty);
            set => SetValue(FilterTextProperty, value);
        }


        /// <summary>
        /// Текст, отображаемый на пустом поле поиска. Удаляется при пользовательском вводе.
        /// </summary>
        public string WatermarkText 
        {
            get => (string)GetValue(WatermarkTextProperty);
            set => SetValue(WatermarkTextProperty, value);
        }


        private void ClearButton_OnClick(object sender, RoutedEventArgs e) 
        {
            FilterText = string.Empty;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs) 
        {
            if (MustFocusOnLoaded)
            {
                Keyboard.Focus(InputBox);
            }
        }
    }
}