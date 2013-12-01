using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LambdaToXpath.Model
{
    public class Element
    {
        public Element()
        {
            Attributes = new List<Attribute>();
        }

        public string ElementName { get; set; }

        public Parent Parent { get; set; }

        public Sibling FollowingSibling { get; set; }

        public Sibling PreceedingSibling { get; set; }

        public Attribute Attribute(string name)
        {
            return new Attribute(name);
        }

        internal List<Attribute> Attributes { get; set; }
      
    }
}
