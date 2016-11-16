// Created by Laxale 16.11.2016
//
//


namespace Freengy.CommonResources.Controls 
{
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Controls;
    

    public partial class WatermarkTextBox : TextBox 
    {
        public WatermarkTextBox() 
        {
            InitializeComponent();
        }


        public string WatermarkText 
        {
            get { return (string)GetValue(WatermarkTextProperty); }
            set { SetValue(WatermarkTextProperty, value); }
        }

        public SolidColorBrush WatermarkForeground 
        {
            get { return (SolidColorBrush)GetValue(WatermarkForegroundProperty); }
            set { SetValue(WatermarkForegroundProperty, value); }
        }

        public SolidColorBrush MouseOverBrush 
        {
            get { return (SolidColorBrush)GetValue(WatermarkForegroundProperty); }
            set { SetValue(WatermarkForegroundProperty, value); }
        }


        public static readonly DependencyProperty WatermarkTextProperty =
            DependencyProperty.Register(nameof(WatermarkText), typeof(string), typeof(WatermarkTextBox));

        public static readonly DependencyProperty WatermarkForegroundProperty =
            DependencyProperty.Register(nameof(WatermarkForeground), typeof(SolidColorBrush), typeof(WatermarkTextBox));

        public static readonly DependencyProperty MouseOverBrushProperty =
            DependencyProperty.RegisterAttached(nameof(MouseOverBrush), typeof(SolidColorBrush), typeof(WatermarkTextBox));
    }
}