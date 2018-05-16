// Created by Laxale 21.04.2018
//
//

using System;
using Freengy.Base.Models.Readonly;
using Freengy.Base.Views;
using Freengy.Common.Models;


namespace Freengy.Base.ViewModels 
{
    /// <summary>
    /// Viewmodel for the <see cref="UserAccountView"/>
    /// </summary>
    public class UserAccountViewModel : WaitableViewModel 
    {
        private bool isMyFriend;


        public UserAccountViewModel(UserAccount account) 
        {
            Account = account ?? throw new ArgumentNullException(nameof(account));
        }

        
        /// <summary>
        /// Возвращает или задаёт значение - является ли данный аккаунт моим другом.
        /// </summary>
        public bool IsMyFriend 
        {
            get => isMyFriend;

            set
            {
                if (isMyFriend == value) return;

                isMyFriend = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Detailed user account.
        /// </summary>
        public UserAccount Account { get; }
    }
}