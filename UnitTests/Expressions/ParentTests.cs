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
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td" && e.Attribute("id").Contains("_parent_child") && e.Attribute("class").Text == "myClass");

            Assert.AreEqual("//td[@class='myClass' and contains(@id,'_parent_child')]", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Element_With_Parent()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td" && e.Parent.Name == "tr");

            Assert.AreEqual("//tr/td",xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Element_With_Parent_With_Text()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td" && e.Parent.Name == "tr" && e.Parent.Text == "test");

            Assert.AreEqual("//tr[text()='test']/td", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Element_With_Parent_With_Text_And_Position_And_Attribute()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td" && e.Parent.Name == "tr" && e.Parent.Text == "test" && e.Parent.Position == 5 && e.Parent.Attribute("id").Text == "parent");

            Assert.AreEqual("//tr[@id='parent' and text()='test' and position()=5]/td", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Element_With_Parent_Specified_By_Variable()
        {
            string parentName = "tr";
            string child = "td";
            var xpath = CreateXpath.Where(e => e.TargetElementName == child && e.Parent.Name == parentName);

            Assert.AreEqual("//tr/td", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Element_With_Parent_Specified_Using_Variable()
        {
            string parent = "tr";
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td" && e.Parent.Name == parent);

            Assert.AreEqual("//tr/td", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Element_With_Parent_With_Attribute_Containing_Text()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td" && e.Parent.Name == "tr" && e.Parent.Attribute("id").Contains("_parent_child"));

            Assert.AreEqual("//tr[contains(@id,'_parent_child')]/td", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Element_With_Parent_With_Attribute_With_Exact_Text()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td" && e.Parent.Name == "tr" && e.Parent.Attribute("id").Text == "_parent_child");

            Assert.AreEqual("//tr[@id='_parent_child']/td", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Element_With_Parent_With_Attribute_With_Exact_Text_Specififed_By_Attributes()
        {
            string attrText = "_parent_child";
            string attrName = "id";
            string parentName = "tr";
            string targetName = "td";
            var xpath = CreateXpath.Where(e => e.TargetElementName == targetName && e.Parent.Name == parentName && e.Parent.Attribute(attrName).Text == attrText);

            Assert.AreEqual("//tr[@id='_parent_child']/td", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Element_With_Parent_With_Attribute_With_Exact_Text_All_Specified_Using_Attributes()
        {
            string text = "_parent_child";
            string parentName = "tr";
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td" && e.Parent.Name == parentName && e.Parent.Attribute("id").Text == text);

            Assert.AreEqual("//tr[@id='_parent_child']/td", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Element_With_Parent_With_Multiple_Attributes_With_Exact_Text()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td" && e.Parent.Name == "tr" && e.Parent.Attribute("id").Text == "_parent_child" && e.Parent.Attribute("class").Text == "myClass");

            Assert.AreEqual("//tr[@class='myClass' and @id='_parent_child']/td", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Element_With_Parent_With_Multiple_Attributes_Containing_Text()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td" && e.Parent.Name == "tr" && e.Parent.Attribute("id").Contains("_parent_child") && e.Parent.Attribute("class").Contains("someClass"));

            Assert.AreEqual("//tr[contains(@class,'someClass') and contains(@id,'_parent_child')]/td", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Element_With_Parent_Defined_First_In_Expression()
        {
            var xpath = CreateXpath.Where(e => e.Parent.Name == "tr" && e.TargetElementName == "td");

            Assert.AreEqual("//tr/td", xpath);
        }
    }
}
