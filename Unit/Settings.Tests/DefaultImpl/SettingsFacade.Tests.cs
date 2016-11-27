// Created by Laxale 23.11.2016
//
//


namespace Settings.Tests.DefaultImpl
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Freengy.Settings.Interfaces;
    using Freengy.Settings.DefaultImpl;
    using Freengy.Settings.ModuleSettings;

    using NUnit.Framework;

    using Catel.IoC;


    internal class TestSettingsUnit : SettingsUnitBase 
    {
        public override IDictionary<string, ICollection<Attribute>> ColumnsProperties { get; } =
            new Dictionary<string, ICollection<Attribute>>
            {
                { nameof(TestSettingsUnit.Name), new [] { new RequiredAttribute() } }
            };

        public override string Name { get; set; } = Guid.NewGuid().ToString().Substring(0, 6);
    }


    [TestFixture]
    public class SettingsFacadeTests 
    {
        [Test(Description = "Try to get not registered unit - receive null")]
        public void GetUnit_GenericNotRegistered() 
        {
            var facade = SettingsFacade.Instance;

            var result = facade.GetUnit<TestSettingsUnit>();

            Assert.IsNull(result);
        }
    }
}