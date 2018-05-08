// Created by Laxale 01.11.2016
//
//

using System;
using System.Linq;
using System.Windows.Data;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using Freengy.Base.Messages;
using Freengy.Base.Helpers.Commands;
using Freengy.Base.ViewModels;
using Freengy.Base.Chat.Interfaces;
using Freengy.Base.DefaultImpl;
using Freengy.Chatter.Views;
using Freengy.Networking.DefaultImpl;

using Freengy.Base.Interfaces;


namespace Freengy.Chatter.ViewModels 
{
    /// <summary>
    /// Viewmodel for <see cref="ChatterView"/>.
    /// </summary>
    public class ChatterViewModel : WaitableViewModel 
    {
        private readonly IChatSessionFactory chatSessionFactory;
        private readonly ChatMessageSender messageSender = new ChatMessageSender();
        private readonly ObservableCollection<ChatSessionViewModel> chatSessions = new ObservableCollection<ChatSessionViewModel>();


        public ChatterViewModel(ITaskWrapper taskWrapper, IGuiDispatcher guiDispatcher, IChatSessionFactory chatSessionFactory, IMyServiceLocator serviceLocator) : 
            base(taskWrapper, guiDispatcher, serviceLocator)
        {
            this.chatSessionFactory = chatSessionFactory;

            ChatSessions = CollectionViewSource.GetDefaultView(chatSessions);

            FillSomeSessions();

            this.Subscribe<MessageChatSessionChanged>(OnChatSessionChanged);
            this.Publish(new MessageInitializeModelRequest(this, "Loading sessions"));
        }

        
        /// <summary>
        /// Command to create a new chat session.
        /// </summary>
        public MyCommand CommandCreateSession { get; }


        /// <summary>
        /// CuUrrent chat sessions collection.
        /// </summary>
        public ICollectionView ChatSessions { get; }


        private bool CanCreateSession => true;


        private void FillSomeSessions() 
        {
            chatSessionFactory.SetNetworkInterface((msg, acc) => { });
            var firstSession = chatSessionFactory.CreateInstance("First test session", "First test session");
            var seconSession = chatSessionFactory.CreateInstance("Secon test session", "Secon test session");
            chatSessionFactory.SetNetworkInterface(messageSender.SendMessageToFriend);

            var firstViewModel = new ChatSessionViewModel(firstSession);
            var seconViewModel = new ChatSessionViewModel(seconSession);

            chatSessions.Add(firstViewModel);
            chatSessions.Add(seconViewModel);
        }

        private void CommandCreateSessionImpl() 
        {
            // create new!
        }

        private void OnChatSessionChanged(MessageChatSessionChanged message) 
        {
            if (message.IsCreated)
            {
                if (chatSessions.All(viewModel => viewModel.Session.Id != message.Session.Id))
                {
                    GUIDispatcher.InvokeOnGuiThread(() => chatSessions.Add(new ChatSessionViewModel(message.Session)));
                }
            }
            else
            {
                ChatSessionViewModel viewModelToRemove = chatSessions.FirstOrDefault(viewModel => viewModel.Session.Id == message.Session.Id);

                if (viewModelToRemove != null)
                {
                    GUIDispatcher.InvokeOnGuiThread(() => chatSessions.Remove(viewModelToRemove));
                }
            }
        }
    }
}