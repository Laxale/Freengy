// Created by Laxale 08.05.2018
//
//

using System.Windows.Controls;

using Freengy.Base.Attributes;
using Freengy.FriendList.ViewModels;


namespace Freengy.FriendList.Views
{
    /// <summary>
    /// Interaction logic for SearchFriendsView.xaml
    /// </summary>
    [HasViewModel(typeof(SearchFriendsViewModel))]
    public partial class SearchFriendsView : UserControl 
    {
        public SearchFriendsView() 
        {
            InitializeComponent();
        }
    }
}