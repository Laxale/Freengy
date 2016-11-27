// Created by Laxale 26.11.2016
//
//


namespace Freengy.Settings.Configuration 
{
    using System;

    using Freengy.Settings.Interfaces;
    using Freengy.Settings.DefaultImpl;

    using NHibernate;
    using NHibernate.Cfg;
    using NHibernate.Mapping.ByCode;

    
    internal class Initializer 
    {
        private static readonly object Locker = new object();

        private static ISessionFactory sessionFactory;
        private static ISessionFactory SessionFactory 
        {
            get
            {
                lock (Initializer.Locker)
                {
                    if (Initializer.sessionFactory == null)
                    {
                        Initializer.Configure();
                    }

                    return Initializer.sessionFactory;
                }
            }
        }

        
        private static void Configure() 
        {
            var mapper = new ModelMapper();
            var config = new Configuration();
            var mappings = SettingsFacade.FullInstance.GetRegisteredMappings();

            config.Configure();
            
            foreach (var mapping in mappings)
            {
                mapper.AddMapping(mapping);

                config.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());
            }

            Initializer.sessionFactory = config.BuildSessionFactory();
        }


        public static ISession OpenSession() 
        {
            return Initializer.SessionFactory.OpenSession();
        }

        public static void CloseSession() 
        {
            SessionFactory.Close();
        }
    }
}