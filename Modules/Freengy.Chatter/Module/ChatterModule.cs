﻿// Created by Laxale 01.11.2016
//
//


namespace Freengy.Chatter.Module 
{
    using System;

    using Freengy.Chatter.Views;
    using Freengy.Base.Interfaces;
    
    using Prism.Modularity;

    using Catel.IoC;
    

    public class ChatterModule : IUiModule, IModule 
    {
        #region Singleton

        private static ChatterModule instance;

        private ChatterModule() 
        {

        }

        public static ChatterModule Instance => ChatterModule.instance ?? (ChatterModule.instance = new ChatterModule());

        #endregion Singleton


        public Type ExportedViewType { get; } = typeof(ChatterView);


        public void Initialize() 
        {
            //ServiceLocator.Default.RegisterType<i>
        }
    }
}