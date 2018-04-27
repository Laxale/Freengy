// Created by Laxale 23.10.2016
//
//

using System;

using Freengy.Base.Helpers;
using Freengy.Base.Interfaces;
using Freengy.FriendList.Views;
using Freengy.Diagnostics.Interfaces;
using Freengy.FriendList.Diagnostics;

using Catel.IoC;
using Freengy.Common.Helpers;
using Prism.Modularity;


namespace Freengy.FriendList.Module 
{
    public class FriendListModule : IUiModule, IModule 
    {
        public Type ExportedViewType { get; } = typeof (FriendListView);


        public void Initialize() 
        {
            using (new StatisticsDeployer(nameof(FriendListModule)))
            {
                var controller = ServiceLocator.Default.ResolveType<IDiagnosticsController>();
                controller.RegisterCategory(new FriendListDiagnosticsCategory());

                //ServiceLocator.Default.RegisterInstance(FriendStateController.Instance);
            }
        }
    }
}