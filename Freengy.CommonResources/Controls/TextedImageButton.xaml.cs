// Created by Laxale 08.05.2018
//
//

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace Freengy.CommonResources.Controls 
{
    /// <summary>
    /// Interaction logic for TextedImageButton.xaml
    /// </summary>
    public partial class TextedImageButton : Button 
    {
        public TextedImageButton() 
        {
            InitializeComponent();
        }


        public static readonly DependencyProperty ImageContentProperty = 
            DependencyProperty.Register(nameof(ImageContent), typeof(object), typeof(TextedImageButton));

        public static readonly DependencyProperty ImageTextProperty = 
            DependencyProperty.Register(nameof(ImageText), typeof(string), typeof(TextedImageButton));

        public static readonly DependencyProperty TextForegroundProperty = 
            DependencyProperty.Register
            (
                nameof(TextForeground), 
                typeof(SolidColorBrush), 
                typeof(TextedImageButton), 
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 0, 0)))
            );


        /// <summary>
        /// Возвращает или задаёт цвет текста кнопки.
        /// </summary>
        public SolidColorBrush TextForeground 
        {
            get => (SolidColorBrush) GetValue(TextForegroundProperty);
            set => SetValue(TextForegroundProperty, value);
        }
        
        /// <summary>
        /// Возвращает или задаёт текст возле иконки.
        /// </summary>
        public string ImageText 
        {
            get => (string)GetValue(ImageTextProperty);
            set => SetValue(ImageTextProperty, value);
        }

        /// <summary>
        /// Возвращает или задаёт содержимое "иконки" кнопки.
        /// </summary>
        public object ImageContent 
        {
            get => GetValue(ImageContentProperty);
            set => SetValue(ImageContentProperty, value);
        }
    }
}