using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LambdaToXpath
{
    public class ExpressionTerm
    {
        public ExpressionTerm()
        {
            ExpressionBooleanCondition = true;
        }

        public string Function { get; set; }

        public string FunctionArgument { get; set; }

        public string Value { get; set; }

        /// <summary>
        /// Function returning bool is equal to this value (if applicable) Ex: e.TargetElement.Text.Contains(..) == ExpressionBooleanCondition
        /// </summary>
        public bool ExpressionBooleanCondition { get; set; } 

        public bool IsValidFunctionByName(string functionName)
        {
            return this.Function.Contains(functionName);
        }

        public bool IsValidFunctionByRegex(string regex)
        {
            return Regex.IsMatch(this.Function, regex);
        }
    }
}
