// Created by Laxale 10.05.2018
//
//

using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

using Freengy.Base.Helpers.Commands;
using Freengy.Base.Models.Readonly;
using Freengy.Base.ViewModels;
using Freengy.Common.Constants;
using Freengy.Common.Interfaces;
using Freengy.Common.Models;
using Freengy.Networking.Constants;
using Freengy.Networking.Helpers;
using Freengy.Networking.Interfaces;


namespace Freengy.UI.ViewModels 
{
    internal class EditMyAccountViewModel : WaitableViewModel 
    {
        private readonly string mySessionToken;
        private readonly string myInitialName;
        private readonly string myInitialStatus;
        private readonly string myInitialDescription;
        private readonly ILoginController loginController;

        private string myName;
        private string myStatus;
        private string myDescription;
        private SolidColorBrush myColor;

        public event Action SavedChanges = () => { };


        public EditMyAccountViewModel() 
        {
            loginController = ServiceLocator.Resolve<ILoginController>();
            UserAccount myAaccount = loginController.MyAccountState.Account;

            CommandSave = new MyCommand(SaveImpl, () => IsChanged);

            MyName = myAaccount.Name;
            mySessionToken = loginController.MySessionToken;

            MyStatus = "Test status. Not working yet";

            myInitialName = MyName;
            myInitialStatus = MyStatus;
            myInitialDescription = MyDescription;
        }


        /// <summary>
        /// Команда сохранить изменения.
        /// </summary>
        public MyCommand CommandSave { get; }


        /// <summary>
        /// Возвращает значение флага - были ли сохранены изменения на сервере.
        /// </summary>
        public bool Saved { get; private set; }

        /// <summary>
        /// Возвращает значение - изменились ли мои данные.
        /// </summary>
        public bool IsChanged => MyName != myInitialName || MyStatus != myInitialStatus || MyDescription != myInitialDescription;

        /// <summary>
        /// Возвращает или задаёт название моего аккаунта.
        /// </summary>
        public string MyName 
        {
            get => myName;

            set
            {
                if (myName == value) return;

                myName = value;
                CommandSave.RaiseCanExecuteChanged();

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Возвращает или задаёт описание моего аккаунта.
        /// </summary>
        public string MyDescription 
        {
            get => myDescription;

            set
            {
                if (myDescription == value) return;

                myDescription = value;
                CommandSave.RaiseCanExecuteChanged();

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Возвращает или задаёт значение моего статуса.
        /// </summary>
        public string MyStatus 
        {
            get => myStatus;

            set
            {
                if (myStatus == value) return;

                myStatus = value;
                CommandSave.RaiseCanExecuteChanged();

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Возвращает или задаёт цвет моего аккаунта.
        /// </summary>
        public SolidColorBrush MyColor 
        {
            get => myColor;

            set
            {
                if (myColor == value) return;

                myColor = value;

                OnPropertyChanged();
            }
        }


        private async void SaveImpl() 
        {
            SetBusyState("Saving on server");
            
            try
            {
                 await InvokeSave();
            }
            catch (Exception ex)
            {
                ReportMessage(ex.Message);
            }
        }

        private async Task InvokeSave() 
        {
            using (var actor = ServiceLocator.Resolve<IHttpActor>())
            {
                actor
                    .SetRequestAddress(Url.Http.Edit.EditAccount)
                    .SetClientSessionToken(mySessionToken)
                    .AddHeader(FreengyHeaders.Client.ClientIdHeaderName, loginController.MyAccountState.Account.Id.ToString());

                var changeRequest = new EditAccountModel
                {
                    NewDescription = MyDescription,
                    NewEmoteStatus = MyStatus,
                    NewName = MyName,
                    NewImageBlob = null
                };

                await actor.PostAsync(changeRequest);

                ClearBusyState();
                if (actor.ResponceMessage.StatusCode != HttpStatusCode.Accepted)
                {
                    ReportMessage($"Failed to save changes: {actor.ResponceMessage.StatusCode}");
                }
                else
                {
                    Saved = true;
                    SavedChanges.Invoke();
                }
            }
        }
    }
}