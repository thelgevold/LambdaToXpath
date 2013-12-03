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
    public class PositionTests
    {
        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_With_Position()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td" && e.Position == 5);

            Assert.AreEqual("//td[position()=5]", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_With_Position_And_Attribute()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td" && e.Position == 5 && e.Attribute("class").Text == "myClass");

            Assert.AreEqual("//td[@class='myClass' and position()=5]", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_With_Position_And_Parent()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td" && e.Position == 5 && e.Parent.Name == "tr");

            Assert.AreEqual("//tr/td[position()=5]", xpath);
        }
    }
}
