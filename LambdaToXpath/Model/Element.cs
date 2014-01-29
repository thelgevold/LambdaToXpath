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
            Relatives = new List<Relative>();
            Equal = true;
        }

        public bool Equal { get; set; }

        public string TargetElementName { get; set; }

        public string TargetElementText { get; set; }

        public bool TextContainsFunction { get; set; } 

        public Parent Parent { get; set; }

        public Relative Descendant { get; set; }

        public Relative Ancestor { get; set; }

        public List<Sibling> Siblings { get; set; }

        public Sibling FollowingSibling { get; set; }

        public Sibling PrecedingSibling { get; set; }

        public int? Position { get; set; }

        public Attribute Attribute(string name)
        {
            return new Attribute(name);
        }

        public List<Relative> Relatives { get; set; }

        public List<Attribute> Attributes { get; set; }

        public bool ElementHasConditions 
        {
            get 
            {
                return (Attributes.Count + Siblings.Count + (Position ?? 0) + Relatives.Count + (TargetElementText ?? string.Empty).Length > 0) ; 
            }
        }
    }
}
