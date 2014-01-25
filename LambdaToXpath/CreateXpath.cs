using LambdaToXpath.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LambdaToXpath
{
    public class CreateXpath
    {
        public static string Where(Expression<Func<Element, bool>> exp)
        {
            Element element = new Element();

            XpathExpressionParser.ParseExpression((BinaryExpression)exp.Body, element);

            return XpathGenerator.ToXpathString(element);
        }
    }
}
