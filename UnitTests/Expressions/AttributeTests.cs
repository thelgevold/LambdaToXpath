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
    public class AttributeTests
    {
        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_With_Single_Attribute_Containing_Text()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td" && e.Attribute("id").Contains("_parent_child") && e.Attribute("class").Text == "class1");

            Assert.AreEqual("//td[@class='class1' and contains(@id,'_parent_child')]", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_With_One_Attribute_Containing_Text_And_Another_Attribute_With_Exact_Text()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td" && e.Attribute("id").Contains("_parent_child"));

            Assert.AreEqual("//td[contains(@id,'_parent_child')]", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_With_Multiple_Attributes_Containing_Text()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td" && e.Attribute("id").Contains("_parent_child") == true && e.Attribute("class").Contains("myClass") == true);

            Assert.AreEqual("//td[contains(@class,'myClass') and contains(@id,'_parent_child')]", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_With_Single_Attribute_With_Text()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td" && e.Attribute("class").Text == "testClass");

            Assert.AreEqual("//td[@class='testClass']", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_With_Multiple_Attributes_With_Text()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td" && e.Attribute("class").Text == "testClass" && e.Attribute("id").Text == "div1" && e.Attribute("data-test1").Text == "someData");

            Assert.AreEqual("//td[@data-test1='someData' and @id='div1' and @class='testClass']", xpath);
        }
    }
}
