// Created by Laxale 14.11.2016
//
//


namespace Freengy.CommonResources.XamlResources 
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Collections.Generic;


    /// <summary>
    /// Control represents two-side panel with side-attached panels
    /// </summary>
    public partial class FlipPanel : UserControl 
    {
        public FlipPanel() 
        {
            this.InitializeComponent();
        }

        
        /// <summary>
        /// Template for a panel-slide button
        /// </summary>
        public ControlTemplate SlideButtonTemplate 
        {
            get { return (ControlTemplate)GetValue(SlideButtonTemplateProperty); }
            set { SetValue(SlideButtonTemplateProperty, value); }
        }

        
        public UserControl FrontPanel 
        {
            get { return (UserControl)GetValue(FrontPanelProperty); }
            set { SetValue(FrontPanelProperty, value); }
        }

        public UserControl BackPanel 
        {
            get { return (UserControl)GetValue(BackPanelProperty); }
            set { SetValue(BackPanelProperty, value); }
        }

        public static readonly DependencyProperty FrontPanelProperty =
            DependencyProperty.Register(nameof(FrontPanel), typeof(UserControl), typeof(FlipPanel));

        public static readonly DependencyProperty BackPanelProperty =
            DependencyProperty.Register(nameof(BackPanel), typeof(UserControl), typeof(FlipPanel));

        public static readonly DependencyProperty SlideButtonTemplateProperty =
            DependencyProperty.Register(nameof(SlideButtonTemplate), typeof(ControlTemplate), typeof(FlipPanel));
    }
}
