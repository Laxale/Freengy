// Created by Laxale 05.05.2018
//
//

using System.Windows.Controls;

using Freengy.Base.Attributes;
using Freengy.UI.ViewModels;


namespace Freengy.UI.Views 
{
    /// <summary>
    /// Interaction logic for HeaderToolbarView.xaml
    /// </summary>
    [HasViewModel(typeof(HeaderToolbarViewModel))]
    public partial class HeaderToolbarView : UserControl 
    {
        public HeaderToolbarView() 
        {
            InitializeComponent();
        }
    }
}