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
            ElementParser.ThrowIfNotSupported(chain);

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
                return GetConstant(operation);
            }

            else if (operation.NodeType == ExpressionType.MemberAccess)
            {
                return ParseMemberAccessExpression(operation);
            }
            else if (operation.NodeType == ExpressionType.Call)
            {
               return CallMethod(operation);
            }

            else if (operation.NodeType == ExpressionType.Equal)
            {
                return ParseEqualityExpression(operation);
            }

            else if (operation.NodeType == ExpressionType.Convert)
            {
                UnaryExpression me = (UnaryExpression)operation;
                
                var value = InvokeExpression(me.Operand);
                return new ExpressionTerm() { Value = value.ToString() };
            }

            return null;
        }

        private static object ParseMemberAccessExpression(Expression operation)
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


        private static ExpressionTerm ParseEqualityExpression(Expression operation)
        {
            BinaryExpression be = (BinaryExpression)operation;

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

            var expressionTerm = new ExpressionTerm() { Function = functionName, FunctionArgument = functionArgument };
          
            var rightExpression = ParseSubExpressions(be.Right);

            if (rightExpression is string)
            {
                expressionTerm.Value = rightExpression.ToString();
            }
            if (rightExpression is ExpressionTerm)
            {
                var expr = (ExpressionTerm)rightExpression;
                expressionTerm.Value = expr.Value;
            }
            else
            {
                //Ignore == true/false in expressions
                if (rightExpression.GetType() == typeof(bool))
                {
                    expressionTerm.Value = ((ExpressionTerm)leftExpression).Value;
                    expressionTerm.ExpressionBooleanCondition = (bool)rightExpression;
                }
                else
                {
                    expressionTerm.Value = rightExpression.ToString();
                }
            }
            return expressionTerm;
        }

        private static object InvokeExpression(Expression operation)
        {
            return Expression.Lambda(operation).Compile().DynamicInvoke();
        }

        private static object GetConstant(Expression operation)
        {
            var constant = ((ConstantExpression)operation).Value;

            if (constant is string)
            {
                return new ExpressionTerm() { Value = constant.ToString() };
            }

            return constant;               
        }

        private static object CallMethod(Expression operation)
        {
            MethodCallExpression me = (MethodCallExpression)operation;
            
            var mex = me.Object as MemberExpression;

            if (mex != null &&mex.Member != null && (mex.Member.Name == "TargetElementText" || mex.Member.Name == "Text"))
            {
                string textFunction = "TargetElementText";

                if (mex.Member.Name == "Text")
                {
                    textFunction = "Parent.Text";
                }

                var text = Expression.Lambda(me.Arguments[0]).Compile().DynamicInvoke().ToString();
                return new ExpressionTerm() { Value = text, Function = textFunction, FunctionArgument = "Contains" };
            }
            
            if (me.Object != null && me.Object.Type == typeof(Model.Attribute))
            {
                return ElementParser.ParseAttribute(me);
            }

            return new ExpressionTerm() { Value = InvokeExpression(me).ToString() };
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
