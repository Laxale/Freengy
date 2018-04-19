// Created by Laxale 17.04.2018
//
//

using System;


namespace Freengy.Common.Database 
{
    /// <summary>
    /// Атрибут, объявляющий ORM-контекст для типа объекта, хранимого в базе.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RelationalContextAttribute : Attribute 
    {
        public RelationalContextAttribute(Type contextType) 
        {
            ContextType = contextType ?? throw new ArgumentNullException(nameof(contextType));
        }


        /// <summary>
        /// Тип ORM-контекста для класса, помеченного атрибутом <see cref="RelationalContextAttribute"/>.
        /// </summary>
        public Type ContextType { get; set; }
    }
}