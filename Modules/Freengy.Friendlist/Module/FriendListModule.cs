// Created by Laxale 23.10.2016
//
//

using System;

using Freengy.Base.Helpers;
using Freengy.Base.Interfaces;
using Freengy.FriendList.Views;
using Freengy.Diagnostics.Interfaces;
using Freengy.FriendList.Diagnostics;
using Freengy.Base.DefaultImpl;
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
                var controller = MyServiceLocator.Instance.Resolve<IDiagnosticsController>();
                controller.RegisterCategory(new FriendListDiagnosticsCategory());

                //ServiceLocator.Default.RegisterInstance(FriendStateController.Instance);
            }
        }
    }
}