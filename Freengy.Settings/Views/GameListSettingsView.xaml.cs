// Created by Laxale 27.11.2016
//
//


namespace Freengy.Settings.Views 
{
    using System.Windows.Controls;

    using Freengy.Settings.Helpers;
    using Freengy.Settings.Messages;

    using Catel.IoC;
    using Catel.Messaging;

    
    public partial class GameListSettingsView : UserControl 
    {
        public GameListSettingsView() 
        {
            // this is the only fking way to avoid design-time exceptions when setting datacontext
            // by prism autowire or d:DesignInstance
            var contextRequestMessage = new MessageRequestContext(this);
            ServiceLocator.Default.ResolveType<IMessageMediator>().SendMessage(contextRequestMessage);

            this.InitializeComponent();
        }
    }
}