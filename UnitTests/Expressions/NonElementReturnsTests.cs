using LambdaToXpath;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Expressions
{
    [TestClass]
    public class NonElementReturnsTests
    {
        [TestMethod]
        public void Will_Return_Text_Property()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td").Select(e => e.Text);

            Assert.AreEqual("//td/text()", xpath);
        }

        [TestMethod]
        public void Will_Return_Attribute_Property1()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td" && e.Attribute("class").Text == "myClass").Select(e => e.Attribute("class"));

            Assert.AreEqual("//td[@class='myClass']/@class", xpath);
        }

        [TestMethod]
        public void Will_Return_Attribute_Property2()
        {
            string classAttr = "class";
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td").Select(e => e.Attribute(classAttr));
            Assert.AreEqual("//td/@class", xpath);
        }

        [TestMethod]
        public void Will_Return_Attribute_Property3()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td").Select(e => e.Attribute(GetAttributeName()));
            Assert.AreEqual("//td/@class", xpath);
        }

        private string GetAttributeName() 
        {
            string classAttr = "class";
            return classAttr;
        }
    }
}
