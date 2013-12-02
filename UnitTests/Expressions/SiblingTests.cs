using LambdaToXpath;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class SiblingTests
    {
        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_With_Following_Sibling()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td" && e.FollowingSibling.Name == "td");

            Assert.AreEqual("//td[following-sibling::td]", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_With_Preceding_Sibling()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td" && e.PrecedingSibling.Name == "td");

            Assert.AreEqual("//td[preceding-sibling::td]", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_With_Preceding_Sibling_And_Following_Sibling()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "b" && e.PrecedingSibling.Name == "a" && e.FollowingSibling.Name == "c");

            Assert.AreEqual("//b[following-sibling::c and preceding-sibling::a]", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_With_Preceding_Sibling_And_Following_Sibling_And_Attribute()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "b" && e.PrecedingSibling.Name == "a" && e.FollowingSibling.Name == "c" && e.Attribute("class").Text == "myClass");

            Assert.AreEqual("//b[@class='myClass' and following-sibling::c and preceding-sibling::a]", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_With_Preceding_Sibling_And_Following_Sibling_And_Two_Attribute()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "b" && e.PrecedingSibling.Name == "a" && e.FollowingSibling.Name == "c" && e.Attribute("class").Text == "myClass" && e.Attribute("id").Contains("div_span"));

            Assert.AreEqual("//b[contains(@id,'div_span') and @class='myClass' and following-sibling::c and preceding-sibling::a]", xpath);
        }
    }
}
