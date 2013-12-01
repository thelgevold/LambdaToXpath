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
                xpath.Append("/");
            }

            xpath.Append(element.ElementName);

            if (element.Attributes.Count > 0)
            {
                xpath.Append("[");
                xpath.Append(string.Join(" and ",element.Attributes.Select(a => string.Format("@{0}='{1}'",a.Name,a.Text))));
                xpath.Append("]");
            }
            
            return xpath.ToString();
        }
    }
}
