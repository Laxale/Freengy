// Created by Laxale 01.11.2016
//
//

using System.Linq;
using System.Windows.Data;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using Freengy.Chatter.Views;
using Freengy.Base.ViewModels;
using Freengy.Base.Interfaces;
using Freengy.Base.Chat.Interfaces;

using Catel.IoC;
using Catel.MVVM;
using Freengy.Base.Helpers;


namespace Freengy.Chatter.ViewModels 
{
    using Freengy.Base.Messages;


    /// <summary>
    /// Viewmodel for <see cref="ChatterView"/>.
    /// </summary>
    public class ChatterViewModel : WaitableViewModel 
    {
        private readonly IChatSessionFactory chatSessionFactory;
        private readonly ObservableCollection<ChatSessionViewModel> currentChatSessions = new ObservableCollection<ChatSessionViewModel>();


        public ChatterViewModel() 
        {
            chatSessionFactory = ServiceLocatorProperty.ResolveType<IChatSessionFactory>();

            ChatSessions = CollectionViewSource.GetDefaultView(currentChatSessions);

            FillSomeSessions();

            Mediator.SendMessage(new MessageInitializeModelRequest(this, "Loading sessions"));
        }

        protected override void SetupCommands() 
        {
            
        }

        
        /// <summary>
        /// Command to create a new chat session.
        /// </summary>
        public MyCommand CommandCreateSession { get; private set; }


        /// <summary>
        /// CuUrrent chat sessions collection.
        /// </summary>
        public ICollectionView ChatSessions { get; private set; }


        private void FillSomeSessions() 
        {
            var firstSession = chatSessionFactory.CreateInstance("First test session", "First test session");
            var seconSession = chatSessionFactory.CreateInstance("Secon test session", "Secon test session");

            var firstViewModel = new ChatSessionViewModel(firstSession);
            var seconViewModel = new ChatSessionViewModel(seconSession);

            currentChatSessions.Add(firstViewModel);
            currentChatSessions.Add(seconViewModel);
        }

        private bool CanCreateSession => true;


        private void CommandCreateSessionImpl() 
        {
            // create new!
        }
    }
}