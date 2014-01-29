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
        public void Will_Generate_Xpath_For_Simple_Element_With_Single_Attribute_Not_Containing_Text()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td" && e.Attribute("id").Contains("_parent_child") == false);

            Assert.AreEqual("//td[not(contains(@id,'_parent_child'))]", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_With_Attributes_Defined_By_Variables()
        {
            string myClass1 = "displayNone";
            string attrName = "class";
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td" && e.Attribute(attrName).Text == myClass1);

            Assert.AreEqual("//td[@class='displayNone']", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_With_Partial_Attribute_Defined_By_Variables1()
        {
            string myClass1 = "displayNone";
            string attrName = "class";
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td" && e.Attribute(attrName).Contains(myClass1));

            Assert.AreEqual("//td[contains(@class,'displayNone')]", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_With_Partial_Attribute_Defined_By_Variables2()
        {
            string myClass1 = "displayNone";
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td" && e.Attribute("id").Contains(myClass1));

            Assert.AreEqual("//td[contains(@id,'displayNone')]", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_With_Partial_Attribute_Defined_By_Variables3()
        {
            string attrName = "id";
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td" && e.Attribute(attrName).Contains("displayNone"));

            Assert.AreEqual("//td[contains(@id,'displayNone')]", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_With_Partial_Attribute_Defined_By_Variables4()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td" && e.Attribute(GetAttributeName()).Contains("displayNone"));

            Assert.AreEqual("//td[contains(@id,'displayNone')]", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_With_Partial_Attribute_Defined_By_Variables5()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td" && e.Attribute(GetAttributeName()).Contains(GetAttributeContent()));

            Assert.AreEqual("//td[contains(@id,'displayNone')]", xpath);
        }

        private string GetAttributeName()
        {
            return "id";
        }

        private string GetAttributeContent()
        {
            return "displayNone";
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_With_One_Attribute_Containing_Text_And_Another_Attribute_With_Exact_Text()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td" && e.Attribute("id").Contains("_parent_child"));

            Assert.AreEqual("//td[contains(@id,'_parent_child')]", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_With_Multiple_Attributes_Containing_Text1()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td" && e.Attribute("id").Contains("_parent_child") == true && e.Attribute("class").Contains("myClass") == true);

            Assert.AreEqual("//td[contains(@class,'myClass') and contains(@id,'_parent_child')]", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_With_Multiple_Attributes_Containing_Text2()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td" && e.Attribute("id").Contains("_parent_child") && e.Attribute("class").Contains("myClass"));

            Assert.AreEqual("//td[contains(@class,'myClass') and contains(@id,'_parent_child')]", xpath);
        }

        //"Not implemented yet"
        [Ignore]
        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_With_Multiple_Attributes_Containing_Text3()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td" && e.Attribute("id").Contains("_parent_child") == false && e.Attribute("class").Contains("myClass") == false);

            Assert.AreEqual("//td[contains(@class,'myClass') and contains(@id,'_parent_child')]", xpath);
        }

        //"Not implemented yet"
        [Ignore]
        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_With_Multiple_Attributes_Containing_Text4()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td" && !e.Attribute("id").Contains("_parent_child") && !e.Attribute("class").Contains("myClass"));

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
