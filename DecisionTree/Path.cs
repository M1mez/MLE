using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    public class Path
    {
        public Path(int level)
        {
            Level = level;
        }

        public int Level;
        public Node Destination;
    }
}
