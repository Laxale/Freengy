// Created by Laxale 21.04.2018
//
//

using System;

using Freengy.Base.Views;
using Freengy.Common.Models;
using Freengy.Common.Models.Readonly;


namespace Freengy.Base.ViewModels 
{
    /// <summary>
    /// Viewmodel for the <see cref="UserAccountView"/>
    /// </summary>
    public class UserAccountViewModel : WaitableViewModel 
    {
        public UserAccountViewModel(UserAccount account) 
        {
            Account = account ?? throw new ArgumentNullException(nameof(account));
        }


        /// <summary>
        /// Detailed user account.
        /// </summary>
        public UserAccount Account { get; }
    }
}