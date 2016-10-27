﻿// Created by Laxale 27.10.2016
//
//


namespace Freengy.Base.Helpers 
{
    using System.Windows;


    /// <summary>
    /// Used to pass datacontext to elements that cant use parent's datacontext directly
    /// <para>http://www.thomaslevesque.com/2011/03/21/wpf-how-to-bind-to-data-when-the-datacontext-is-not-inherited/</para>
    /// </summary>
    public class DataContextProxy : Freezable 
    {
        protected override Freezable CreateInstanceCore() 
        {
            return new DataContextProxy();
        }


        public object Data 
        {
            get { return GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Data. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(DataContextProxy), new UIPropertyMetadata(null));
    }
}