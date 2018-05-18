// Created by Laxale 11.05.2018
//
//

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

using Freengy.Base.DefaultImpl;
using Freengy.Base.Interfaces;
using Freengy.Base.Messages;
using Freengy.Base.Models.Update;
using Freengy.Base.ViewModels;
using Freengy.Common.Helpers.Result;
using Freengy.FriendList.Views;
using Freengy.Networking.Messages;


namespace Freengy.FriendList.ViewModels 
{
    /// <summary>
    /// Вьюмодель для <see cref="FriendUpdatesView"/>.
    /// </summary>
    internal class FriendUpdatesViewModel : WaitableViewModel, IUserActivity 
    {
        private readonly ObservableCollection<FriendUpdate> friendUpdates = new ObservableCollection<FriendUpdate>();

        public event Action GotNewUpdate = () => { };


        public FriendUpdatesViewModel() 
        {
            FriendUpdates = CollectionViewSource.GetDefaultView(friendUpdates);

            this.Publish(new MessageRefreshRequired(this));
            this.Publish(new MessageActivityChanged(this, true));
        }


        /// <summary>
        /// Возвращает значение - можно ли остановить данную активити без ведома юзера.
        /// </summary>
        public bool CanCancelInSilent { get; } = true;

        /// <summary>
        /// Возвращает описание активности в контексте её остановки.
        /// </summary>
        public string CancelDescription { get; } = string.Empty;

        /// <summary>
        /// Возвращает коллекцию апдейтов друзей.
        /// </summary>
        public ICollectionView FriendUpdates { get; }


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
        /// Обновить вьюмодель.
        /// </summary>
        public override void Refresh() 
        {
            base.Refresh();

            GUIDispatcher.InvokeOnGuiThread(friendUpdates.Clear);

            this.Subscribe<MessageFriendUpdates>(OnFriendUpdate);
        }


        private void OnFriendUpdate(MessageFriendUpdates message) 
        {
            GUIDispatcher.BeginInvokeOnGuiThread(() =>
            {
                friendUpdates.AddRange(message.Updates);
                GotNewUpdate.Invoke();
            });
        }
    }
}