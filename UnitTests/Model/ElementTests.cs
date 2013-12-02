using LambdaToXpath.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Model
{
    [TestClass]
    public class ElementTests
    {
        [TestMethod]
        public void Will_Create_Square_Brackets_If_Element_Has_Conditions()
        {
            Element element = new Element();

            element.Attributes.Add(new LambdaToXpath.Model.Attribute("test"));
            element.Siblings.Add(new Sibling());
       
            Assert.AreEqual(true, element.ElementHasConditions);
        }

        [TestMethod]
        public void Will_Not_Create_Square_Brackets_If_Element_Does_Not_Have_Conditions()
        {
            Element element = new Element();

            Assert.AreEqual(false, element.ElementHasConditions);
        }
    }
}
