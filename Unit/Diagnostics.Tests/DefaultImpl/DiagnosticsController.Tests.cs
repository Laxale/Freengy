﻿// Created by Laxale 15.11.2016
//
//

using System;
using System.Linq;
using System.Collections.Generic;

using Freengy.Base.Interfaces;
using Freengy.Base.DefaultImpl;
using Freengy.Diagnostics.Interfaces;
using Freengy.Diagnostics.DefaultImpl;

using Moq;

using NUnit.Framework;


namespace Diagnostics.Tests.DefaultImpl 
{
    [TestFixture]
    public class DiagnosticsControllerTests 
    {
        private IDiagnosticsController diagnosticsController;

        
        [SetUp]
        public void Setup()
        {
            Action emptyAction = () => { };
            var fakeDispatcher = new Mock<IGuiDispatcher>();
            fakeDispatcher.Setup(dispatcher => dispatcher.InvokeOnGuiThread(emptyAction));

           MyServiceLocator.Instance.RegisterInstance(fakeDispatcher.Object);

            this.diagnosticsController = DiagnosticsController.Instance;
        }

        [Test]
        public void RegisterUnit_NullCase() 
        {
            Action registerNullAction =
                () =>
                {
                    this.diagnosticsController.RegisterCategory(null);
                };

            Assert.Throws<ArgumentNullException>(() => registerNullAction());
        }

        [TestCase(null, Description = "Register null-named unit. Get exception")]
        [TestCase("",   Description = "Register empty-named unit. Get exception")]
        public void RegisterUnit_BadNameCase(string badName) 
        {
            var badNamedUnit = new Mock<IDiagnosticsCategory>();
            badNamedUnit.Setup(unit => unit.Name).Returns(badName);

            Action registerNullAction =
                () =>
                {
                    this.diagnosticsController.RegisterCategory(badNamedUnit.Object);
                };

            Assert.Throws<ArgumentNullException>(() => registerNullAction());
        }

        [Test]
        public void RegisterUnit_NullTestCasesCase() 
        {
            var badCasesUnit = new Mock<IDiagnosticsCategory>();
            badCasesUnit.Setup(unit => unit.Name).Returns("Good name");
            badCasesUnit.Setup(unit => unit.TestUnits).Returns((IEnumerable<IDiagnosticsUnit>)null);

            Action registerNullAction =
                () =>
                {
                    this.diagnosticsController.RegisterCategory(badCasesUnit.Object);
                };

            Assert.Throws<ArgumentNullException>(() => registerNullAction());
        }

        [Test]
        public void RegisterUnit_EmptyTestCasesCase() 
        {
            var badCasesUnit = new Mock<IDiagnosticsCategory>();
            badCasesUnit.Setup(unit => unit.Name).Returns("Good name");
            badCasesUnit.Setup(unit => unit.TestUnits).Returns(Enumerable.Empty<IDiagnosticsUnit>);

            Action registerNullAction =
                () =>
                {
                    this.diagnosticsController.RegisterCategory(badCasesUnit.Object);
                };

            Assert.Throws<InvalidOperationException>(() => registerNullAction());
        }

        [TestCase(true,  Description = "Register normal unit. Okay")]
        [TestCase(false, Description = "Register throwing unit. Okay")]
        public void RegisterUnit_RegisterNormalCase(bool throwing) 
        {
            IDiagnosticsCategory goodCategory = DiagnosticsControllerTests.GetGoodCategory("wow not bad");

            this.diagnosticsController.RegisterCategory(goodCategory);

            Assert.IsTrue(this.diagnosticsController.IsCategoryRegistered(goodCategory));
        }

        [TestCase(null, Description = "Pass null, get exception")]
        [TestCase("",   Description = "Pass empty, get exception")]
        public void IsUnitRegistered_BadStringCase(string badName) 
        {
            Action testAction = () => this.diagnosticsController.IsCategoryRegistered(badName);

            Assert.Throws<ArgumentNullException>(() => testAction());
        }

        [Test]
        public void IsUnitRegistered_GoodStringNotRegisteredCase() 
        {
            bool isRegistered = this.diagnosticsController.IsCategoryRegistered(Guid.NewGuid().ToString());

            Assert.IsFalse(isRegistered);
        }

        [Test]
        public void IsUnitRegistered_GoodStringIsRegisteredCase() 
        {
            IDiagnosticsCategory goodCategory = DiagnosticsControllerTests.GetGoodCategory("good");

            this.diagnosticsController.RegisterCategory(goodCategory);

            bool isRegistered = this.diagnosticsController.IsCategoryRegistered(goodCategory.Name);

            Assert.IsTrue(isRegistered);
        }

        [Test]
        public void IsUnitRegistered_NullUnitCase() 
        {
            Action testAction = () => this.diagnosticsController.IsCategoryRegistered((IDiagnosticsCategory)null);

            Assert.Throws<ArgumentNullException>(() => testAction());
        }

        [TestCase(null, Description = "Pass in null unit name - get exception")]
        [TestCase("",   Description = "Pass in empty unit name - get exception")]
        public void IsUnitRegistered_BadUnitNameCase(string badName) 
        {
            var mockUnit = new Mock<IDiagnosticsCategory>();
            mockUnit.Setup(unit => unit.Name).Returns(badName);

            Action testAction = () => this.diagnosticsController.IsCategoryRegistered(mockUnit.Object);

            Assert.Throws<ArgumentNullException>(() => testAction());
        }

