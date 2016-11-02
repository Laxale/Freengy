// Created by Laxale 01.11.2016
//
//


namespace Freengy.Chatter.ViewModels 
{
    using System.ComponentModel;
    using System.Collections.ObjectModel;

    using Freengy.Base.ViewModels;
    using Freengy.Base.Interfaces;


    public class ChatterViewModel : WaitableViewModel 
    {
        private readonly ObservableCollection<IChatSession> currentChatSessions = new ObservableCollection<IChatSession>();


        protected override void SetupCommands() 
        {
            
        }


        public ICollectionView GameList { get; private set; }
    }
}