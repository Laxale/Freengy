// Created by Laxale 23.10.2016
//
//


namespace Freengy.Friendlist.Views 
{
    using System.Windows.Controls;

    using Freengy.Friendlist.ViewModels;

    using CatelControl = Catel.Windows.Controls.UserControl;


    public partial class FriendlistView : CatelControl 
    {
        public FriendlistView() 
        {
            this.InitializeComponent();
        }


        private void FriendList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var friendListSender = sender as ListBox;

            if (friendListSender == null) return;

            var friendListViewModel = friendListSender.DataContext as FriendlistViewModel;

            if (friendListViewModel == null) return;

            friendListViewModel.CommandRemoveFriend.RaiseCanExecuteChanged();
        }
    }
}