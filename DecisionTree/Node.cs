using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    public class Node
    {
        public Node(int attribute)
        {
            Attribute = attribute;
        }
        public int Attribute;
        public List<Path> paths = new List<Path>();
        public int originEdge;
        public bool IsLeaf; //=> DataSet.Attributes.Count - 1 == Attribute;
        
    }
}
