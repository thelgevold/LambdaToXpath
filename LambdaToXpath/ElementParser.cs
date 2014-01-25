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
        private static string extractMethodArguments = @"(?<=\().+?(?=\))";

        public static string ParseAttribute(MethodCallExpression me)
        {
            var containsExpr = me.Arguments.Single();
            var containsValue = Expression.Lambda(containsExpr).Compile().DynamicInvoke().ToString();
            
            MethodCallExpression attribute = (MethodCallExpression)me.Object;

            var attrNameExpr = attribute.Arguments.Single();
            var attributeNameValue = Expression.Lambda(attrNameExpr).Compile().DynamicInvoke().ToString();
            

            string expression = me.ToString().Replace(containsExpr.ToString(), SurroundByQuotes(containsValue)).Replace(attrNameExpr.ToString(), SurroundByQuotes(attributeNameValue));
            return expression;
        }

        public static string ParseAttribute(MemberExpression me)
        {
            var attributeFunction = (MethodCallExpression)me.Expression;
            var value = Expression.Lambda(attributeFunction.Arguments[0]).Compile().DynamicInvoke();

            value = string.Format("\"{0}\"", value);

            var expression = me.ToString().Replace(attributeFunction.Arguments[0].ToString(), value.ToString());

            return expression;
        }

        private static string SurroundByQuotes(string value)
        {
            return string.Format("\"{0}\"", value); 
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

        public static bool ParseSiblings(Element element, string expressionPart)
        {
            if (expressionPart.Contains(".FollowingSibling.Name") == true)
            {
                element.Siblings.Add(new Sibling() { Name = GetValue(expressionPart) });
                return true;
            }
            else if (expressionPart.Contains(".PrecedingSibling.Name") == true)
            {
                element.Siblings.Add(new Sibling() { Name = GetValue(expressionPart), Preceding = true });
                return true;
            }

            return false;
        }

        public static bool ParseContextElement(Element element, string expressionPart)
        {
            if (expressionPart.Contains(".TargetElementName") == true)
            {
                element.TargetElementName = GetValue(expressionPart);
                return true;
            }
            else if (Regex.IsMatch(expressionPart, "Attribute(.*)\\.Text") == true)
            {
                var keyVal = GetAttributeNameAndValue(expressionPart);
                element.Attributes.Add(new Model.Attribute(keyVal.Key) { Text = CleanUp(keyVal.Value), ExactMatch = true });
                return true;
            }
            else if (Regex.IsMatch(expressionPart, "Attribute(.*)\\.Contains") == true)
            {
                var keyVal = GetAttributeNameAndContainedText(expressionPart);
                element.Attributes.Add(new Model.Attribute(keyVal.Key) { Text = CleanUp(keyVal.Value) });
                return true;
            }

            return false;
        }

        public static bool ParseRelatives(Element element, string expressionPart)
        {
            if (expressionPart.Contains(".Descendant") == true)
            {
                element.Relatives.Add(new Relative() { Descendant = true, Name = GetValue(expressionPart) });
                return true;
            }

            if (expressionPart.Contains(".Ancestor") == true)
            {
                element.Relatives.Add(new Relative() { Descendant = false, Name = GetValue(expressionPart) });
                return true;
            }

            return false;
        }

        public static bool ParsePosition(Element element, string expressionPart)
        {
            if (expressionPart.Contains(".Position") == true)
            {
                var matches = Regex.Matches(expressionPart.Trim('('), extractMethodArguments);
                element.Position = Int32.Parse(CleanUp(matches[0].Value));
                return true;
            }

            return false;
        }

        public static bool ParseParent(Element element, string expressionPart)
        {
            if (expressionPart.Contains("Parent.Name") == true)
            {
                EnsureParent(element);
                element.Parent.Name = GetValue(expressionPart);
                return true;
            }
            else if (Regex.IsMatch(expressionPart, "Parent.Attribute(.*)\\.Contains") == true)
            {
                EnsureParent(element);
                var keyVal = GetAttributeNameAndContainedText(expressionPart);
                element.Parent.Attributes.Add(new Model.Attribute(keyVal.Key) { Text = CleanUp(keyVal.Value) });
                return true;
            }
            else if (Regex.IsMatch(expressionPart, "Parent.Attribute(.*)\\.Text") == true)
            {
                EnsureParent(element);
                var keyVal = GetAttributeNameAndValue(expressionPart);
                element.Parent.Attributes.Add(new Model.Attribute(keyVal.Key) { Text = CleanUp(keyVal.Value), ExactMatch = true });
                return true;
            }
            else if (expressionPart.Contains("Parent.Position") == true)
            {
                var matches = Regex.Matches(expressionPart.Trim('('), extractMethodArguments);
                element.Parent.Position = Int32.Parse(CleanUp(matches[0].Value));
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

        public static KeyValuePair<string, string> GetAttributeNameAndContainedText(string expressionPart)
        {
            var matches = Regex.Matches(expressionPart.Trim('('), extractMethodArguments);

            return new KeyValuePair<string, string>(CleanUp(matches[0].Value), CleanUp(matches[1].Value));
        }

        public static KeyValuePair<string, string> GetAttributeNameAndValue(string expressionPart)
        {
            var temp = expressionPart.Split("==".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            var attributeName = Regex.Match(expressionPart.Trim(')', '('), extractMethodArguments).Value;

            return new KeyValuePair<string, string>(CleanUp(attributeName), CleanUp(temp[1]));
        }

        public static string GetValue(string expressionPart)
        {
            var temp = expressionPart.Split("==".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            string name = temp[0];

            if (temp.Length == 2)
            {
                name = temp[1];
            }

            return CleanUp(name);
        }

        public static string CleanUp(string raw)
        {
            return raw.Trim().Trim(')').Trim('\\').Trim('"');
        }
    }
}
