using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LambdaToXpath
{
    public static class MethodNameMapper
    {
        public static string MapToXpathFunctionName(string name)
        {
            switch (name)
            {
                case "Contains" :
                    return "contains";
                case "StartsWith":
                    return "starts-with";
                case "EndsWith":
                    return "ends-with";
            }

            throw new ArgumentException(name);
        }
    }

}
