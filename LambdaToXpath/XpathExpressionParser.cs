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
    public class XpathExpressionParser
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

                //Don't execute functions defined on internal objects 
                if (me.Member.Module.Assembly.FullName == typeof(XpathExpressionParser).Assembly.FullName)
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

                return Expression.Lambda(me).Compile().DynamicInvoke();
            }

            else if (operation.NodeType == ExpressionType.Equal)
            {
                BinaryExpression be = (BinaryExpression)operation;

                var e = string.Format("{0} == {1}", ParseSubExpressions(be.Left).ToString(), ParseSubExpressions(be.Right).ToString());
                return e;
            }

            else if (operation.NodeType == ExpressionType.Convert)
            {
                var value = Expression.Lambda(operation).Compile().DynamicInvoke();
                return Expression.Convert(Expression.Constant(value),value.GetType());
            }

            return null;
        }

        public static void Parse(Element element, string expressionPart)
        {
            bool done = ElementParser.ParseParent(element, expressionPart);

            if (done == false)
            {
                done = ElementParser.ParseContextElement(element, expressionPart);
            }
            if (done == false)
            {
                done = ElementParser.ParseSiblings(element, expressionPart);
            }
            if (done == false)
            {
                done = ElementParser.ParsePosition(element, expressionPart);
            }
            if (done == false)
            {
                done = ElementParser.ParseRelatives(element, expressionPart);
            }
        }

      
    }
}
