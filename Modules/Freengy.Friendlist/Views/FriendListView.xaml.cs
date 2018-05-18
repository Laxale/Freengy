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

using Prism.Regions;


namespace Freengy.FriendList.Views 
{
    /// <summary>
    /// Вьюмодель для <see cref="FriendListView"/>.
    /// </summary>
    [RegionMemberLifetime(KeepAlive = false)]
    [HasViewModel(typeof(FriendListViewModel))]
    public partial class FriendListView 
    {
        public FriendListView() 
        {
            InitializeComponent();
        }

        ~FriendListView()
        {
            var t = 0;
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