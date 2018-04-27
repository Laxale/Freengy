// Created by Laxale 23.10.2016
//
//

using System;

using Freengy.Base.Helpers;
using Freengy.Base.Interfaces;
using Freengy.Common.Helpers;
using Freengy.Diagnostics.Interfaces;
using Freengy.GameList.Views;
using Freengy.GameList.Diagnostics;

using Catel.IoC;
using Prism.Modularity;


namespace Freengy.GameList.Module 
{
    public class GameListModule : IUiModule, IModule 
    {
        public Type ExportedViewType { get; } = typeof (GameListView);


        public void Initialize() 
        {
            using (new StatisticsDeployer(nameof(GameListModule)))
            {
                var controller = ServiceLocator.Default.ResolveType<IDiagnosticsController>();
                controller.RegisterCategory(new GameListDiagnosticsCategory());
            }
        }
    }
}