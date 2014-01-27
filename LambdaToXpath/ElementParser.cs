using LambdaToXpath.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LambdaToXpath
{
    public static class ElementParser
    {
        /// <summary>
        /// .Attribute(..).Contains(..)
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public static ExpressionTerm ParseAttribute(MethodCallExpression me)
        {
            var containsExpr = me.Arguments.Single();
            var containsValue = Expression.Lambda(containsExpr).Compile().DynamicInvoke().ToString();
            
            MethodCallExpression attribute = (MethodCallExpression)me.Object;

            var attrNameExpr = attribute.Arguments.Single();
            var attributeNameValue = Expression.Lambda(attrNameExpr).Compile().DynamicInvoke().ToString();
            
            string expression = me.ToString().Replace(containsExpr.ToString(), SurroundByQuotes(containsValue)).Replace(attrNameExpr.ToString(), SurroundByQuotes(attributeNameValue));

            return new ExpressionTerm { Function = expression, Value = containsValue, FunctionArgument = attributeNameValue };
        }

        /// <summary>
        /// .Attribute(..).Text = ""
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public static ExpressionTerm ParseAttribute(MemberExpression me)
        {
            var attributeFunction = (MethodCallExpression)me.Expression;
            var value = Expression.Lambda(attributeFunction.Arguments[0]).Compile().DynamicInvoke();

            string argument = value.ToString();

            value = string.Format("\"{0}\"", value);

            var expression = me.ToString().Replace(attributeFunction.Arguments[0].ToString(), value.ToString());

            return new ExpressionTerm() {Function = expression,Value = value.ToString(),FunctionArgument = argument };
        }

        private static string SurroundByQuotes(string value)
        {
            return string.Format("\"{0}\"", value); 
        }

        public static List<Func<Element, ExpressionTerm, bool>> GetAllParsers()
        {
            List<Func<Element, ExpressionTerm, bool>> parsers = new List<Func<Element, ExpressionTerm, bool>>();

            parsers.Add((element, expressionTerm) => ElementParser.ParseParent(element, expressionTerm));
            parsers.Add((element, expressionTerm) => ElementParser.ParseContextElement(element, expressionTerm));
            parsers.Add((element, expressionTerm) => ElementParser.ParseSiblings(element, expressionTerm));
            parsers.Add((element, expressionTerm) => ElementParser.ParsePosition(element, expressionTerm));
            parsers.Add((element, expressionTerm) => ElementParser.ParseRelatives(element, expressionTerm));

            return parsers;
        }

        public static object ResolveFieldOrProperty(MemberExpression me,object instance)
        {
            if (me.Member.MemberType == MemberTypes.Field)
            {
                return ((FieldInfo)me.Member).GetValue(instance);
            }
            else if (me.Member.MemberType == MemberTypes.Property)
            {
                return ((PropertyInfo)me.Member).GetValue(instance, null);
            }

            return null;
        }

        public static bool ParseSiblings(Element element, ExpressionTerm expressionPart)
        {
            if (expressionPart.IsValidFunctionByName(".FollowingSibling.Name") == true)
            {
                element.Siblings.Add(new Sibling() { Name = expressionPart.Value });
                return true;
            }
            else if (expressionPart.IsValidFunctionByName(".PrecedingSibling.Name") == true)
            {
                element.Siblings.Add(new Sibling() { Name = expressionPart.Value, Preceding = true });
                return true;
            }

            return false;
        }

        public static bool ParseContextElement(Element element, ExpressionTerm expressionPart)
        {
            if (expressionPart.IsValidFunctionByName(".TargetElementName") == true)
            {
                element.TargetElementName = expressionPart.Value;
                return true;
            }
            else if (expressionPart.IsValidFunctionByName(".TargetElementText") == true)
            {
                element.TargetElementText = expressionPart.Value;
                return true;
            }
            else if (expressionPart.IsValidFunctionByRegex("Attribute(.*)\\.Text") == true)
            {
                element.Attributes.Add(new Model.Attribute(expressionPart.FunctionArgument) { Text = expressionPart.Value, ExactMatch = true });
                return true;
            }
            else if (expressionPart.IsValidFunctionByRegex("Attribute(.*)\\.Contains") == true)
            {
                element.Attributes.Add(new Model.Attribute(expressionPart.FunctionArgument) { Text = expressionPart.Value });
                return true;
            }

            return false;
        }

        public static bool ParseRelatives(Element element, ExpressionTerm expressionPart)
        {
            if (expressionPart.IsValidFunctionByName(".Descendant") == true)
            {
                element.Relatives.Add(new Relative() { Descendant = true, Name = expressionPart.Value });
                return true;
            }

            if (expressionPart.IsValidFunctionByName(".Ancestor") == true)
            {
                element.Relatives.Add(new Relative() { Descendant = false, Name = expressionPart.Value });
                return true;
            }

            return false;
        }

        public static bool ParsePosition(Element element, ExpressionTerm expressionPart)
        {
            if (expressionPart.IsValidFunctionByName(".Position") == true)
            {
                element.Position = Int32.Parse(expressionPart.Value);
                return true;
            }

            return false;
        }

        public static bool ParseParent(Element element, ExpressionTerm expressionPart)
        {
            if (expressionPart.IsValidFunctionByName("Parent.Name") == true)
            {
                EnsureParent(element);
                element.Parent.Name = expressionPart.Value;
                return true;
            }
            else if (expressionPart.IsValidFunctionByName("Parent.Text") == true)
            {
                EnsureParent(element);
                element.Parent.Text = expressionPart.Value;
                return true;
            }
            else if (expressionPart.IsValidFunctionByRegex("Parent.Attribute(.*)\\.Contains") == true)
            {
                EnsureParent(element);
                element.Parent.Attributes.Add(new Model.Attribute(expressionPart.FunctionArgument) { Text = expressionPart.Value });
                return true;
            }
            else if (expressionPart.IsValidFunctionByRegex("Parent.Attribute(.*)\\.Text") == true)
            {
                EnsureParent(element);
                element.Parent.Attributes.Add(new Model.Attribute(expressionPart.FunctionArgument) { Text = expressionPart.Value, ExactMatch = true });
                return true;
            }
            else if (expressionPart.IsValidFunctionByName("Parent.Position") == true)
            {
                element.Parent.Position = Int32.Parse(expressionPart.Value);
                return true;
            }

            return false;
        }

        public static void EnsureParent(Element element)
        {
            if (element.Parent == null)
            {
                element.Parent = new Parent();
            }
        }
    }
}
