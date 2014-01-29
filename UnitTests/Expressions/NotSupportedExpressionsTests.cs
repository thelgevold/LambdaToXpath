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
    public class NotSupportedExpressionsTests
    {
        [ExpectedException(typeof(NotSupportedException))]
        [TestMethod]
        public void Or_Not_Supported()
        {
            CreateXpath.Where(e => e.TargetElementName == "td" || e.Parent.Name == "tr");
        }

        [ExpectedException(typeof(NotSupportedException))]
        [TestMethod]
        public void Not_Equal_Not_Supported()
        {
            CreateXpath.Where(e => e.TargetElementName != "td" && e.Parent.Name == "tr");
        }

        [ExpectedException(typeof(NotSupportedException))]
        [TestMethod]
        public void Not_Expression_Not_Supported()
        {
            CreateXpath.Where(e => e.TargetElementName == "td" && !e.TargetElementText.Contains("test"));
        }
    }
}
