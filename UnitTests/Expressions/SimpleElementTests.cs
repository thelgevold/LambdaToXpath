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
            var xpath = CreateXpath.Where(e => e.TargetElementName == "td");

            Assert.AreEqual("//td", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_Specified_By_A_Variable()
        {
            string tdElement = "td";
            var xpath = CreateXpath.Where(e => e.TargetElementName == tdElement);

            Assert.AreEqual("//td", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_With_Text()
        {
            string tdElement = "td";
            var xpath = CreateXpath.Where(e => e.TargetElementName == tdElement && e.TargetElementText == "my test");

            Assert.AreEqual("//td[text()='my test']", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_Starting_With_Text()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "span" && e.TargetElementText.StartsWith("Test"));

            Assert.AreEqual("//span[starts-with(text(),'Test')]", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_Not_Starting_With_Text()
        {
            var xpath = CreateXpath.Where(e => e.TargetElementName == "span" && e.TargetElementText.StartsWith("Test") == false);

            Assert.AreEqual("//span[not(starts-with(text(),'Test'))]", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_Starting_With_Text_Represented_By_Variable()
        {
            string test = "Test";
            var xpath = CreateXpath.Where(e => e.TargetElementName == "span" && e.TargetElementText.StartsWith(test));

            Assert.AreEqual("//span[starts-with(text(),'Test')]", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_Containing_Text()
        {
            string tdElement = "td";
            var xpath = CreateXpath.Where(e => e.TargetElementName == tdElement && e.TargetElementText.Contains("my test"));

            Assert.AreEqual("//td[contains(text(),'my test')]", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_Not_Containing_Text()
        {
            string tdElement = "td";
            var xpath = CreateXpath.Where(e => e.TargetElementName == tdElement && e.TargetElementText.Contains("my test") == false);

            Assert.AreEqual("//td[not(contains(text(),'my test'))]", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_Specified_By_A_Variable_Inside_Another_Object()
        {
            TestClass testClass = new TestClass();
            testClass.Name = "tr";
            var xpath = CreateXpath.Where(e => e.TargetElementName == testClass.Name);

            Assert.AreEqual("//tr", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_Specified_By_A_Variable_Inside_Deep_Object()
        {
            TestClass2 outer = new TestClass2();

            outer.Test = new TestClass() { Name = "a" };

            var xpath = CreateXpath.Where(e => e.TargetElementName == outer.Test.Name);

            Assert.AreEqual("//a", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_Specified_By_Value_Returned_From_Static_Method_Call()
        {
            TestClass2 outer = new TestClass2();

            outer.Test = new TestClass() { Name = "a" };

            var xpath = CreateXpath.Where(e => e.TargetElementName == GetValue1("td"));

            Assert.AreEqual("//td", xpath);

            xpath = CreateXpath.Where(e => e.TargetElementName == GetValue1());

            Assert.AreEqual("//td", xpath);
        }

        [TestMethod]
        public void Will_Generate_Xpath_For_Simple_Element_Specified_By_Value_Returned_From_Instance_Method_Call()
        {
            TestClass2 outer = new TestClass2();

            outer.Test = new TestClass() { Name = "a" };

            var xpath = CreateXpath.Where(e => e.TargetElementName == GetValue2("table"));

            Assert.AreEqual("//table", xpath);

            xpath = CreateXpath.Where(e => e.TargetElementName == GetValue2());

            Assert.AreEqual("//table", xpath);
        }

        private static string GetValue1(string e)
        {
            return e;
        }

        private string GetValue2(string e)
        {
            return e;
        }

        private static string GetValue1()
        {
            return "td";
        }

        private string GetValue2()
        {
            return "table";
        }

        public class TestClass
        {
            public string Name { get; set; }

            public string Id { get; set; }

            public int Count { get; set; }
        }

        public class TestClass2
        {
            public TestClass Test { get; set; }
        }
    }
}
