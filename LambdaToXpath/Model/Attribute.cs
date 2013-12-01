using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LambdaToXpath.Model
{
    public class Attribute
    {
        public Attribute(string name)
        {
            Name = name;
        }

        internal string Name { get; set; } 

        public string Text { get; set; }
    }
}
