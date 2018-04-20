// Created by Laxale 20.04.2018
//
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Freengy.Base.Attributes;


namespace Freengy.UI.Helpers 
{
    /// <summary>
    /// Содержит кэш маппингов типов Вью и Вьюмоделей. Нужно для избежания повторных обращений к рефлексии.
    /// </summary>
    internal class ViewMappingCache 
    {
        private readonly Dictionary<string, Type> viewToModelMappings = new Dictionary<string, Type>();


        /// <summary>
        /// Получить тип Вьюмодели, соответствующий типу Вью.
        /// По умолчанию Вью должны лежать в папке Views, Вьюмодели в папке ViewModels.
        /// </summary>
        /// <param name="viewType">Тип Вью для определения связанного с ним типа Вьюмодели.</param>
        /// <returns>Связанный тип Вьюмодели.</returns>
        public Type GetViewModelType(Type viewType) 
        {
            if (viewType == null) throw new ArgumentNullException(nameof(viewType));

            var viewTypeName = viewType.FullName;

            if (!viewToModelMappings.TryGetValue(viewTypeName, out Type viewModelType))
            {
                // дефолтная реализация поиска - заставляет хранить классы в строго именованных папках.
                // рефлексивный поиск гибче
                //                viewTypeName = viewTypeName.Replace(".Views.", ".ViewModels.");
                //                var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
                //                var suffix = viewTypeName.EndsWith("View") ? "Model" : "ViewModel";
                //                viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "{0}{1}, {2}", viewTypeName, suffix, viewAssemblyName);

                viewModelType = ReadViewModelTypeAttribute(viewType);

                viewToModelMappings.Add(viewTypeName, viewModelType);
            }

            return viewModelType;
        }


        private Type ReadViewModelTypeAttribute(Type viewType)
        {
            var viewModelAttr = viewType.GetCustomAttribute<HasViewModelAttribute>();

            if (viewModelAttr == null)
            {
                throw new InvalidOperationException($"Тип вью '{ viewType.Name }' не содержит атрибута '{ nameof(HasViewModelAttribute) }'");
            }

            return viewModelAttr.ViewModelType;
        }
    }
}
