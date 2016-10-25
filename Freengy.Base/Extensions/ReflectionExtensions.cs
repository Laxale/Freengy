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

            Type implementingType = assembly.DefinedTypes.FirstOrDefault(definedType => definedType.Implements(targetInterfaceType));

            if (implementingType == null)
            {
                var message = $"Assembly '{ assembly.FullName }' doesnt implement '{ targetInterfaceType.FullName }'";

                throw new Exception(message);
            }

            return implementingType;
        }

        public static Type FindImplementingType<TInterfaceType>(this Assembly assembly) where TInterfaceType : class 
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

            Type interfaceType = typeof (TInterfaceType);
            Type implementingType = assembly.DefinedTypes.FirstOrDefault(definedType => definedType.Implements(interfaceType));

            if (implementingType == null)
            {
                var message = $"Assembly '{assembly.FullName }' doesnt implement '{ interfaceType.FullName }'";

                throw new Exception(message);
            }

            return implementingType;
        }

        /// <summary>
        /// Checks if this type implements target interface type
        /// </summary>
        /// <typeparam name="TInterfaceType">Interface type to check </typeparam>
        /// <param name="type"></param>
        /// <returns>True or false</returns>
        public static bool Implements<TInterfaceType>(this Type type) 
        {
            var implementedInterfaces = type.GetInterfaces();

            bool isImplementingType = implementedInterfaces.Contains(typeof(TInterfaceType));

            return isImplementingType;
        }

        public static bool Implements(this Type type, Type interfaceType) 
        {
            var implementedInterfaces = type.GetInterfaces();

            bool isImplementingType = implementedInterfaces.Contains(interfaceType);

            return isImplementingType;
        }
    }
}