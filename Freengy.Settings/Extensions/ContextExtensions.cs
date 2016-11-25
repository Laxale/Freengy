// Created by Laxale 25.11.2016
//
//


namespace Freengy.Settings.Extensions 
{
    using System;
    using System.Data.Entity;

    using Freengy.Settings.Interfaces;

    using Catel.IoC;


    internal static class ContextExtensions 
    {
        private static readonly ITypeFactory TypeFactory = ServiceLocator.Default.ResolveType<ITypeFactory>();


        public static void PopulateWithRegisteredTypes(this DbContext context) 
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var facade = ServiceLocator.Default.ResolveType<ISettingsFacade>();
            var entityTypesToRegister = facade.GetRegisteredEntityTypes();

            foreach (Type type in entityTypesToRegister)
            {
                var newRow = ContextExtensions.TypeFactory.CreateInstance(type);

                //add and remove row to create empty table
                context.Set(type).Add(newRow);
                //context.Set(type).Remove(newRow);
            }

            context.SaveChanges();
        }
    }
}
