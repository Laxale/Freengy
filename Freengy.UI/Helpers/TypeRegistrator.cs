﻿// Created by Laxale 22.10.2016
//
//

using System;

using Freengy.Base.Interfaces;
using Freengy.Base.DefaultImpl;


namespace Freengy.UI.Helpers 
{
    /// <summary>
    /// Registers interfaces and implementations to service loctor
    /// TODO: may be move to common IModule implementation?
    /// </summary>
    public sealed class TypeRegistrator : IRegistrator
    {
        #region Singleton

        private static TypeRegistrator instance;

        private TypeRegistrator()
        {

        }

        public static TypeRegistrator Instance => TypeRegistrator.instance ?? (TypeRegistrator.instance = new TypeRegistrator());

        #endregion Singleton
        

        public bool IsRegistered { get; private set; }

        public void Register() 
        {
            MyServiceLocator.Instance.RegisterInstance(UiNavigator.Instance);
            
            this.IsRegistered = true;
        }

        public void Unregister()
        {
            throw new NotImplementedException();
            //this.Registered = false;
        }
    }
}