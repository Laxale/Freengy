// Created by Laxale 18.04.2018
//
//


using System.Windows;
using System.Windows.Input;

using Freengy.Base.Attributes;
using Freengy.Base.Helpers;
using Freengy.FriendList.ViewModels;


namespace Freengy.FriendList.Views 
{
    /// <summary>
    /// Window to search users and interact with them.
    /// </summary>
    [HasViewModel(typeof(AddNewFriendViewModel))]
    public partial class AddNewFriendWindow : Window 
    {
        /// <inheritdoc />
        /// <summary>
        /// Construct new <see cref="T:Freengy.FriendList.Views.AddNewFriendWindow" />.
        /// </summary>
        public AddNewFriendWindow() 
        {
            InitializeComponent();
        }


        private void AddNewFriendWindow_OnKeyDown(object sender, KeyEventArgs e) 
        {
            new KeyHandler(e).ExecuteOnEscapePressed(Close);
        }
    }
}