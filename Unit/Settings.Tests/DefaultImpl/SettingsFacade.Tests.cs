// Created by Laxale 23.11.2016
//
//


namespace Settings.Tests.DefaultImpl
{
    using System;

    using Freengy.Settings.Interfaces;
    using Freengy.Settings.DefaultImpl;

    using NUnit.Framework;

    using Moq;


    internal class TestSettingsUnit 
    {
        public long Id { get; set; }
    }

    [TestFixture]
    public class SettingsFacadeTests 
    {
        [Test]
        public void RegisterUnit_PassNullCase() 
        {
            var facade = SettingsFacade.Instance;

            Action testAction =
                () =>
                {
                    facade.RegisterUnit(null);
                };

            Assert.Throws<ArgumentNullException>(() => testAction());
        }

        [Test(Description = "Register some test unit and check if facade returns self")]
        public void RegisterUnit_CheckReturningSelf() 
        {
            var facade = SettingsFacade.Instance;

            var mockUnitOne = new Mock<IBaseSettingsUnit>();
            var mockUnitTwo = new Mock<IBaseSettingsUnit>();

            mockUnitOne.Setup(mock => mock.Name).Returns("good unit one");
            mockUnitTwo.Setup(mock => mock.Name).Returns("good unit two");

            ISettingsFacade returnedFacase = 
                facade
                .RegisterUnit(mockUnitOne.Object)
                .RegisterUnit(mockUnitTwo.Object);

            Assert.AreEqual(facade, returnedFacase);
        }

        [Test(Description = "Try to get not registered unit - get exception")]
        public void GetUnit_GenericNotRegistered() 
        {
            var facade = SettingsFacade.Instance;

            Action testAction =
                () =>
                {
                    facade.GetUnit<TestSettingsUnit>();
                };

            Assert.Throws<InvalidOperationException>(() => testAction());
        }
    }
}