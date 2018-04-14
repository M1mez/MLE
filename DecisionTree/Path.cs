using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    public class Path
    {
        public Path(int value)
        {
            Value = value;
        }

        public int Value;
        public Node Destination;
    }
}
