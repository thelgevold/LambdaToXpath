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
    public class LambdaExpressionParser
    {
        public static void ParseExpression(BinaryExpression chain, Element element)
        {
            if (chain.Left is BinaryExpression)
            {
                Parse(element, ParseSubExpressions(chain.Right).ToString());

                ParseExpression((BinaryExpression)chain.Left, element);
            }
            else
            {
                Parse(element, string.Format("{0} == {1}", chain.Left.ToString(), ParseSubExpressions(chain.Right).ToString()));
            }
        }

        private static object ParseSubExpressions(Expression operation)
        {
            if(operation.NodeType == ExpressionType.Constant)
            {
                return ((ConstantExpression)operation).Value;
            }

            else if (operation.NodeType == ExpressionType.MemberAccess)
            {
                MemberExpression me = (MemberExpression)operation;

                if (me.Expression.Type == typeof(Model.Attribute))
                {
                    return ElementParser.ParseAttribute(me);
                }

                //Don't execute properties defined on internal objects 
                if (me.Member.Module.Assembly.FullName == typeof(LambdaExpressionParser).Assembly.FullName)
                {
                    return operation;
                }

                //Evaluate values of variables/properties
                object instance = ParseSubExpressions(me.Expression);
                if (instance != null)
                {
                    return ElementParser.ResolveFieldOrProperty(me, instance);
                }

                return operation.ToString();
            }
            else if (operation.NodeType == ExpressionType.Call)
            {
                MethodCallExpression me = (MethodCallExpression)operation;

                if (me.Object != null && me.Object.Type == typeof(Model.Attribute))
                {
                    return ElementParser.ParseAttribute(me);
                }

                return InvokeExpression(me);
            }

            else if (operation.NodeType == ExpressionType.Equal)
            {
                BinaryExpression be = (BinaryExpression)operation;

                var e = string.Format("{0} == {1}", ParseSubExpressions(be.Left).ToString(), ParseSubExpressions(be.Right).ToString());
                return e;
            }

            else if (operation.NodeType == ExpressionType.Convert)
            {
                var value = InvokeExpression(operation);
                return Expression.Convert(Expression.Constant(value),value.GetType());
            }

            return null;
        }

        private static object InvokeExpression(Expression operation)
        {
            return Expression.Lambda(operation).Compile().DynamicInvoke();
        }

        public static void Parse(Element element, string expressionPart)
        {
            foreach (var parser in ElementParser.GetAllParsers())
            {
                bool done = parser(element,expressionPart);
                if (done == true)
                {
                    return;
                }
            }
        }
    }
}
