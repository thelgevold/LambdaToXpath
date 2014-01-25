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
    public class NoConflictTests
    {
        [TestMethod]
        public void No_Conflict_With_Parent_Function()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "Parent.Name" && e.Parent.Name == "table");
            Assert.AreEqual("//table/Parent.Name", xpath);
        }

        [TestMethod]
        public void No_Conflict_With_Attribute()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "Attribute(\"test\").Text" && e.Attribute("test").Text == "test2");
            Assert.AreEqual("//Attribute(\"test\").Text[@test='test2']", xpath);
        }

        [TestMethod]
        public void No_Conflict_With_Sibling()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == ".FollowingSibling.Name" && e.PrecedingSibling.Name == "td");
            Assert.AreEqual("//.FollowingSibling.Name[preceding-sibling::td]", xpath);
        }
    }
}
