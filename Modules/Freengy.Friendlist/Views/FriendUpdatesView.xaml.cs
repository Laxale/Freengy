// Created by Laxale 11.05.2018
//
//

using System.Windows.Controls;

using Freengy.Base.Attributes;
using Freengy.FriendList.ViewModels;


namespace Freengy.FriendList.Views 
{
    /// <summary>
    /// Interaction logic for FriendUpdatesView.xaml
    /// </summary>
    [HasViewModel(typeof(FriendUpdatesViewModel))]
    public partial class FriendUpdatesView : UserControl 
    {
        public FriendUpdatesView() 
        {
            InitializeComponent();
        }
    }
}