// Created by Laxale 27.11.2016 
//
//


namespace Freengy.Settings.Migrations
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Collections.Generic;

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

                // add column with a name of a property
                var columnSyntax = createColumnSyntax.WithColumn(unitColumnsProperty.Key);

                var attributeHandler = new MigrationAttributeHandler(columnType, columnSyntax);
                
                // add column's properties - nullability, length, etc
                foreach (Attribute attribute in unitColumnsProperty.Value)
                {
                    attributeHandler.Handle(attribute);
                }
            }
        }
    }
}