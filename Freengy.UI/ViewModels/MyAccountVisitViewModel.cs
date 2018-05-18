// Created by Laxale 20.04.2018
//
//

using System;
using System.Windows.Documents;

using Freengy.Base.DefaultImpl;
using Freengy.Base.Extensions;
using Freengy.Base.Helpers;
using Freengy.Base.Helpers.Commands;
using Freengy.Base.Interfaces;
using Freengy.Base.Messages;
using Freengy.Base.Models;
using Freengy.Base.Models.Readonly;
using Freengy.UI.Views;
using Freengy.Common.Models;
using Freengy.Base.ViewModels;
using Freengy.Base.Windows;
using Freengy.Common.Helpers;
using Freengy.Common.Helpers.Result;
using Freengy.Networking.Interfaces;
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
        private readonly IImageCacher imageCacher;
        private readonly ILoginController loginController;

        private byte[] currentIconBlob;
        private string currentAvatarPath;
        private uint currentLevelStartExp;
        private uint currentLevelFinishExp;


        public MyAccountVisitViewModel(IImageCacher imageCacher) 
        {
            this.imageCacher = imageCacher;
            loginController = ServiceLocator.Resolve<ILoginController>();
            MyAccountState = loginController.MyAccountState;

            CommandEditAccount = new MyCommand(EditAccountImpl);
            CommandSelectIcon = new MyCommand(SelectIconImpl);

            this.Publish(new MessageRefreshRequired(this));
            this.Publish(new MessageActivityChanged(this, true));
            this.Publish(new MessageInitializeModelRequest(this, ""));
        }

        
        /// <summary>
        /// Команда редактирования данных аккаунта.
        /// </summary>
        public MyCommand CommandEditAccount { get; }

        /// <summary>
        /// Команда выбора иконки аккаунта.
        /// </summary>
        public MyCommand CommandSelectIcon { get; }


        /// <summary>
        /// State of my account.
        /// </summary>
        public AccountState MyAccountState { get; }

        /// <summary>
        /// Возвращает путь к моему текущему аватару.
        /// </summary>
        public string CurrentAvatarPath 
        {
            get => currentAvatarPath;

            private set
            {
                // аватар кэшируется в один и тот же файл, при этом меняется содержимое файла
                //if (currentAvatarPath == value) return;

                currentAvatarPath = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Возвращает блоб моей текущей иконки.
        /// </summary>
        public byte[] CurrentIconBlob 
        {
            get => currentIconBlob;

            private set
            {
                if (currentIconBlob == value) return;

                currentIconBlob = value;

                OnPropertyChanged();
            }
        }

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
        /// Возвращает описание активности в контексте её остановки.
        /// </summary>
        public string CancelDescription { get; } = string.Empty;

        
        /// <summary>
        /// Cancel activity.
        /// </summary>
        /// <returns>Result of a cancel attempt.</returns>
        public Result Cancel() 
        {
            this.Unsubscribe();

            return Result.Ok();
        }


        /// <summary>
        /// Непосредственно логика инициализации, которая выполняется в Initialize().
        /// </summary>
        protected override void InitializeImpl() 
        {
            base.InitializeImpl();

            SetExpirienceValues();
            RaiseCurrentExpChanged();

            var myAvatar = MyAccountState.Account.GetAvatar();
            UpdateAvatarPath(myAvatar);

            this.Subscribe<MessageMyAccountUpdated>(OnMyAccountChanged);
        }


        private void SelectIconImpl() 
        {
            var win = new EmptyCustomToolWindow();
            var content = new SelectIconView();
            win.MainContent = content;

            win.Height = 400;
            win.Width = 300;
            win.Background = new CommonResourceExposer().GetBrush(CommonResourceExposer.LightGrayBrushKey);

            win.Title = "Select account icon";
            
            ServiceLocator.Resolve<ICurtainedExecutor>().ExecuteWithCurtain(KnownCurtainedIds.MainWindowId, () => win.ShowDialog());
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
                Height = 500,
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

        private void SetExpirienceValues() 
        {
            CurrentLevelStartExp = ExpirienceCalculator.GetExpOfLevel(MyAccountState.Account.Level);
            CurrentLevelFinishExp = ExpirienceCalculator.GetExpOfLevel(MyAccountState.Account.Level + 1) - 1;
        }

        private void OnMyAccountChanged(MessageMyAccountUpdated message) 
        {
            lock (this)
            {
                SetExpirienceValues();

                UserAvatarModel avatarModel = message.MyAccountState.Account.GetAvatar();

                UpdateAvatarPath(avatarModel);

                OnPropertyChanged(nameof(MyAccountState));
                RaiseCurrentExpChanged();
            }
        }

        private void UpdateAvatarPath(UserAvatarModel myAvatarModel) 
        {
            if (myAvatarModel == null)
            {
                return;
            }

            Result<string> result = imageCacher.CacheAvatar(myAvatarModel);
            if (result.Success)
            {
                CurrentAvatarPath = result.Value;
            }
        }
    }
}