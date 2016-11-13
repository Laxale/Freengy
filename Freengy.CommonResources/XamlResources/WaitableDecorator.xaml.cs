// Created by Laxale 20.10.2016
//
//


namespace Freengy.CommonResources.XamlResources 
{
    using System.Windows;
    using System.Windows.Controls;


    public partial class WaitableDecorator : UserControl 
    {
        public WaitableDecorator() 
        {
            this.InitializeComponent();
        }


        /// <summary>
        /// Sets the name of a property that indicates busy state of a viewmodel
        /// </summary>
        public string IsWaitingPropertyName 
        {
            get { return (string)this.GetValue(IsWaitingPropertyNameProperty); }
            set { this.SetValue(IsWaitingPropertyNameProperty, value); }
        }

        /// <summary>
        /// Sets the name of a property that indicates busy state of a viewmodel
        /// </summary>
        public DataTemplate WaitTemplate 
        {
            get { return (DataTemplate)this.GetValue(WaitTemplateProperty); }
            set { this.SetValue(WaitTemplateProperty, value); }
        }


        public static readonly DependencyProperty WaitTemplateProperty =
            DependencyProperty.Register
            (
                "WaitTemplate",
                typeof(DataTemplate),
                typeof(WaitableDecorator)
            );

        public static readonly DependencyProperty IsWaitingPropertyNameProperty =
            DependencyProperty.Register("IsWaitingPropertyName", typeof(string), typeof(WaitableDecorator), new PropertyMetadata("fake"));
    }
}