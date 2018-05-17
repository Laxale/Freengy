// Created by Laxale 23.10.2016
//
//

using System.Windows;
using System.Windows.Controls;
using Freengy.Base.Attributes;
using Freengy.Base.Extensions;
using Freengy.Base.Helpers;
using Freengy.Base.Models;
using Freengy.FriendList.ViewModels;


namespace Freengy.FriendList.Views 
{
    [HasViewModel(typeof(FriendListViewModel))]
    public partial class FriendListView 
    {
        public FriendListView() 
        {
            InitializeComponent();
        }


        private void FriendAvatarImage_OnLoaded(object sender, RoutedEventArgs e) 
        {
            var image = (Image) sender;
            var context = (AccountStateViewModel)image.DataContext;

            UserAvatarModel friendAvatar = context.AccountState.Account.GetAvatar();

            if (friendAvatar != null)
            {
                image.Source = new ImageLoader().LoadBitmapImage(friendAvatar.AvatarPath).Value;
            }
        }
    }
}