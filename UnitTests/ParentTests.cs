using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LambdaToXpath;

namespace UnitTests
{
    [TestClass]
    public class ParentTests
    {
        [TestMethod]
        public void Will_Generate_Xpath_For_Element_With_Parent()
        {
            var xpath = CreateXpath.Where(e => e.ElementName == "td" && e.Parent.Name == "tr");

            Assert.AreEqual("//tr/td",xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Element_With_Parent_Defined_First_In_Expression()
        {
            var xpath = CreateXpath.Where(e => e.Parent.Name == "tr" && e.ElementName == "td");

            Assert.AreEqual("//tr/td", xpath);
        }
    }
}
