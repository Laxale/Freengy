// Created by Laxale 01.11.2016
//
//


namespace Freengy.Chatter.ViewModels 
{
    using System.Linq;
    using System.Windows.Data;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using System.Collections.ObjectModel;

    using Freengy.Base.ViewModels;
    using Freengy.Base.Interfaces;
    using Freengy.Base.Chat.Interfaces;

    using Catel.IoC;
    using Catel.MVVM;
    

    public class ChatterViewModel : WaitableViewModel 
    {
        private readonly IChatMessageFactory chatMessageFactory;
        private readonly IChatSessionFactory chatSessionFactory;
        private readonly ObservableCollection<ChatSessionViewModel> currentChatSessions = new ObservableCollection<ChatSessionViewModel>();


        public ChatterViewModel() 
        {
            this.chatMessageFactory = base.serviceLocator.ResolveType<IChatMessageFactory>();
            this.chatSessionFactory = base.serviceLocator.ResolveType<IChatSessionFactory>();

            this.ChatSessions = CollectionViewSource.GetDefaultView(this.currentChatSessions);

            this.FillSomeSessions();
        }

        protected override void SetupCommands() 
        {
            
        }

        protected override async Task InitializeAsync() 
        {
            await base.InitializeAsync();

            // smth later

        }


        #region commands

        public Command CommandCreateSession { get; private set; }
        
        #endregion commands

        
        public ICollectionView ChatSessions { get; private set; }


        private void FillSomeSessions() 
        {
            var firstSession = this.chatSessionFactory.CreateInstance("First test session", "First test session");
            var seconSession = this.chatSessionFactory.CreateInstance("Secon test session", "Secon test session");

            var firstDecorator = new ChatSessionViewModel(firstSession);
            var seconDecorator = new ChatSessionViewModel(seconSession);

            this.currentChatSessions.Add(firstDecorator);
            this.currentChatSessions.Add(seconDecorator);
        }



        private void CommandCreateSessionImpl() 
        {
            // create new!
        }
        private bool CanCreateSession => true;
    }
}