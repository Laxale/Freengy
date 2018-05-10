// Created by Laxale 10.05.2018
//
//

using System.Windows.Controls;

using Freengy.UI.ViewModels;
using Freengy.Base.Attributes;


namespace Freengy.UI.Views 
{
    /// <summary>
    /// Interaction logic for EditMyAccountView.xaml
    /// </summary>
    [HasViewModel(typeof(EditMyAccountViewModel))]
    public partial class EditMyAccountView : UserControl 
    {
        public EditMyAccountView() 
        {
            InitializeComponent();
        }
    }
}