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
                Parse(element, (ExpressionTerm)ParseSubExpressions(chain.Right));
                ParseExpression((BinaryExpression)chain.Left, element);
            }
            else
            {
                ExpressionTerm expressionTerm = (ExpressionTerm)ParseSubExpressions(chain.Right);
                expressionTerm.Function = chain.Left.ToString();
                Parse(element, expressionTerm);
            }
        }

        private static object ParseSubExpressions(Expression operation)
        {
            if(operation.NodeType == ExpressionType.Constant)
            {
                var constant = ((ConstantExpression)operation).Value;

                if (constant is string)
                {
                    return new ExpressionTerm() { Value = constant.ToString() };
                }

                return constant;               
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
                    var retValue = ElementParser.ResolveFieldOrProperty(me, instance);

                    if (retValue is string)
                    {
                        return new ExpressionTerm() { Value = retValue.ToString() };
                    }

                    return retValue;
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

                return new ExpressionTerm() { Value = InvokeExpression(me).ToString() };
            }

            else if (operation.NodeType == ExpressionType.Equal)
            {
                BinaryExpression be = (BinaryExpression)operation;

                be = (BinaryExpression)be.ReduceExtensions();

                string functionName = null;
                var leftExpression = ParseSubExpressions(be.Left);
                string functionArgument = null;

                if (leftExpression is ExpressionTerm)
                {
                    var expr = (ExpressionTerm)leftExpression;
                    functionName = expr.Function;
                    functionArgument = expr.FunctionArgument;
                }
                else
                {
                    functionName = leftExpression.ToString();
                }

                string value = null;

                var rightExpression = ParseSubExpressions(be.Right);

                if (rightExpression is string)
                {
                    value = rightExpression.ToString();
                }
                if (rightExpression is ExpressionTerm)
                {
                    var expr = (ExpressionTerm)rightExpression;
                    value = expr.Value;
                    
                }
                else
                {
                    //Ignore == true/false in expressions
                    if (rightExpression.GetType() == typeof(bool))
                    {
                        value = ((ExpressionTerm)leftExpression).Value;
                    }
                    else
                    {
                        value = rightExpression.ToString();
                    }
                }

                return new ExpressionTerm() { Function = functionName, Value = value,FunctionArgument = functionArgument };
            }

            else if (operation.NodeType == ExpressionType.Convert)
            {
                var value = InvokeExpression(operation);
                return new ExpressionTerm() { Value = value.ToString() };
            }

            return null;
        }

        private static object InvokeExpression(Expression operation)
        {
            return Expression.Lambda(operation).Compile().DynamicInvoke();
        }

        public static void Parse(Element element, ExpressionTerm expressionTerm)
        {
            foreach (var parser in ElementParser.GetAllParsers())
            {
                bool done = parser(element, expressionTerm);
                if (done == true)
                {
                    return;
                }
            }
        }
    }
}
