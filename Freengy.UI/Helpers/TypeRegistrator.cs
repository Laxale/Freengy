// Created by Laxale 22.10.2016
//
//


namespace Freengy.UI.Helpers 
{
    using System;

    using Catel.IoC;

    using Freengy.Base.Interfaces;
    using Freengy.Networking.Interfaces;
    using Freengy.Networking.DefaultImpl;


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
            ServiceLocator.Default.RegisterInstance<ILoginController>(LoginController.Instance);
            ServiceLocator.Default.RegisterType<ILoginParameters, LoginParameters>(RegistrationType.Transient);

            this.IsRegistered = true;
        }

        public void Unregister()
        {
            throw new NotImplementedException();
            this.IsRegistered = false;
        }
    }
}