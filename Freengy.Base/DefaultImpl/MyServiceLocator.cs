// Created by Laxale 08.05.2018
//
//


using System;
using System.Collections.Generic;
using Freengy.Base.Interfaces;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Modularity;

using Microsoft.Practices.Unity;
using Microsoft.Practices.ServiceLocation;


namespace Freengy.Base.DefaultImpl 
{
    public class MyServiceLocator : IMyServiceLocator 
    {
        private static readonly object Locker = new object();

        private static MyServiceLocator instance;

        private IUnityContainer container;


        private MyServiceLocator() { }


        /// <summary>
        /// Единственный инстанс <see cref="MyServiceLocator"/>.
        /// </summary>
        public static MyServiceLocator Instance 
        {
            get
            {
                lock (Locker)
                {
                    return instance ?? (instance = new MyServiceLocator());
                }
            }
        }


        public void ConfigureContainer(IUnityContainer container) 
        {
            this.container = container ?? throw new ArgumentNullException(nameof(container));

            RegisterInstance((IMyServiceLocator)this);
        }

        public T Resolve<T>() where T : class 
        {
            return container.Resolve<T>();
        }

        public T ResolveWithParameters<T>(params object[] parameters) where T : class 
        {
            var length = parameters.Length;
            var overrides = new ResolverOverride[length];

            for (int index = 0; index < length; index++)
            {
                var parameterOverride = new ParameterOverride("", parameters[index]);

                overrides[index] = parameterOverride;
            }
            
            return container.Resolve<T>(overrides);
        }

        public void RegisterIfNotRegistered<TInterface, TImplementer>() where TImplementer : TInterface 
        {
            if (container.IsRegistered<TInterface>()) return;

            container.RegisterType(typeof(TInterface), typeof(TImplementer));
        }

        public void RegisterInstance<T>(T instance) where T : class 
        {
            container.RegisterInstance(typeof(T), instance);
        }
    }
}