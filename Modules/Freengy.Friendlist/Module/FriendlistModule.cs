// Created by Laxale 23.10.2016
//
//

using System;

using Freengy.Base.Interfaces;
using Freengy.FriendList.Views;
using Freengy.Diagnostics.Interfaces;
using Freengy.FriendList.Diagnostics;
using Freengy.FriendList.ViewModels;

using Prism.Modularity;

using Catel.IoC;
using Catel.Services;


namespace Freengy.FriendList.Module 
{
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