// Created by Laxale 27.11.2016
//
//


namespace Freengy.Settings.Migrations 
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Freengy.Base.Interfaces;
    using Freengy.Base.DefaultImpl;

    using FluentMigrator.Builders.Create.Table;


    internal class MigrationAttributeHandler 
    {
        #region vars

        private readonly Type propertyType;

        private readonly string intTypeFullName = typeof(int).FullName;
        private readonly string longTypeFullName = typeof(long).FullName;
        private readonly string stringTypeFullName = typeof(string).FullName;

        private readonly ICreateTableColumnAsTypeSyntax createColumnSyntax;
        private readonly IResponsibilityChainer<Attribute> attributeChainer = new ResponsibilityChainer<Attribute>();
        
        #endregion vars


        public MigrationAttributeHandler(Type propertyType, ICreateTableColumnAsTypeSyntax createColumnSyntax)
        {
            this.propertyType = propertyType;
            this.createColumnSyntax = createColumnSyntax;

            this.SetupChainer();
        }


        public bool Handle(Attribute attribute) 
        {
            return this.attributeChainer.Handle(attribute);
        }


        private void SetupChainer() 
        {
            this.attributeChainer.AddHandler(this.HandleStringLength);
            this.attributeChainer.AddHandler(this.HandleRequiredAtribute);
        }

        private bool HandleStringLength(Attribute attribute) 
        {
            var lengthAttribute = attribute as StringLengthAttribute;

            if (lengthAttribute == null) return false;

            if (this.propertyType.FullName == this.stringTypeFullName)
            {
                this.AddColumnAsString(lengthAttribute.MaximumLength);
            }
            else
            {
                // just ignore fail attribute
                //string message = $"Must not apply StringLengthAttribute to a {this.propertyType.FullName} property type";
                //throw new InvalidOperationException(message);
            }

            return true;
        }
        private bool HandleRequiredAtribute(Attribute attribute) 
        {
            var requiredAttribute = attribute as RequiredAttribute;

            if (requiredAttribute == null) return false;

            if (this.propertyType.FullName == this.intTypeFullName)
            {
                this.AddColumnAsInt();
            }
            else if (this.propertyType.FullName == this.longTypeFullName)
            {
                this.AddColumnAsLong();
            }
            else if (this.propertyType.FullName == this.stringTypeFullName)
            {
                this.AddColumnAsString(int.MaxValue);
            }

            return true;
        }


        private void AddColumnAsInt() 
        {
            this.createColumnSyntax.AsInt32().NotNullable();
        }
        private void AddColumnAsLong() 
        {
            this.createColumnSyntax.AsInt64().NotNullable();
        }
        private void AddColumnAsString(int stringLength) 
        {
            this.createColumnSyntax.AsString(stringLength).NotNullable();
        }
    }
}