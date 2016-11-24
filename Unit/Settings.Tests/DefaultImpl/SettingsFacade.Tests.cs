// Created by Laxale 23.11.2016
//
//


namespace Settings.Tests.DefaultImpl
{
    using System;

    using Freengy.Settings.Interfaces;
    using Freengy.Settings.DefaultImpl;

    using NUnit.Framework;

    using Catel.IoC;


    internal class TestSettingsUnit 
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }


    [TestFixture]
    public class SettingsFacadeTests 
    {
        [SetUp]
        public void Setup()
        {
            SettingsFacade.Instance.RegisterEntityType(typeof(TestSettingsUnit));
            ServiceLocator.Default.RegisterInstance(SettingsFacade.Instance);
        }

        [Test(Description = "Try to get not registered unit - receive null")]
        public void GetUnit_GenericNotRegistered()
        {
            var facade = SettingsFacade.Instance;

            var result = facade.GetUnit<TestSettingsUnit>();

            Assert.IsNull(result);
        }
    }
}