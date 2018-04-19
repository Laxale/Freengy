// Created by Laxale 18.04.2018
//
//


using System.Windows.Input;

using Freengy.Base.Helpers;


namespace Freengy.FriendList.Views 
{
    /// <summary>
    /// Window to search users and interact with them.
    /// </summary>
    public partial class AddNewFriendWindow : Catel.Windows.Window 
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