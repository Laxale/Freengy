// Created by Laxale 26.10.2016
//
//

using Freengy.Base.Interfaces;
using Freengy.Base.ViewModels;
using Freengy.Common.Models.Readonly;


namespace Freengy.FriendList.ViewModels 
{
    public class AccountStateViewModel : WaitableViewModel 
    {
        public AccountStateViewModel(ITaskWrapper taskWrapper, IGuiDispatcher guiDispatcher, IMyServiceLocator serviceLocator) : 
            base(taskWrapper, guiDispatcher, serviceLocator)
        {

        }


        public AccountState AccountState { get; set; }


        public void RaiseAccountPropertyCahnged() 
        {
            OnPropertyChanged(nameof(AccountState));
        }
    }
}