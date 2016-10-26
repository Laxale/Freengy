// Created by Laxale 25.10.2016
//
//


namespace Freengy.Base.Extensions 
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Freengy.Base.Helpers;


    public static class ReflectionExtensions 
    {
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

        public static Type FindImplementingType<TInterfaceType>(this Assembly assembly) where TInterfaceType : class 
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

            Type interfaceType = typeof (TInterfaceType);
            Type implementingType = assembly.DefinedTypes.FirstOrDefault(definedType => definedType.ImplementsInterface(interfaceType));

            ThrowNotImplementsIfNull(assembly, implementingType, interfaceType);

            return implementingType;
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