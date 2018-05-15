// Created by Laxale 15.05.2018
//
//

using System;
using System.Windows;
using System.Windows.Controls;


namespace Freengy.CommonResources.Controls 
{
    /// <summary>
    /// Interaction logic for MyProgressBar.xaml
    /// </summary>
    public partial class MyProgressBar : UserControl
    {
        private Border fillingBorder;


        public MyProgressBar() 
        {
            InitializeComponent();

            SizeChanged += (sender, args) =>
            {
                AdjustFilling();
                args.Handled = true;
            };
        }


        public static readonly DependencyProperty MinimumProperty = 
            DependencyProperty.Register(nameof(Minimum), typeof(double), typeof(MyProgressBar), new PropertyMetadata(0d));

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(nameof(Maximum), typeof(double), typeof(MyProgressBar), new PropertyMetadata(1d));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(double), typeof(MyProgressBar), new PropertyMetadata(0d));

        public static readonly DependencyProperty CornerRadiusProperty = 
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(MyProgressBar));


        /// <summary>
        /// Возвращает или задаёт радиусы рамки прогрессбара.
        /// </summary>
        public CornerRadius CornerRadius 
        {
            get => (CornerRadius) GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        /// <summary>
        /// Возвращает или задаёт минимальное значение прогрессбара.
        /// </summary>
        public double Minimum 
        {
            get => (double) GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        /// <summary>
        /// Возвращает или задаёт максимальное значение прогрессбара.
        /// </summary>
        public double Maximum 
        {
            get => (double)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        /// <summary>
        /// Возвращает или задаёт текущее значение прогрессбара.
        /// </summary>
        public double Value 
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }


        /// <summary>
        /// Invoked whenever the effective value of any dependency property on this <see cref="T:System.Windows.FrameworkElement" /> has been updated. 
        /// The specific dependency property that changed is reported in the arguments parameter. 
        /// Overrides <see cref="M:System.Windows.DependencyObject.OnPropertyChanged(System.Windows.DependencyPropertyChangedEventArgs)" />.</summary>
        /// <param name="e">The event data that describes the property that changed, as well as old and new values.</param>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            ValidateBounds(e);
        }


        private void FillingRect_OnLoaded(object sender, RoutedEventArgs e) 
        {
            fillingBorder = (Border) sender;
            
            fillingBorder.SizeChanged += (send, arg) =>
            {
                arg.Handled = true;
            };

            AdjustFilling();
        }


        private void AdjustFilling() 
        {
            var gainedValue = Value - Minimum;
            //так может быть когда ещё не все свойства установлены при загрузке контрола
            if (gainedValue < 0) return;

            var range = Maximum - Minimum;
            var percentage = (Value - Minimum) / range;

            if (fillingBorder == null) return;

            fillingBorder.Width = this.ActualWidth * percentage;
        }

        private void ValidateBounds(DependencyPropertyChangedEventArgs e) 
        {
            if (e.Property.Name == nameof(Minimum))
            {
                //if (Minimum > Value || Minimum > Maximum) throw new InvalidOperationException("Minimum must not be so great");

                AdjustFilling();
            }

            if (e.Property.Name == nameof(Maximum))
            {
                if (Maximum < Value || Maximum < Minimum) throw new InvalidOperationException("Maximum must not be so small");

                AdjustFilling();
            }

            if (e.Property.Name == nameof(Value))
            {
                if (Value < Minimum || Value > Maximum) throw new InvalidOperationException("Value must not exceed bounds");

                AdjustFilling();
            }
        }
    }
}