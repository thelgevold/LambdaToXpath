﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class AttributeTests
    {
        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_With_Single_Attribute_With_Text()
        {
            var xpath = LambdaToXpath.LambdaToXpath.Where(e => e.ElementName == "td" && e.Attribute("class").Text == "testClass");

            Assert.AreEqual("//td[@class='testClass']", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_With_Multiple_Attributes_With_Text()
        {
            var xpath = LambdaToXpath.LambdaToXpath.Where(e => e.ElementName == "td" && e.Attribute("class").Text == "testClass" && e.Attribute("id").Text == "div1" && e.Attribute("data-test1").Text == "someData");

            Assert.AreEqual("//td[@data-test1='someData' and @id='div1' and @class='testClass']", xpath);
        }
    }
}