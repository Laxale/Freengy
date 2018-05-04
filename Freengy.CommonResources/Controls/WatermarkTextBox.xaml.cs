// Created by Laxale 16.11.2016
//
//

using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;


namespace Freengy.CommonResources.Controls 
{
    public partial class WatermarkTextBox : TextBox 
    {
        public WatermarkTextBox() 
        {
            InitializeComponent();
        }


        public static readonly DependencyProperty WatermarkTextProperty =
            DependencyProperty.Register(nameof(WatermarkText), typeof(string), typeof(WatermarkTextBox));

        public static readonly DependencyProperty WatermarkForegroundProperty =
            DependencyProperty.Register
            (
                nameof(WatermarkForeground), 
                typeof(SolidColorBrush), 
                typeof(WatermarkTextBox),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 0, 0)))
            );

        public static readonly DependencyProperty MouseOverBrushProperty =
            DependencyProperty.Register
            (
                nameof(MouseOverBrush), 
                typeof(SolidColorBrush), 
                typeof(WatermarkTextBox),
                new PropertyMetadata(new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)))
            );


        public string WatermarkText 
        {
            get => (string)GetValue(WatermarkTextProperty);
            set => SetValue(WatermarkTextProperty, value);
        }

        public SolidColorBrush WatermarkForeground 
        {
            get => (SolidColorBrush)GetValue(WatermarkForegroundProperty);
            set => SetValue(WatermarkForegroundProperty, value);
        }

        public SolidColorBrush MouseOverBrush 
        {
            get => (SolidColorBrush)GetValue(WatermarkForegroundProperty);
            set => SetValue(WatermarkForegroundProperty, value);
        }
    }
}