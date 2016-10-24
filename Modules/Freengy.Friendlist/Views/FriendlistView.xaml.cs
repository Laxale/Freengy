// Created by Laxale 23.10.2016
//
//


namespace Freengy.FriendList.Views 
{
    using System.Windows.Controls;

    using Freengy.FriendList.ViewModels;

    using CatelControl = Catel.Windows.Controls.UserControl;


    public partial class FriendListView : CatelControl 
    {
        public FriendListView() 
        {
            this.InitializeComponent();
        }


        private void FriendList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var friendListSender = sender as ListBox;

            if (friendListSender == null) return;

            var friendListViewModel = friendListSender.DataContext as FriendListViewModel;

            if (friendListViewModel == null) return;

            friendListViewModel.CommandRemoveFriend.RaiseCanExecuteChanged();
        }
    }
}