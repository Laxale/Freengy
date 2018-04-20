// Created by Laxale 20.04.2018
//
//

using Freengy.Base.Helpers;
using Freengy.Base.Attributes;
using Freengy.UI.ViewModels;


namespace Freengy.UI.Views 
{
    /// <summary>
    /// Interaction logic for MyAccountVisitView.xaml
    /// </summary>
    [HasViewModel(typeof(MyAccountVisitViewModel))]
    public partial class MyAccountVisitView  
    {
        public MyAccountVisitView() 
        {
            InitializeComponent();

            new ViewModelWierer().Wire(this);
        }
    }
}