using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LambdaToXpath;

namespace UnitTests
{
    [TestClass]
    public class ParentTests
    {
        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_With_Parent_With_One_Attribute_Containing_Text_And_Another_Attribute_With_Exact_Text()
        {
            var xpath = CreateXpath.Where(e => e.ElementName == "td" && e.Attribute("id").Contains("_parent_child") && e.Attribute("class").Text == "myClass");

            Assert.AreEqual("//td[@class='myClass' and contains(@id,'_parent_child')]", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Element_With_Parent()
        {
            var xpath = CreateXpath.Where(e => e.ElementName == "td" && e.Parent.Name == "tr");

            Assert.AreEqual("//tr/td",xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Element_With_Parent_With_Attribute_Containing_Text()
        {
            var xpath = CreateXpath.Where(e => e.ElementName == "td" && e.Parent.Name == "tr" && e.Parent.Attribute("id").Contains("_parent_child"));

            Assert.AreEqual("//tr[contains(@id,'_parent_child')]/td", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Element_With_Parent_With_Attribute_With_Exact_Text()
        {
            var xpath = CreateXpath.Where(e => e.ElementName == "td" && e.Parent.Name == "tr" && e.Parent.Attribute("id").Text == "_parent_child");

            Assert.AreEqual("//tr[@id='_parent_child']/td", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Element_With_Parent_With_Multiple_Attributes_With_Exact_Text()
        {
            var xpath = CreateXpath.Where(e => e.ElementName == "td" && e.Parent.Name == "tr" && e.Parent.Attribute("id").Text == "_parent_child" && e.Parent.Attribute("class").Text == "myClass");

            Assert.AreEqual("//tr[@class='myClass' and @id='_parent_child']/td", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Element_With_Parent_With_Multiple_Attributes_Containing_Text()
        {
            var xpath = CreateXpath.Where(e => e.ElementName == "td" && e.Parent.Name == "tr" && e.Parent.Attribute("id").Contains("_parent_child") && e.Parent.Attribute("class").Contains("someClass"));

            Assert.AreEqual("//tr[contains(@class,'someClass') and contains(@id,'_parent_child')]/td", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Element_With_Parent_Defined_First_In_Expression()
        {
            var xpath = CreateXpath.Where(e => e.Parent.Name == "tr" && e.ElementName == "td");

            Assert.AreEqual("//tr/td", xpath);
        }
    }
}
