﻿// Created by Laxale 27.11.2016 
//
//


namespace Freengy.Settings.Migrations
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Freengy.Settings.ModuleSettings;

    using FluentMigrator;
    using FluentMigrator.Builders.Create.Table;

    
    /// <summary>
    /// This class is used by external FluentMigrator tool. 
    ///<para>This tool is supposed to be located in output\Tools\FluentMugrator\tools</para>
    /// </summary>
    [Migration(1, "Initial migration")]
    public class SettingsMigration : Migration 
    {
        private readonly List<SettingsUnitBase> settingUnits = new List<SettingsUnitBase>();


        public SettingsMigration() 
        {
            this.FillUnitsList();
        }


        public override void Up() 
        {
            foreach (SettingsUnitBase settingsUnit in this.settingUnits)
            {
                string unitTableName = settingsUnit.GetType().Name;

                // common part for all units
                ICreateTableColumnOptionOrWithColumnSyntax createColumnSyntax =
                    base
                        .Create
                        .Table(unitTableName)
                        .WithColumn(nameof(settingsUnit.Id)).AsInt64().Identity().PrimaryKey();

                this.ProcessUnitColumns(settingsUnit, createColumnSyntax);
            }
        }

        public override void Down() 
        {
            foreach (SettingsUnitBase settingsUnit in this.settingUnits)
            {
                base.Delete.Table(settingsUnit.Name);
            }
        }


        private void FillUnitsList() 
        {
            this.settingUnits.Add(new GameListSettingsUnit());
            this.settingUnits.Add(new FriendListSettingsUnit());
        }

        private void ProcessUnitColumns(SettingsUnitBase unit, ICreateTableColumnOptionOrWithColumnSyntax createColumnSyntax) 
        {
            Type unitType = unit.GetType();
            PropertyInfo[] propInfos = unitType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            if (unit.ColumnsProperties == null || !unit.ColumnsProperties.Any())
            {
                throw new InvalidOperationException($"{unitType.Name} has empty ColumnsProperties");
            }

            foreach (KeyValuePair<string, ICollection<Attribute>> unitColumnsProperty in unit.ColumnsProperties)
            {
                Type columnType =
                    propInfos.FirstOrDefault(propInfo => propInfo.Name == unitColumnsProperty.Key)?.PropertyType;

                if (columnType == null)
                {
                    string message =
                        $"Property name { unitColumnsProperty.Key } declared in ColumnsProperties" +
                        $"does not correspond to real property in { unitType.FullName }";

                    throw new InvalidOperationException(message);
                }

                var columnSyntax = createColumnSyntax.WithColumn(unitColumnsProperty.Key);
                var columnTypeSyntax = this.AddColumnType(columnType, unitColumnsProperty.Value, columnSyntax);
                this.AddNullabilityAttribute(unitColumnsProperty.Value, columnTypeSyntax);
            }
        }

        private ICreateTableColumnOptionOrWithColumnSyntax AddColumnType(Type columnType, ICollection<Attribute> columnAttributes,
            ICreateTableColumnAsTypeSyntax columnSyntax) 
        {
            if (columnType == typeof(int))
            {
                return columnSyntax.AsInt32();
            }

            if (columnType == typeof(long))
            {
                return columnSyntax.AsInt64();
            }

            if (columnType == typeof(string))
            {
                StringLengthAttribute strigLengthAttribute = 
                    columnAttributes
                    .FirstOrDefault(attribute => attribute is StringLengthAttribute) as StringLengthAttribute;

                if (strigLengthAttribute == null)
                {
                    return columnSyntax.AsString();
                }
                else
                {
                    return columnSyntax.AsString(strigLengthAttribute.MaximumLength);
                }
            }

            throw new NotImplementedException($"Not implemented column type { columnType }");
        }
        
        private void AddNullabilityAttribute(ICollection<Attribute> columnAttributes, ICreateTableColumnOptionOrWithColumnSyntax columnSyntax) 
        {
            if (columnAttributes.Any(attribute => attribute is RequiredAttribute))
            {
                columnSyntax.NotNullable();
            }
            else
            {
                columnSyntax.Nullable();
            }
        }
    }
}