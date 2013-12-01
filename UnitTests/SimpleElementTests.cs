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
    public class SimpleElementTests
    {
        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_With_Just_Name()
        {
            var xpath = CreateXpath.Where(e => e.ElementName == "td");

            Assert.AreEqual("//td", xpath);
        }
    }
}
