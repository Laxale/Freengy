// Created by Laxale 26.10.2016
//
//

using Freengy.Base.Interfaces;
using Freengy.Base.Models.Readonly;
using Freengy.Base.ViewModels;


namespace Freengy.FriendList.ViewModels 
{
    public class AccountStateViewModel : WaitableViewModel 
    {
        private bool hasNewMessages;


        public AccountStateViewModel(ITaskWrapper taskWrapper, IGuiDispatcher guiDispatcher, IMyServiceLocator serviceLocator) : 
            base(taskWrapper, guiDispatcher, serviceLocator)
        {

        }


        public bool HasNewMessages 
        {
            get => hasNewMessages;

            set
            {
                if (hasNewMessages == value) return;

                hasNewMessages = value;

                OnPropertyChanged();
            }
        }

        public AccountState AccountState { get; set; }


        public void RaiseAccountPropertyCahnged() 
        {
            OnPropertyChanged(nameof(AccountState));
        }
    }
}