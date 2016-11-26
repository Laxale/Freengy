// Created by Laxale 26.11.2016
//
//


namespace Freengy.Settings.Configuration 
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Freengy.Base.Interfaces;
    using Freengy.Base.DefaultImpl;
    using Freengy.Settings.ModuleSettings;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;
    

    /// <summary>
    /// Maps poco class <see cref="T"/> properties to a database
    /// </summary>
    /// <typeparam name="T">A <see cref="SettingsUnitBase"/> child poco</typeparam>
    internal class GenericMapping<T> : ClassMapping<T>  where T : SettingsUnitBase, new() 
    {
        public GenericMapping() 
        {
            base.Table(nameof(T));

            this.MapKey();
            this.MapProperties();
        }


        private void MapKey() 
        {
            base.Id(mappedObject => mappedObject.Id, idMapper => idMapper.Generator(Generators.Identity));
        }

        private void MapProperties() 
        {
            foreach (KeyValuePair<string, ICollection<Attribute>> propertyPair in new T().ColumnsProperties)
            {
                Action<IPropertyMapper> mapperAction =
                    mapper =>
                    {
                        bool handled = false;

                        foreach (Attribute attribute in propertyPair.Value)
                        {
                            handled |= new AttributeHandler(mapper).Handle(attribute);

                            if (handled) break;
                        }

                        if (handled)
                        {
                            mapper.Column(propertyPair.Key);
                        }
                    };

                base.Property(propertyPair.Key, mapperAction);
            }
        }
    }

    /// <summary>
    /// Just for some comfort. Contains <see cref="IResponsibilityChainer{TObjectType}"/> for handling property attributes
    /// </summary>
    internal class AttributeHandler 
    {
        private readonly IPropertyMapper mapper;
        private readonly IResponsibilityChainer<Attribute> attributeChainer = new ResponsibilityChainer<Attribute>();


        public AttributeHandler(IPropertyMapper mapper) 
        {
            this.mapper = mapper;

            this.SetupChainer();
        }


        public bool Handle(Attribute attribute) 
        {
            return this.attributeChainer.Handle(attribute);
        }


        private void SetupChainer() 
        {
            this.attributeChainer.AddHandler(this.HanldeRequiredAttribute);
            this.attributeChainer.AddHandler(this.HanldeStringLengthAttribute);
        }

        private bool HanldeRequiredAttribute(Attribute attribute) 
        {
            var requiredAttribute = attribute as RequiredAttribute;

            if (requiredAttribute == null) return false;

            this.mapper.NotNullable(true);

            return true;
        }
        private bool HanldeStringLengthAttribute(Attribute attribute)
        {
            var stringLengthAttribute = attribute as StringLengthAttribute;

            if (stringLengthAttribute == null) return false;

            // there is no way to check minimum length in nhibernate without
            // attributes on poco class. But I want to avoid using nhibernate in over assemblies
            this.mapper.Length(stringLengthAttribute.MaximumLength);

            return true;
        }
    }
}