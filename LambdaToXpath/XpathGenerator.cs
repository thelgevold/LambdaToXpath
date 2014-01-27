using LambdaToXpath.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LambdaToXpath
{
    public class XpathGenerator
    {
        public static string ToXpathString(Element element)
        {
            StringBuilder xpath = new StringBuilder();
            xpath.Append("//");

            if (element.Parent != null)
            {
                xpath.Append(element.Parent.Name);

                if (element.Parent.ElementHasConditions == true)
                {
                    xpath.Append("[");
                    if (element.Parent.Attributes.Count > 0)
                    {
                        xpath.Append(string.Join(" and ", element.Parent.Attributes.Select(a => GetAttributePart(a))));
                    }
                    if (element.Parent.Text != null)
                    {
                        xpath.Append(EnsureAndOperator(element.Parent.Attributes.Count));
                        xpath.Append(string.Format("text()='{0}'", element.Parent.Text));
                    }
                    if (element.Parent.Position.HasValue == true)
                    {
                        xpath.Append(EnsureAndOperator(element.Parent.Attributes.Count,(element.Parent.Text ?? string.Empty).Length));
                        xpath.Append(string.Format("position()={0}", element.Parent.Position.Value));
                    }
                    xpath.Append("]");
                }
                xpath.Append("/");
            }

            xpath.Append(element.TargetElementName);

            if (element.ElementHasConditions == true)
            {
                xpath.Append("[");

                if (element.Attributes.Count > 0)
                {
                    xpath.Append(string.Join(" and ", element.Attributes.Select(a => GetAttributePart(a))));
                }

                if (element.Siblings.Count > 0)
                {
                    xpath.Append(EnsureAndOperator(element.Attributes.Count));
                    xpath.Append(string.Join(" and ", element.Siblings.Select(s => GetSiblingPart(s))));
                }

                if (element.Position.HasValue == true)
                {
                    xpath.Append(EnsureAndOperator(element.Attributes.Count,element.Siblings.Count));
                    xpath.Append(string.Format("position()={0}", element.Position.Value));
                }

                if (element.Relatives.Count > 0)
                {
                    xpath.Append(EnsureAndOperator(element.Attributes.Count, element.Siblings.Count, element.Position ?? 0));
                    xpath.Append(string.Join(" and ", element.Relatives.Select(r => GetRelativePart(r))));
                }

                if (element.TargetElementText != null)
                {
                    xpath.Append(EnsureAndOperator(element.Attributes.Count, element.Siblings.Count, element.Position ?? 0, element.Relatives.Count));
                    xpath.Append(string.Format("text()='{0}'",element.TargetElementText));
                }

                xpath.Append("]");
            }
            return xpath.ToString();
        }

        private static string EnsureAndOperator(params int[] conditions)
        {
            if (conditions.Sum() > 0)
            {
                return " and ";
            }

            return string.Empty;
        }

        private static string GetRelativePart(Relative r)
        {
            if (r.Descendant == true)
            {
                return string.Format("descendant::{0}", r.Name);
            }

            return string.Format("ancestor::{0}", r.Name);
        }

        private static string GetSiblingPart(Sibling s)
        {
            if (s.Preceding == true)
            {
                return string.Format("preceding-sibling::{0}", s.Name);
            }

            return string.Format("following-sibling::{0}", s.Name);
        }

        private static string GetAttributePart(Model.Attribute a)
        {
            if (a.ExactMatch == true)
            {
                return string.Format("@{0}='{1}'", a.Name, a.Text);
            }

            return string.Format("contains(@{0},'{1}')", a.Name, a.Text);
        }
    }
}
