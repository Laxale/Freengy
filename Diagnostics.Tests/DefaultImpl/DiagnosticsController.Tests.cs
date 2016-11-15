// Created by Laxale 15.11.2016
//
//


namespace Diagnostics.Tests.DefaultImpl 
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using Freengy.Diagnostics.Interfaces;
    using Freengy.Diagnostics.DefaultImpl;

    using Moq;

    using NUnit.Framework;


    [TestFixture]
    public class DiagnosticsControllerTests 
    {
        private readonly IDiagnosticsController diagnosticsController = DiagnosticsController.Instance;


        [Test]
        public void RegisterUnit_NullCase() 
        {
            Action registerNullAction =
                () =>
                {
                    this.diagnosticsController.RegisterUnit(null);
                };

            Assert.Throws<ArgumentNullException>(() => registerNullAction());
        }

        [TestCase(null, Description = "Register null-named unit. Get exception")]
        [TestCase("",   Description = "Register empty-named unit. Get exception")]
        public void RegisterUnit_BadNameCase(string badName) 
        {
            var badNamedUnit = new Mock<IDiagnosticsUnit>();
            badNamedUnit.Setup(unit => unit.Name).Returns(badName);

            Action registerNullAction =
                () =>
                {
                    this.diagnosticsController.RegisterUnit(badNamedUnit.Object);
                };

            Assert.Throws<ArgumentNullException>(() => registerNullAction());
        }

        [Test]
        public void RegisterUnit_NullTestCasesCase() 
        {
            var badCasesUnit = new Mock<IDiagnosticsUnit>();
            badCasesUnit.Setup(unit => unit.Name).Returns("Good name");
            badCasesUnit.Setup(unit => unit.TestCases).Returns((IEnumerable<Func<bool>>)null);

            Action registerNullAction =
                () =>
                {
                    this.diagnosticsController.RegisterUnit(badCasesUnit.Object);
                };

            Assert.Throws<ArgumentNullException>(() => registerNullAction());
        }

        [Test]
        public void RegisterUnit_EmptyTestCasesCase() 
        {
            var badCasesUnit = new Mock<IDiagnosticsUnit>();
            badCasesUnit.Setup(unit => unit.Name).Returns("Good name");
            badCasesUnit.Setup(unit => unit.TestCases).Returns(Enumerable.Empty<Func<bool>>);

            Action registerNullAction =
                () =>
                {
                    this.diagnosticsController.RegisterUnit(badCasesUnit.Object);
                };

            Assert.Throws<InvalidOperationException>(() => registerNullAction());
        }

        [TestCase(true,  Description = "Register normal unit. Okay")]
        [TestCase(false, Description = "Register throwing unit. Okay")]
        public void RegisterUnit_RegisterNormalCase(bool throwing) 
        {
            IDiagnosticsUnit goodUnit = DiagnosticsControllerTests.GetGoodUnit();

            this.diagnosticsController.RegisterUnit(goodUnit);

            Assert.IsTrue(this.diagnosticsController.IsUnitRegistered(goodUnit));
        }

        [TestCase(null, Description = "Pass null, get exception")]
        [TestCase("",   Description = "Pass empty, get exception")]
        public void IsUnitRegistered_BadStringCase(string badName) 
        {
            Action testAction = () => this.diagnosticsController.IsUnitRegistered(badName);

            Assert.Throws<ArgumentNullException>(() => testAction());
        }

        [Test]
        public void IsUnitRegistered_GoodStringNotRegisteredCase() 
        {
            bool isRegistered = this.diagnosticsController.IsUnitRegistered(Guid.NewGuid().ToString());

            Assert.IsFalse(isRegistered);
        }

        [Test]
        public void IsUnitRegistered_GoodStringIsRegisteredCase() 
        {
            IDiagnosticsUnit goodUnit = DiagnosticsControllerTests.GetGoodUnit();

            this.diagnosticsController.RegisterUnit(goodUnit);

            bool isRegistered = this.diagnosticsController.IsUnitRegistered(goodUnit.Name);

            Assert.IsTrue(isRegistered);
        }

        [Test]
        public void IsUnitRegistered_NullUnitCase() 
        {
            Action testAction = () => this.diagnosticsController.IsUnitRegistered((IDiagnosticsUnit)null);

            Assert.Throws<ArgumentNullException>(() => testAction());
        }

        [TestCase(null, Description = "Pass in null unit name - get exception")]
        [TestCase("",   Description = "Pass in empty unit name - get exception")]
        public void IsUnitRegistered_BadUnitNameCase(string badName) 
        {
            var mockUnit = new Mock<IDiagnosticsUnit>();
            mockUnit.Setup(unit => unit.Name).Returns(badName);

            Action testAction = () => this.diagnosticsController.IsUnitRegistered(mockUnit.Object);

            Assert.Throws<ArgumentNullException>(() => testAction());
        }

        [Test]
        public void IsUnitRegistered_GoodUnitIsRegisteredCase() 
        {
            IDiagnosticsUnit goodUnit = DiagnosticsControllerTests.GetGoodUnit();

            this.diagnosticsController.RegisterUnit(goodUnit);

            bool isRegistered = this.diagnosticsController.IsUnitRegistered(goodUnit);

            Assert.IsTrue(isRegistered);
        }

        [TestCase(null, Description = "Pass in null unit name - get exception")]
        [TestCase("",   Description = "Pass in empty unit name - get exception")]
        public void UnregisterUnit_BadStringCase(string badName) 
        {
            Action testAction = () => this.diagnosticsController.UnregisterUnit(badName);

            Assert.Throws<ArgumentNullException>(() => testAction());
        }

        [Test(Description = "Unregister random name - get nothing. Okay")]
        public void UnregisterUnit_GoodStringNotRegisteredCase() 
        {
            string randomName = Guid.NewGuid().ToString();

            this.diagnosticsController.UnregisterUnit(randomName);
            bool isRegistered = this.diagnosticsController.IsUnitRegistered(randomName);

            Assert.IsFalse(isRegistered);
        }

        [Test(Description = "Unregister registered name - get nothing. Okay")]
        public void UnregisterUnit_GoodStringIsRegisteredCase() 
        {
            IDiagnosticsUnit goodUnit = DiagnosticsControllerTests.GetGoodUnit();

            this.diagnosticsController.RegisterUnit(goodUnit);

            bool isRegistered = this.diagnosticsController.IsUnitRegistered(goodUnit.Name);

            this.diagnosticsController.UnregisterUnit(goodUnit.Name);

            bool isUnregistered = !this.diagnosticsController.IsUnitRegistered(goodUnit.Name);

            Assert.AreEqual(isRegistered, isUnregistered);
        }

        [Test]
        public void UnregisterUnit_NullUnitCase() 
        {
            Action testAction = () => this.diagnosticsController.UnregisterUnit((IDiagnosticsUnit)null);

            Assert.Throws<ArgumentNullException>(() => testAction());
        }

        [TestCase(null, Description = "Pass in null unit name - get exception")]
        [TestCase("",   Description = "Pass in empty unit name - get exception")]
        public void UnregisterUnit_BadUnitNameCase(string badName) 
        {
            var mockUnit = new Mock<IDiagnosticsUnit>();
            mockUnit.Setup(unit => unit.Name).Returns(badName);

            Action testAction = () => this.diagnosticsController.UnregisterUnit(mockUnit.Object);

            Assert.Throws<ArgumentNullException>(() => testAction());
        }

        [Test(Description = "Unregister not registered unit - get nothing. Okay")]
        public void UnregisterUnit_UnitNotRegisteredCase() 
        {
            IDiagnosticsUnit goodUnit = DiagnosticsControllerTests.GetGoodUnit();

            this.diagnosticsController.UnregisterUnit(goodUnit);

            bool isRegistered = this.diagnosticsController.IsUnitRegistered(goodUnit);

            Assert.IsFalse(isRegistered);
        }

        [Test(Description = "Unregister registered unit - get nothing. Okay")]
        public void UnregisterUnit_UnitIsRegisteredCase() 
        {
            IDiagnosticsUnit goodUnit = DiagnosticsControllerTests.GetGoodUnit();

            this.diagnosticsController.RegisterUnit(goodUnit);

            bool isRegistered = this.diagnosticsController.IsUnitRegistered(goodUnit);

            this.diagnosticsController.UnregisterUnit(goodUnit);

            bool isUnregistered = !this.diagnosticsController.IsUnitRegistered(goodUnit);

            Assert.AreEqual(isRegistered, isUnregistered);
        }


        private static IDiagnosticsUnit GetGoodUnit() 
        {
            string name = "GoodName";
            IEnumerable<Func<bool>> testCases = new List<Func<bool>>
            {
                () => true,
                () => false
            };

            var mockUnit = new Mock<IDiagnosticsUnit>();
            mockUnit.Setup(unit => unit.Name).Returns(name);
            mockUnit.Setup(unit => unit.TestCases).Returns(testCases);

            return mockUnit.Object;
        }
    }
}