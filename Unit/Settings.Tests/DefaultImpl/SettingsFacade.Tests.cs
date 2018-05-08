// Created by Laxale 23.11.2016
//
//

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Freengy.Settings.Interfaces;
using Freengy.Settings.DefaultImpl;
using Freengy.Settings.ModuleSettings;

using NUnit.Framework;


namespace Settings.Tests.DefaultImpl 
{
    internal class TestSettingsUnit : SettingsUnit 
    {
        public override string Name { get; } = Guid.NewGuid().ToString().Substring(0, 6);
    }


    [TestFixture]
    public class SettingsFacadeTests 
    {
        [Test(Description = "Try to get not registered unit - receive null")]
        public void GetUnit_GenericNotRegistered() 
        {
            var facade = SettingsRepository.Instance;

            var result = facade.GetUnit<TestSettingsUnit>();

            Assert.IsNull(result);
        }
    }
}