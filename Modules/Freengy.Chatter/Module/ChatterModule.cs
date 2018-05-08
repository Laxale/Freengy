// Created by Laxale 01.11.2016
//
//

using System;

using Freengy.Chatter.Views;
using Freengy.Base.Helpers;
using Freengy.Base.Interfaces;
using Freengy.Common.Helpers;

using Prism.Modularity;


namespace Freengy.Chatter.Module 
{
    public class ChatterModule : IUiModule, IModule 
    {
        private static ChatterModule instance;


        private ChatterModule() { }


        public static ChatterModule Instance => instance ?? (instance = new ChatterModule());


        public Type ExportedViewType { get; } = typeof(ChatterView);


        public void Initialize() 
        {
            using (new StatisticsDeployer(nameof(ChatterModule)))
            {
                
            }
        }
    }
}