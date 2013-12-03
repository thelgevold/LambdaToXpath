﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LambdaToXpath.Model
{
    public class Parent 
    {
        public Parent()
        {
            Attributes = new List<Attribute>();
        }

        public string Name { get; set; }

        public int? Position { get; set; }

        public Attribute Attribute(string name)
        {
            return new Attribute(name);
        }

        internal List<Attribute> Attributes { get; set; }

        public bool ElementHasConditions { get { return Attributes.Count + (Position ?? 0) > 0; } }
    }
}
