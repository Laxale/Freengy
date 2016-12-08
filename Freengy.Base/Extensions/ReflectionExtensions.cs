// Created by Laxale 25.10.2016
//
//


namespace Freengy.Base.Extensions
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Collections.Generic;

    using Freengy.Base.Helpers;
    

    public static class ReflectionExtensions
    {
        public static Type FindInheritingType<TBaseType>(this Assembly assembly) where TBaseType : class 
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

            Type baseType = typeof(TBaseType);

            Type inheritingType = assembly.ExportedTypes.FirstOrDefault(definedType => definedType.IsSubclassOf(baseType));
            
            ThrowNotImplementsIfNull(assembly, inheritingType, baseType);

            return inheritingType;
        }
        public static Type TryFindInheritingType<TBaseType>(this Assembly assembly) where TBaseType : class 
        {
            try
            {
                return FindInheritingType<TBaseType>(assembly);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IEnumerable<Type> FindInheritingTypes<TBaseType>(this Assembly assembly) where TBaseType : class 
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

            Type baseType = typeof(TBaseType);

            IEnumerable<Type> implementingTypes =
                assembly
                .DefinedTypes
                .Where(definedType => definedType.IsSubclassOf(baseType))
                .ToArray();

            ThrowNotImplementsIfNull(assembly, implementingTypes.FirstOrDefault(), baseType);

            return implementingTypes;
        }


        public static Type FindImplementingType<TInterfaceType>(this Assembly assembly) where TInterfaceType : class 
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

            Type interfaceType = typeof(TInterfaceType);
            Type implementingType =
                assembly.DefinedTypes.FirstOrDefault(definedType => definedType.ImplementsInterface(interfaceType));

            ThrowNotImplementsIfNull(assembly, implementingType, interfaceType);

            return implementingType;
        }

        /// <summary>
        /// Get first type from assembly that implements target interface type
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="targetInterfaceType"></param>
        /// <returns>Interface implementer type</returns>
        public static Type FindImplementingType(this Assembly assembly, Type targetInterfaceType) 
        {
            Common.ThrowIfArgumentsHasNull(assembly, targetInterfaceType);

            Type implementingType =
                assembly
                    .DefinedTypes
                    .FirstOrDefault(definedType => definedType.ImplementsInterface(targetInterfaceType));

            ThrowNotImplementsIfNull(assembly, implementingType, targetInterfaceType);

            return implementingType;
        }

        /// <summary>
        /// Tries to find <see cref="TInterfaceType"/> implementing type in assembly. Doesnt throw if any errors
        /// </summary>
        /// <typeparam name="TInterfaceType"></typeparam>
        /// <param name="assembly"></param>
        /// <returns><see cref="TInterfaceType"/> implementing type or null if any errors</returns>
        public static Type TryFindImplementingType<TInterfaceType>(this Assembly assembly) where TInterfaceType : class 
        {
            try
            {
                return FindImplementingType<TInterfaceType>(assembly);
            }
            catch (Exception)
            {
                return null;
            }
        }


        /// <summary>
        /// Checks if this type implements target interface type
        /// </summary>
        /// <typeparam name="TInterfaceType">Interface type to check </typeparam>
        /// <param name="type"></param>
        /// <returns>True or false</returns>
        public static bool ImplementsInterface<TInterfaceType>(this Type type) 
        {
            var implementedInterfaces = type.GetInterfaces();

            bool isImplementingType = implementedInterfaces.Contains(typeof(TInterfaceType));

            return isImplementingType;
        }
        public static bool ImplementsInterface(this Type type, Type interfaceType)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var implementedInterfaces = type.GetInterfaces();
            
            bool isImplementingType = implementedInterfaces.Contains(interfaceType);

            return isImplementingType;
        }

        
        private static void ThrowNotImplementsIfNull(Assembly assembly, Type type, Type targetType) 
        {
            if (type != null) return;

            var message = $"Assembly '{ assembly.GetName().Name }' doesnt implement or inherit '{ targetType.FullName }'";

            throw new NotImplementedException(message);
        }
    }
}