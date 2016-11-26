// Created by Laxale 23.11.2016
//
//


namespace Settings.Tests.DefaultImpl
{
    using System;
    using System.Collections.Generic;

    using Freengy.Settings.Interfaces;
    using Freengy.Settings.DefaultImpl;
    using Freengy.Settings.ModuleSettings;

    using NUnit.Framework;

    using Catel.IoC;


    internal class TestSettingsUnit : SettingsUnitBase 
    {
        public long Id { get; set; }

        public override IDictionary<string, ICollection<Attribute>> ColumnsProperties { get; }

        public string Name { get; set; } = Guid.NewGuid().ToString().Substring(0, 6);
    }


    [TestFixture]
    public class SettingsFacadeTests 
    {
        [SetUp]
        public void Setup()
        {
            ServiceLocator.Default.RegisterInstance(SettingsFacade.Instance);
        }

        [Test(Description = "Try to get not registered unit - receive null")]
        public void GetUnit_GenericNotRegistered()
        {
            var facade = SettingsFacade.Instance;

            var result = facade.GetUnit<TestSettingsUnit>();

            Assert.IsNull(result);
        }

        [Test(Description = "Try to get not registered unit - receive null")]
        public void GetUnit_GenericRegistered() 
        {
            SettingsFacade.FullInstance.RegisterUnitType<TestSettingsUnit>();

            var facade = SettingsFacade.Instance;

            var result = facade.GetUnit<TestSettingsUnit>();

            Assert.IsNotNull(result);
        }
    }
}