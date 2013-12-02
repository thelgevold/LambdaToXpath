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
            Siblings = new List<Sibling>();
        }

        public string TargetElementName { get; set; }

        public Parent Parent { get; set; }

        public List<Sibling> Siblings { get; set; }

        public Sibling FollowingSibling { get; set; }

        public Sibling PrecedingSibling { get; set; }

        public Attribute Attribute(string name)
        {
            return new Attribute(name);
        }

        public List<Attribute> Attributes { get; set; }

        public bool ElementHasConditions { get { return Attributes.Count + Siblings.Count > 0; } }
    }
}
