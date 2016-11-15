// Created by Laxale 15.11.2016
//
//


namespace Diagnostics.Tests.DefaultImpl 
{
    using System;

    using Freengy.Diagnostics.Interfaces;
    using Freengy.Diagnostics.DefaultImpl;

    using NUnit.Framework;


    [TestFixture]
    public class DiagnosticsUnitFactoryTests 
    {
        [TestCase(null, Description = "Pass null name - get exception")]
        [TestCase("",   Description = "Pass empty name - get exception")]
        public void CreateInstance_TestBadName(string badName) 
        {
            IDiagnosticsUnitFactory factory = new DiagnosticsUnitFactory();

            Action testAction =
                () =>
                {
                    factory.CreateInstance(null, () => true);
                };

            Assert.Throws<ArgumentNullException>(() => testAction());
        }

        [Test]
        public void CreateInstance_TestNullAtomicTest() 
        {
            IDiagnosticsUnitFactory factory = new DiagnosticsUnitFactory();

            Action testAction =
                () =>
                {
                    factory.CreateInstance("Wow", null);
                };

            Assert.Throws<ArgumentNullException>(() => testAction());
        }

        [Test]
        public void CreateInstance_TestNormalCase() 
        {
            IDiagnosticsUnitFactory factory = new DiagnosticsUnitFactory();

            factory.CreateInstance("Wow", () => true);
        }

        [Test(Description = "Throwing atomic test does nothing bad here")]
        public void CreateInstance_TestThrowingAtomicCase() 
        {
            IDiagnosticsUnitFactory factory = new DiagnosticsUnitFactory();

            factory.CreateInstance("Wow", () => { throw new ArgumentException(); });
        }
    }
}