// Created by Laxale 20.04.2018
//
//

using System;
using System.Windows.Documents;

using Freengy.Base.DefaultImpl;
using Freengy.Base.Helpers;
using Freengy.Base.Helpers.Commands;
using Freengy.Base.Interfaces;
using Freengy.Base.Messages;
using Freengy.UI.Views;
using Freengy.Common.Models;
using Freengy.Base.ViewModels;
using Freengy.Base.Windows;
using Freengy.Common.Helpers;
using Freengy.Common.Helpers.Result;
using Freengy.Networking.Interfaces;
using Freengy.Common.Models.Readonly;
using Freengy.CommonResources;
using Freengy.Localization;
using Freengy.Networking.Messages;


namespace Freengy.UI.ViewModels 
{
    /// <summary>
    /// Viewmodel for <see cref="MyAccountVisitView"/>.
    /// </summary>
    public class MyAccountVisitViewModel : WaitableViewModel, IUserActivity 
    {
        private readonly ILoginController loginController;

        private uint currentLevelStartExp;
        private uint currentLevelFinishExp;


        public MyAccountVisitViewModel() 
        {
            loginController = ServiceLocator.Resolve<ILoginController>();
            MyAccountState = loginController.MyAccountState;

            CommandEditAccount = new MyCommand(EditAccountImpl);

            SetExpirienceBounds();
            RaiseCurrentExpChanged();

            this.Subscribe<MessageMyAccountUpdated>(OnMyAccountChanged);

            this.Publish(new MessageActivityChanged(this, true));
        }


        /// <summary>
        /// Команда редактирования данных аккаунта.
        /// </summary>
        public MyCommand CommandEditAccount { get; }


        /// <summary>
        /// State of my account.
        /// </summary>
        public AccountState MyAccountState { get; }

        /// <summary>
        /// Возвращает начальное значение опыта для данного уровня аккаунта.
        /// </summary>
        public uint CurrentLevelStartExp 
        {
            get => currentLevelStartExp;

            private set
            {
                if (currentLevelStartExp == value) return;

                currentLevelStartExp = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Возвращает конечное значение опыта для данного уровня аккаунта.
        /// </summary>
        public uint CurrentLevelFinishExp 
        {
            get => currentLevelFinishExp;

            private set
            {
                if (currentLevelFinishExp == value) return;

                currentLevelFinishExp = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Возвращает текущее количество опыта аккаунта.
        /// </summary>
        public uint CurrentExp => MyAccountState.Account.GetCurrentExp();

        /// <summary>
        /// Возвращает процентное отношение текущего опыта к конечному опыту для данного уровня.
        /// </summary>
        public uint CurrentExpPercentage => 
            CurrentExp == 0 ? 
                0 : 
                (uint)(100 * ((CurrentExp - CurrentLevelStartExp) / (double)(CurrentLevelFinishExp - currentLevelStartExp)));
        
        /// <summary>
        /// Возвращает значение - можно ли остановить данную активити без ведома юзера.
        /// </summary>
        public bool CanCancelInSilent { get; } = true;

        /// <summary>
        /// Cancel activity.
        /// </summary>
        /// <returns>Result of a cancel attempt.</returns>
        public Result Cancel() 
        {
            this.Unsubscribe();

            return Result.Ok();
        }


        private void EditAccountImpl() 
        {
            var content = new EditMyAccountView();
            var brush = new CommonResourceExposer().GetBrush(CommonResourceExposer.LightGrayBrushKey);
            var dataContext = (EditMyAccountViewModel) content.DataContext;

            var win = new EmptyCustomToolWindow
            {
                Title = StringResources.EditAccountData,
                MainContent = content,
                MinHeight = 300,
                MinWidth = 400,
                Background = brush
            };

            void OnSaved()
            {
                win.Close();
            }

            dataContext.SavedChanges += OnSaved;
            ServiceLocator.Resolve<ICurtainedExecutor>().ExecuteWithCurtain(KnownCurtainedIds.MainWindowId, () => win.ShowDialog());
            dataContext.SavedChanges -= OnSaved;

            if (dataContext.Saved)
            {
                loginController.SyncMyAccount();
            }
        }

        private void RaiseCurrentExpChanged() 
        {
            OnPropertyChanged(nameof(CurrentExp));
            OnPropertyChanged(nameof(CurrentExpPercentage));
        }

        private void SetExpirienceBounds() 
        {
            CurrentLevelStartExp = ExpirienceCalculator.GetExpOfLevel(MyAccountState.Account.Level);
            CurrentLevelFinishExp = ExpirienceCalculator.GetExpOfLevel(MyAccountState.Account.Level + 1) - 1;
        }

        private void OnMyAccountChanged(MessageMyAccountUpdated message) 
        {
            SetExpirienceBounds();

            OnPropertyChanged(nameof(MyAccountState));
            RaiseCurrentExpChanged();
        }
    }
}