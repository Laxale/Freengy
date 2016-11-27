// Created by Laxale 27.10.2016
//
//


namespace Base.Tests.Extensions 
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using Freengy.Base.Extensions;

    using NUnit.Framework;


    internal class TestBaseClass 
    {

    }

    internal class TestInternalClass : TestBaseClass 
    {
        
    }

    internal class TestPublicClass : TestBaseClass 
    {

    }


    [TestFixture]
    public class ReflectionExtensionsTests 
    {
        [Test(Description = "Try to find a non-existing type in test assembly. Get exception")]
        public void FindInheritingType_LookForNotExistingType() 
        {
            Action testAction =
                () =>
                {
                    var currentAssembly = this.GetType().Assembly;
                    var inheritingType = currentAssembly.FindInheritingType<TestInternalClass>();
                };

            Assert.Throws<NotImplementedException>(() => testAction());
        }

        [Test(Description = "Try to find an existing inheriting type in test assembly. Get that type")]
        public void FindInheritingType_LookExistingType() 
        {
            var currentAssembly = this.GetType().Assembly;
            var inheritingType = currentAssembly.FindInheritingType<TestBaseClass>();

            Assert.AreEqual(typeof(TestInternalClass), inheritingType);
        }

        [Test(Description = "Try to find an existing inheriting type in test assembly. Get that type")]
        public void FindInheritingTypes_LookForAllInheritingTypes() 
        {
            var currentAssembly = this.GetType().Assembly;
            var inheritingTypes = currentAssembly.FindInheritingTypes<TestBaseClass>().ToArray();

            bool testPassed =
                inheritingTypes.Length == 2 &&
                inheritingTypes.Contains(typeof(TestPublicClass)) &&
                inheritingTypes.Contains(typeof(TestInternalClass));

            Assert.IsTrue(testPassed);
        }
    }
}