// Created by Laxale 23.10.2016
//
//


namespace Freengy.FriendList.Module 
{
    using System;

    using Freengy.Base.Interfaces;
    using Freengy.FriendList.Views;
    using Freengy.Diagnostics.Interfaces;
    using Freengy.FriendList.Diagnostics;

    using Prism.Modularity;

    using Catel.IoC;


    public class FriendListModule : IUiModule, IModule 
    {
        public Type ExportedViewType { get; } = typeof (FriendListView);


        public void Initialize() 
        {
            var controller = ServiceLocator.Default.ResolveType<IDiagnosticsController>();
            controller.RegisterCategory(new FriendListDiagnosticsCategory());
        }
    }
}