using LambdaToXpath.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LambdaToXpath
{
    public class ElementParser
    {
        public static void ParseExpression(BinaryExpression chain, Element element)
        {
            if (chain.Left is BinaryExpression)
            {
                Parse(element, chain.Right.ToString());
                ParseExpression((BinaryExpression)chain.Left, element);
            }
            else
            {
                Parse(element, string.Format("{0} == {1}",chain.Left.ToString(),chain.Right.ToString()));
            }
        }

        public static void Parse(Element element, string expressionPart)
        {
            if(expressionPart.Contains("Parent.Name") == true)
            {
                element.Parent = new Parent() { Name = GetName(expressionPart) };
            }
            else if(expressionPart.Contains(".ElementName") == true)
            {
                element.ElementName = GetName(expressionPart);
            }
            else if (Regex.IsMatch(expressionPart,"Attribute(.*)\\.Text") == true)
            {
                var keyVal = GetAttributeNameAndValue(expressionPart);
                element.Attributes.Add(new Model.Attribute(keyVal.Key) { Text = CleanUp(keyVal.Value) });
            }
        }

        private static KeyValuePair<string,string> GetAttributeNameAndValue(string expressionPart)
        {
            var temp = expressionPart.Split("==".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            var attributeName = Regex.Match(expressionPart.Trim(')','('), @"(?<=\().+?(?=\))").Value;

            return new KeyValuePair<string, string>(CleanUp(attributeName), CleanUp(temp[1]));
        }

        private static string GetName(string expressionPart)
        {
            var temp = expressionPart.Split("==".ToCharArray(),StringSplitOptions.RemoveEmptyEntries);

            string name = temp[0];

            if (temp.Length == 2)
            {
                name = temp[1];
            }

            return CleanUp(name);
        }

        private static string CleanUp(string raw)
        {
            return raw.Trim().Trim(')').Trim('\\').Trim('"');
        }
    }
}
