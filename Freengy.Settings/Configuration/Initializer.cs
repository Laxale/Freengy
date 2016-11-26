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
        public static ISessionFactory SessionFactory { get; private set; }


        public static ISession Session 
        {
            get;
        }


        public static void Configure() 
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

            Initializer.SessionFactory = config.BuildSessionFactory();
        }


        public static void OpenSession() 
        {

        }

        public static void CloseSession() 
        {

        }

    }
}