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
    public class MiscTests
    {
        [TestMethod]
        public void Partial_Text_And_Partial_Attribute()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "li" && e.TargetElementText.Contains("text1234") && e.Attribute("class").Contains("myClass"));

            Assert.AreEqual("//li[contains(@class,'myClass') and contains(text(),'text1234')]", xpath);
        }

        [TestMethod]
        public void Exact_Text_And_Exact_Attribute()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "li" && e.TargetElementText == "text1234" && e.Attribute("class").Text == "myClass");

            Assert.AreEqual("//li[@class='myClass' and text()='text1234']", xpath);
        }
    }
}
