using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LambdaToXpath
{
    public static class ElementParser
    {
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
    }
}
