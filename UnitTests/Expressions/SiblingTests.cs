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
        public void Will_Generate_Xpath_For_Simple_Element_With_Following_Sibling_Specified_By_Attributes()
        {
            string siblingName = "td2";
            string elementName = "td1";
            var xpath = CreateXpath.Where(e => e.TargetElementName == elementName && e.FollowingSibling.Name == siblingName);

            Assert.AreEqual("//td1[following-sibling::td2]", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_With_Preceding_Sibling()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td" && e.PrecedingSibling.Name == "td");

            Assert.AreEqual("//td[preceding-sibling::td]", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_With_Preceding_Sibling_Specified_By_Attributes()
        {
            string siblingName = "td2";
            string elementName = "td1";

            var xpath = CreateXpath.Where(e => e.TargetElementName == elementName && e.PrecedingSibling.Name == siblingName);

            Assert.AreEqual("//td1[preceding-sibling::td2]", xpath);
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
        public void Will_Generate_Xpath_For_Simple_Element_With_Preceding_Sibling_And_Following_Sibling_And_Attribute_Using_Variables()
        {
            string aVal = "a";
            string bVal = "b";
            string cVal = "c";
            string classVal = "class";

            var xpath = CreateXpath.Where(e => e.TargetElementName == bVal && e.PrecedingSibling.Name == aVal && e.FollowingSibling.Name == cVal && e.Attribute("class").Text == GetClassName());

            Assert.AreEqual("//b[@class='myClass' and following-sibling::c and preceding-sibling::a]", xpath);
        }

        private string GetClassName()
        {
            return "myClass";
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_With_Preceding_Sibling_And_Following_Sibling_And_Two_Attribute()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "b" && e.PrecedingSibling.Name == "a" && e.FollowingSibling.Name == "c" && e.Attribute("class").Text == "myClass" && e.Attribute("id").Contains("div_span"));

            Assert.AreEqual("//b[contains(@id,'div_span') and @class='myClass' and following-sibling::c and preceding-sibling::a]", xpath);
        }
    }
}
