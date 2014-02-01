using LambdaToXpath.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LambdaToXpath
{
    public class NonElement
    {
        public string Text
        {
            get { return "/text()"; }
        }

        public string Attribute(string name)
        {
            return string.Format("@{0}",name.Trim('@'));
        }
    }

    public class Where 
    {
        public Where(string expr) 
        {
            Expr = expr;
        }

        private string Expr { get; set; }

        public static implicit operator string(Where w)  
        {
            return w.Expr;
        }

        public string Select(Expression<Func<NonElement,string>> expr) 
        {
            NonElement nonElement = new NonElement();
            MemberExpression mex = expr.Body as MemberExpression;
            if (mex != null && mex.Member.Name == "Text")
            {
                return string.Format("{0}{1}",this.Expr,nonElement.Text);
            }
            else 
            {
                MethodCallExpression me = (MethodCallExpression)expr.Body;
                var val = Expression.Lambda(me.Arguments[0]).Compile().DynamicInvoke().ToString();
                return string.Format("{0}/@{1}", this.Expr, val);
            }
       }
    }

    public class CreateXpath
    {
        public static Where Where(Expression<Func<Element, bool>> exp)
        {
            Element element = new Element();

            LambdaExpressionParser.ParseExpression((BinaryExpression)exp.Body, element);

            return new Where(XpathGenerator.ToXpathString(element));
        }

    }
}
