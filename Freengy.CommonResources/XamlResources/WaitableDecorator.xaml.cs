﻿// Created by Laxale 20.10.2016
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
            get { return (string)this.GetValue(WaitableDecorator.IsWaitingPropertyNameProperty); }
            set { this.SetValue(WaitableDecorator.IsWaitingPropertyNameProperty, value); }
        }

        /// <summary>
        /// Sets the name of a property that indicates busy state of a viewmodel
        /// </summary>
        public System.Windows.DataTemplate WaitTemplate 
        {
            get { return (System.Windows.DataTemplate)this.GetValue(WaitableDecorator.WaitTemplateProperty); }
            set { this.SetValue(WaitableDecorator.WaitTemplateProperty, value); }
        }


        public static readonly DependencyProperty WaitTemplateProperty =
            DependencyProperty.Register
            (
                "WaitTemplate",
                typeof(DataTemplate),
                typeof(WaitableDecorator),
                new PropertyMetadata(null)
            );

        public static readonly DependencyProperty IsWaitingPropertyNameProperty =
            DependencyProperty.Register("IsWaitingPropertyName", typeof(string), typeof(WaitableDecorator), new PropertyMetadata("fake"));
    }
}