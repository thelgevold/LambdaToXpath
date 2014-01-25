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
        public string Function { get; set; }

        public string FunctionArgument { get; set; }

        public string Value { get; set; }

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
