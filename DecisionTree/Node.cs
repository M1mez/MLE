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
            PreviousAttribute = attribute;
        }
        
        public int Attribute;
        public readonly int PreviousAttribute;
        public readonly List<Path> Paths = new List<Path>();
        public int OriginEdge;
        public bool IsLeaf;
        public int Qualifier;
    }
}
