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
    public class DecendantAndAncestorTests
    {
        [TestMethod]
        public void Will_Generate_Xpath_For_Element_With_Descendant()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "table" && e.Descendant.Name == "td");

            Assert.AreEqual("//table[descendant::td]",xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Element_With_Ancestor()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td" && e.Ancestor.Name == "table");

            Assert.AreEqual("//td[ancestor::table]", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Element_With_Ancestor_Specified_Using_Variables()
        {
            string td = "td";
            string table = "table";
            var xpath = CreateXpath.Where(e => e.TargetElementName == td && e.Ancestor.Name == table);

            Assert.AreEqual("//td[ancestor::table]", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Element_With_Ancestor_And_Descendant()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td" && e.Ancestor.Name == "table" && e.Descendant.Name == "div");

            Assert.AreEqual("//td[descendant::div and ancestor::table]", xpath);
        }
    }
}
