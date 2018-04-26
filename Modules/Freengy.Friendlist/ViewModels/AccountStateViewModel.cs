// Created by Laxale 26.10.2016
//
//

using Freengy.Base.ViewModels;
using Freengy.Common.Models.Readonly;


namespace Freengy.FriendList.ViewModels 
{
    public class AccountStateViewModel : WaitableViewModel 
    {
        public AccountStateViewModel(AccountState accountState) 
        {
            AccountState = accountState;
        }


        public AccountState AccountState { get; }


        public void RaiseAccountPropertyCahnged() 
        {
            OnPropertyChanged(nameof(AccountState));
        }
    }
}