        [Test]
        public void IsUnitRegistered_GoodUnitIsRegisteredCase() 
        {
            IDiagnosticsCategory goodCategory = DiagnosticsControllerTests.GetGoodCategory("yep");

            this.diagnosticsController.RegisterCategory(goodCategory);

            bool isRegistered = this.diagnosticsController.IsCategoryRegistered(goodCategory);

            Assert.IsTrue(isRegistered);
        }

        [TestCase(null, Description = "Pass in null unit name - get exception")]
        [TestCase("",   Description = "Pass in empty unit name - get exception")]
        public void UnregisterUnit_BadStringCase(string badName) 
        {
            Action testAction = () => this.diagnosticsController.UnregisterCategory(badName);

            Assert.Throws<ArgumentNullException>(() => testAction());
        }

        [Test(Description = "Unregister random name - get nothing. Okay")]
        public void UnregisterUnit_GoodStringNotRegisteredCase() 
        {
            string randomName = Guid.NewGuid().ToString();

            this.diagnosticsController.UnregisterCategory(randomName);
            bool isRegistered = this.diagnosticsController.IsCategoryRegistered(randomName);

            Assert.IsFalse(isRegistered);
        }

        [Test(Description = "Unregister registered name - get nothing. Okay")]
        public void UnregisterUnit_GoodStringIsRegisteredCase() 
        {
            IDiagnosticsCategory goodCategory = DiagnosticsControllerTests.GetGoodCategory("hehe");

            this.diagnosticsController.RegisterCategory(goodCategory);

            bool isRegistered = this.diagnosticsController.IsCategoryRegistered(goodCategory.Name);

            this.diagnosticsController.UnregisterCategory(goodCategory.Name);

            bool isUnregistered = !this.diagnosticsController.IsCategoryRegistered(goodCategory.Name);

            Assert.AreEqual(isRegistered, isUnregistered);
        }

        [Test]
        public void UnregisterUnit_NullUnitCase() 
        {
            Action testAction = () => this.diagnosticsController.UnregisterCategory((IDiagnosticsCategory)null);

            Assert.Throws<ArgumentNullException>(() => testAction());
        }

        [TestCase(null, Description = "Pass in null unit name - get exception")]
        [TestCase("",   Description = "Pass in empty unit name - get exception")]
        public void UnregisterUnit_BadUnitNameCase(string badName) 
        {
            var mockUnit = new Mock<IDiagnosticsCategory>();
            mockUnit.Setup(unit => unit.Name).Returns(badName);

            Action testAction = () => this.diagnosticsController.UnregisterCategory(mockUnit.Object);

            Assert.Throws<ArgumentNullException>(() => testAction());
        }

        [Test(Description = "Unregister not registered unit - get nothing. Okay")]
        public void UnregisterUnit_UnitNotRegisteredCase() 
        {
            IDiagnosticsCategory goodCategory = DiagnosticsControllerTests.GetGoodCategory("such");

            this.diagnosticsController.UnregisterCategory(goodCategory);

            bool isRegistered = this.diagnosticsController.IsCategoryRegistered(goodCategory);

            Assert.IsFalse(isRegistered);
        }

        [Test(Description = "Unregister registered unit - get nothing. Okay")]
        public void UnregisterUnit_UnitIsRegisteredCase() 
        {
            IDiagnosticsCategory goodCategory = DiagnosticsControllerTests.GetGoodCategory("much category");

            this.diagnosticsController.RegisterCategory(goodCategory);

            bool isRegistered = this.diagnosticsController.IsCategoryRegistered(goodCategory);

            this.diagnosticsController.UnregisterCategory(goodCategory);

            bool isUnregistered = !this.diagnosticsController.IsCategoryRegistered(goodCategory);

            Assert.AreEqual(isRegistered, isUnregistered);
        }

        [Test]
        public void GetAllCategories_Test() 
        {
            IDiagnosticsCategory firstCategory = DiagnosticsControllerTests.GetGoodCategory("nice category one");
            IDiagnosticsCategory secondCategory = DiagnosticsControllerTests.GetGoodCategory("nice category two");

            this.diagnosticsController.RegisterCategory(firstCategory);
            this.diagnosticsController.RegisterCategory(secondCategory);

            IEnumerable<IDiagnosticsCategory> gotCategories = this.diagnosticsController.GetAllCategories().ToArray();

            bool isTestPassed =
                gotCategories.Count() == 2 &&
                gotCategories.Contains(firstCategory) &&
                gotCategories.Contains(secondCategory);

            Assert.IsTrue(isTestPassed);
        }


        private static IDiagnosticsCategory GetGoodCategory(string categoryName) 
        {
            string unitName = "Unit UnitName";
            
            var mockUnit = new Mock<IDiagnosticsUnit>();
            mockUnit.Setup(unit => unit.Name).Returns(unitName);
            mockUnit.Setup(unit => unit.UnitTest).Returns(() => true);

            IEnumerable<IDiagnosticsUnit> testUnits = new List<IDiagnosticsUnit>
            {
                mockUnit.Object,
                mockUnit.Object
            };

            var mockCategory = new Mock<IDiagnosticsCategory>();
            mockCategory.Setup(category => category.Name).Returns(categoryName);
            mockCategory.Setup(category => category.TestUnits).Returns(testUnits);

            return mockCategory.Object;
        }
    }
}